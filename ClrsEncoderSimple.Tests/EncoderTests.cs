using ClrsEncoderSimple.Transitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClrsEncoderSimple.Tests
{
    [TestClass]
    public class EncoderTests
    {
        [TestMethod]
        public void Spaces()
        {
            var text = "Hello World!";
            var expectedTextNoText = "????? ??????";
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
            var expectedTextNoText = "\\Command";
            var transitions = new Transition[]
            {
                new TransitionChars(State.Plain, State.Command, "\\", TriggerMeaning.Positive),
                new TransitionStrings(State.Plain, State.MathDisplayLatex, "\\["),
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
        public void Environment()
        {
            var text = "\\begin{equation}[text]text";
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

        [DataTestMethod]
        [DataRow("{and{hello}world}")]
        [DataRow("{and[hello]world}")]
        [DataRow("[and{hello}world]")]
        [DataRow("[and[hello]world]")]
        public void NestedBraces(string text)
        {
            var expectedTextNoText = text;
            var encoder = new Encoder(LatexScheme.Transitions);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }
    }
}