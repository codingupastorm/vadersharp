using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VaderSharp
{
    public class SentimentIntensityAnalyzer
    {
        private const double ExclIncr = 0.292;
        private const double QuesIncrSmall = 0.18;
        private const double QuesIncrLarge = 0.96;

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
            SentiText sentiText = new SentiText(input);
            throw new NotImplementedException();
        }

        private double SentimentValence(double valence, SentiText sentiText)
        {
            throw new NotImplementedException();
        }

        private double LeastCheck(double valence)
        {
            return valence;
            throw new NotImplementedException();
        }

        private double PunctuationEmphasis(string text)
        {
            return AmplifyExclamation(text) + AmplifyQuestion(text);
        }

        private double AmplifyExclamation(string text)
        {
            int epCount = text.Count(x => x == '!');

            if (epCount > 4)
                epCount = 4;

            return epCount * ExclIncr;
        }

        private double AmplifyQuestion(string text)
        {
            int qmCount = text.Count(x => x == '?');

            if (qmCount < 1)
                return 0;

            if (qmCount <= 3)
                return qmCount * QuesIncrSmall;

            return QuesIncrLarge;
        }

        private SiftSentiments SiftSentimentScores(IList<double> sentiments)
        {
            SiftSentiments siftSentiments = new SiftSentiments();

            foreach (var sentiment in sentiments)
            {
                if (sentiment > 0)
                    siftSentiments.PosSum += (sentiment + 1); //1 compensates for neutrals

                if (sentiment < 0)
                    siftSentiments.NegSum += (sentiment - 1);

                if (sentiment == 0)
                    siftSentiments.NeuCount++;
            }
            return siftSentiments;
        }

        private SentimentAnalysisResults ScoreValence(IList<double> sentiments, string text)
        {
            if (sentiments.Count == 0)
                return new SentimentAnalysisResults(); //will return with all 0

            double sum = sentiments.Sum();
            double puncAmplifier = PunctuationEmphasis(text);

            if (sum > 0)
            {
                sum += puncAmplifier;
            }
            else if (sum < 0)
            {
                sum -= puncAmplifier;
            }

            double compound = SentimentUtils.Normalize(sum);
            SiftSentiments sifted = SiftSentimentScores(sentiments);
            throw new NotImplementedException();
        }

        private class SiftSentiments
        {
            public double PosSum { get; set; }
            public double NegSum { get; set; }
            public int NeuCount { get; set; }
        }
    }

}
