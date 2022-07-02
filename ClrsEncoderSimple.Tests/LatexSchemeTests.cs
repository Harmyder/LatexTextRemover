using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClrsEncoderSimple.Tests
{
    [TestClass]
    public class LatexSchemeTests
    {
        [DataTestMethod]
        [DataRow("{and{hello}world}")]
        [DataRow("{and[hello]world}")]
        [DataRow("[and{hello}world]")]
        [DataRow("[and[hello]world]")]
        public void NestedBraces(string text)
        {
            var expectedTextNoText = text;
            var encoder = new Encoder(LatexScheme.Transitions, LatexScheme.ReplacementLevels);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }

        [TestMethod]
        public void EnvironmentRewinding()
        {
            var replacable = "abcdf";
            var text = "\\begin{theorem}" + replacable + "\\command\\end{theorem}";
            var expectedTextNoText = text.Replace(replacable, "?????");
            var encoder = new Encoder(LatexScheme.Transitions, LatexScheme.ReplacementLevels);
            var actualTextNoText = encoder.Apply(text);
            Assert.AreEqual(expectedTextNoText, actualTextNoText);
        }
    }
}
