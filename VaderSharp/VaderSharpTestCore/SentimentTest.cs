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
            var test = analyzer.PolarityScores("VADER is smart, handsome, and funny.");
            Console.WriteLine(test);
        }
    }
}
