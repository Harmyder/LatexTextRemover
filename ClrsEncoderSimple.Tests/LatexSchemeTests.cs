using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClrsEncoderSimple.Tests
{
    [TestClass]
    internal class LatexSchemeTests
    {
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
