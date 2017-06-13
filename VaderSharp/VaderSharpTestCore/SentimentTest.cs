using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaderSharp;

namespace VaderSharpTestCore
{
    [TestClass]
    public class SentimentTest
    {
        [TestMethod]
        public void MatchPythonTest()
        {
            SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

            var standardGoodTest = analyzer.PolarityScores("VADER is smart, handsome, and funny.");
            Assert.AreEqual(standardGoodTest.Negative, 0);
            Assert.AreEqual(standardGoodTest.Neutral, 0.254);
            Assert.AreEqual(standardGoodTest.Positive, 0.746);
            Assert.AreEqual(standardGoodTest.Compound, 0.8316);

            var kindOfTest = analyzer.PolarityScores("The book was kind of good.");
            Assert.AreEqual(kindOfTest.Negative, 0);
            Assert.AreEqual(kindOfTest.Neutral, 0.657);
            Assert.AreEqual(kindOfTest.Positive, 0.343);
            Assert.AreEqual(kindOfTest.Compound, 0.3832);

            var complexTest =
                analyzer.PolarityScores(
                    "The plot was good, but the characters are uncompelling and the dialog is not great.");
            Assert.AreEqual(complexTest.Negative, 0.327);
            Assert.AreEqual(complexTest.Neutral, 0.579);
            Assert.AreEqual(complexTest.Positive, 0.094);
            Assert.AreEqual(complexTest.Compound, -0.7042);

        }
    }
}
