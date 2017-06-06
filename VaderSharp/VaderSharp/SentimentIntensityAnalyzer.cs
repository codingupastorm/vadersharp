using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VaderSharp
{
    public class SentimentIntensityAnalyzer
    {
        private Dictionary<string, double> Lexicon { get; set; }
        private string[] LexiconFullFile { get; set; }

        public SentimentIntensityAnalyzer(string filename = "vader_lexicon.txt")
        {
            LexiconFullFile = File.ReadAllLines(filename);
            Lexicon = MakeLexDic();
        }

        private Dictionary<string,double> MakeLexDic()
        {
            var dic = new Dictionary<string, double>();
            foreach (var line in LexiconFullFile)
            {
                var lineArray = line.Trim().Split('\t');
                dic.Add(lineArray[0], Double.Parse(lineArray[2]));
            }
            return dic;
        }

        /// <summary>
        /// Return a float for sentiment strength based on the input text.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double PolarityScores(string input)
        {
            throw new NotImplementedException();
        }

        private double SentimentValence()
        {
            throw new NotImplementedException();
        }
    }
}
