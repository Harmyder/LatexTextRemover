using ClrsEncoderSimple.Transitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ClrsEncoderSimple.Tests
{
    [TestClass]
    public class EncoderTests
    {
        [TestMethod]
        public void Spaces()
        {
            var text = "Hello World";
            var expectedTextNoText = "????? ?????";
            var transitions = new Transition[]
            {
            };
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
                new TransitionChars(State.Command, CharClasses.Alphas, TriggerMeaning.Negative, true),
                new TransitionChars(State.Plain, State.Brackets, "[", TriggerMeaning.Positive),
                new TransitionChars(State.Brackets, "]", TriggerMeaning.Positive),
            };
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
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
            var encoder = new Encoder(transitions);
            Assert.ThrowsException<Exception>(() => encoder.Apply(text));
        }
    }
}