using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaderSharp;
namespace VaderSharpTests
{
    [TestClass]
    public class SentimentAnalyzerTest
    {
        [TestMethod]
        public void Test()
        {
            SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();
            var results = analyzer.PolarityScores("VADER is smart, handsome, and funny.");
            Console.WriteLine("Positive: " + results.Positive);
            Console.WriteLine("Negative: " + results.Negative);
            Console.WriteLine("Positive: " + results.Neutral);
        }
    }
}
