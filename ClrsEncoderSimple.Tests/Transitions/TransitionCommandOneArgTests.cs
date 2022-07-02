using ClrsEncoderSimple.Transitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClrsEncoderSimple.Tests.Transitions
{
    [TestClass]
    public class TransitionCommandOneArgTests
    {
        [DataTestMethod]
        [DataRow("\\")]
        [DataRow("\\c")]
        [DataRow("\\cm")]
        [DataRow("\\cmd")]
        public void OneArgInconclusive(string text)
        {
            var transition = new TransitionCommandOneArg(State.Plain, "cmd");
            var actualAppicability = transition.IsApplicable(text);
            Assert.AreEqual(Applicability.Inconclusive, actualAppicability);
        }

        [TestMethod]
        public void OneArgInconclusive()
        {
            var transition = new TransitionCommandOneArg(State.Plain, "cmd");
            var actualAppicability = transition.IsApplicable("\\cmd{");
            Assert.AreEqual(Applicability.Yes, actualAppicability);
        }

        [TestMethod]
        public void ClosingYes()
        {
            var transition = new TransitionCommandOneArg();
            var actualApplicability = transition.IsApplicable("}");
            Assert.AreEqual(Applicability.Yes, actualApplicability);
        }

        [TestMethod]
        public void ClosingNo()
        {
            var transition = new TransitionCommandOneArg();
            var actualApplicability = transition.IsApplicable(".");
            Assert.AreEqual(Applicability.No, actualApplicability);
        }
    }
}
