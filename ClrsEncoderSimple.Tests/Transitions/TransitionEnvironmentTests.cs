using ClrsEncoderSimple.Transitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClrsEncoderSimple.Tests.Transitions
{
    [TestClass]
    public class TransitionEnvironmentTests
    {
        [DataTestMethod]
        [DataRow("\\")]
        [DataRow("\\b")]
        [DataRow("\\be")]
        [DataRow("\\beg")]
        [DataRow("\\begi")]
        [DataRow("\\begin")]
        [DataRow("\\begin{")]
        [DataRow("\\begin{e")]
        [DataRow("\\begin{eq")]
        [DataRow("\\begin{equ")]
        [DataRow("\\begin{equa")]
        [DataRow("\\begin{equat")]
        [DataRow("\\begin{equati")]
        [DataRow("\\begin{equatio")]
        [DataRow("\\begin{equation")]
        public void ApplicabilityInconclusive(string text)
        {
            var testee = new TransitionEnvironment(State.Plain, "equation");
            var actualApplicability = testee.IsApplicable(text);
            Assert.AreEqual(Applicability.Inconclusive, actualApplicability);
        }

        [TestMethod]
        public void ApplicabilityYes()
        {
            var testee = new TransitionEnvironment(State.Plain, "equation");
            var actualApplicability = testee.IsApplicable("\\begin{equation}");
            Assert.AreEqual(Applicability.Yes, actualApplicability);
        }

        [DataTestMethod]
        [DataRow(".")]
        [DataRow("\\.")]
        [DataRow("\\b.")]
        [DataRow("\\be.")]
        [DataRow("\\beg.")]
        [DataRow("\\begi.")]
        [DataRow("\\begin.")]
        [DataRow("\\begin{.")]
        [DataRow("\\begin{e.")]
        [DataRow("\\begin{eq.")]
        [DataRow("\\begin{equ.")]
        [DataRow("\\begin{equa.")]
        [DataRow("\\begin{equat.")]
        [DataRow("\\begin{equati.")]
        [DataRow("\\begin{equatio.")]
        [DataRow("\\begin{equation.")]
        public void ApplicabilityNo(string text)
        {
            var testee = new TransitionEnvironment(State.Plain, "equation");
            var actualApplicability = testee.IsApplicable(text);
            Assert.AreEqual(Applicability.No, actualApplicability);
        }
    }
}
