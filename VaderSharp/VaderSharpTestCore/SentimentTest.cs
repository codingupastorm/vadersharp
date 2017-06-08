using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaderSharp;

namespace VaderSharpTestCore
{
    [TestClass]
    public class SentimentTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();
            var test1 = analyzer.PolarityScores("VADER is smart, handsome, and funny.");
            Assert.AreEqual(test1.Negative, 0);
            Assert.AreEqual(test1.Positive, 0.746);
            Assert.AreEqual(test1.Neutral, 0.254);
            Assert.AreEqual(test1.Compound, 0.8316);
        }
    }
}
