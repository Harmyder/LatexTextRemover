using ClrsEncoderSimple.Transitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ClrsEncoderSimple.Tests
{
    [TestClass]
    public class EncoderTests
    {
        private readonly Level[] OnlyPlain = new Level[] { new Level(State.Plain, null) };

        [TestMethod]
        public void Spaces()
        {
            var text = "Hello World";
            var expectedTextNoText = "????? ?????";
            var transitions = new Transition[]
            {
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void CharsAndStrings()
        {
            var text = "Hello $$ A_0 $$ olleH";
            var expectedTextNoText = "????? $$ A_0 $$ ?????";
            var transitions = new Transition[]
            {
                new TransitionChars(State.Plain, State.MathInline, "$", TriggerMeaning.Positive),
                new TransitionChars(State.MathInline, "$", TriggerMeaning.Positive),
                new TransitionStrings(State.Plain, State.MathDisplayDollars, "$$"),
                new TransitionStrings(State.MathDisplayDollars, "$$"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void InconclusiveFailsWithYesAvailable()
        {
            var text = "\\Command";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
                new TransitionStrings(State.Plain, State.MathDisplayLatex, "\\["),
                new TransitionChars(State.Command, string.Empty, TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void Eof()
        {
            var text = "\\a";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
                new TransitionChars(State.Command, string.Empty, TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void Rewind()
        {
            var text = "\\command[text]text";
            var expectedTextNoText = "\\command[text]????";
            var transitions = new Transition[]
            {
                new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
                new TransitionChars(State.Command, CharClasses.Alphas, TriggerMeaning.Negative),
                new TransitionChars(State.Plain, State.Brackets, "[", TriggerMeaning.Positive),
                new TransitionChars(State.Brackets, "]", TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void RewindTheSameTransition()
        {
            var text = "\\command\\command";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
                new TransitionChars(State.Command, CharClasses.Alphas, TriggerMeaning.Negative),
                new TransitionChars(State.Command, string.Empty, TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void RewindAgainEnvironment()
        {
            var text = "\\begin{one}\\begin{two}\\end{two}\\end{one}";
            var expectedTextNoText = "\\begin{one}\\begin{two}\\end{two}\\???{???}";
            var transitions = new Transition[]
            {
                new TransitionEnvironmentAny(new[] { State.Plain, State.Environment }),
                new TransitionEnvironmentAny(),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void EnvironmentEndWithoutBegin()
        {
            var text = "\\end{equation}sdfsdf";
            var expectedTextNoText = "\\???{????????}??????";
            var transitions = new Transition[]
            {
                new TransitionEnvironment(State.Plain, "equation"),
                new TransitionEnvironment("equation"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void EnvironmentBeginWithoutEnd()
        {
            var text = "\\begin{equation}";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionEnvironment(State.Plain, "equation"),
                new TransitionEnvironment("equation"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            Assert.ThrowsException<ArgumentException>(() => encoder.Apply(text));
        }

        [TestMethod]
        public void Environment()
        {
            var textBase = "\\begin{equation}[text]text\\end{equation}";
            var text = textBase + "12345";
            var expectedTextNoText = textBase + "?????";
            var transitions = new Transition[]
            {
                new TransitionEnvironment(State.Plain, "equation"),
                new TransitionEnvironment("equation"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void EnvironmentSameNested()
        {
            var text = "\\begin{equation}\\begin{equation}\\end{equation}\\end{equation}";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionEnvironment(State.Plain, "equation"),
                new TransitionEnvironment(State.Environment, "equation"),
                new TransitionEnvironment("equation"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void EnvironmentProperlyNested()
        {
            var text = "\\begin{proof}\\begin{equation}\\end{equation}\\end{proof}";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionEnvironment(State.Plain, "proof"),
                new TransitionEnvironment("proof"),
                new TransitionEnvironment(State.Environment, "equation"),
                new TransitionEnvironment("equation"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void EnvironmentImproperlyNested()
        {
            var text = "\\begin{proof}\\begin{equation}\\end{proof}\\end{equation}";
            var expectedTextNoText = text;
            var transitions = new Transition[]
            {
                new TransitionEnvironment(State.Plain, "proof"),
                new TransitionEnvironment(State.Environment, "proof"),
                new TransitionEnvironment("proof"),
                new TransitionEnvironment(State.Plain, "equation"),
                new TransitionEnvironment(State.Environment, "equation"),
                new TransitionEnvironment("equation"),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            Assert.ThrowsException<InvalidOperationException>(() => encoder.Apply(text));
        }

        [TestMethod]
        public void CommandOneArgNoDueToWrongNonAlpha()
        {
            var text = "\\cmd[opt]{man}";
            var expectedTextNoText = "\\???[opt]{man}";
            var transitions = new Transition[]
            {
                new TransitionCommandOneArg(State.Plain, "cmd"),
                new TransitionCommandOneArg(),
                new TransitionChars(State.Plain, State.Brackets, "[", TriggerMeaning.Positive),
                new TransitionChars(State.Brackets, "]", TriggerMeaning.Positive),
                new TransitionChars(State.Plain, State.CurlyBraces, "{", TriggerMeaning.Positive),
                new TransitionChars(State.CurlyBraces, "}", TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, OnlyPlain);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void BracesInsideOneArg()
        {
            var text = "\\cmd{man{text}more}";
            var expectedTextNoText = "\\cmd{???{text}????}";
            var transitions = new Transition[]
            {
                new TransitionCommandOneArg(State.Plain, "cmd"),
                new TransitionCommandOneArg(),
                new TransitionChars(State.CommandOneArg, State.CurlyBraces, "{", TriggerMeaning.Positive),
                new TransitionChars(State.CurlyBraces, "}", TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, new Level[] { new Level(State.CommandOneArg, null) });
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void Gobbling()
        {
            var text = "\\$";
            var expectedTextNoText = "\\$";
            var transitions = new Transition[]
            {
                new TransitionGobbler(State.Plain, "\\$"),
                new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
                new TransitionChars(State.Plain, State.MathInline, "$", TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions, new Level[] { new Level(State.CommandOneArg, null) });
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }
    }
}