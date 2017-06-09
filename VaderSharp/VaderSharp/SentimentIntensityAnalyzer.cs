using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using VaderSharp.Properties;

namespace VaderSharp
{
    public class SentimentIntensityAnalyzer
    {
        private const double ExclIncr = 0.292;
        private const double QuesIncrSmall = 0.18;
        private const double QuesIncrLarge = 0.96;

        private Dictionary<string, double> Lexicon { get; set; }
        private string[] LexiconFullFile { get; set; }

        public SentimentIntensityAnalyzer()
        {
            LexiconFullFile = Resources.VaderLexicon.Split('\n');
            Lexicon = MakeLexDic();
        }

        private Dictionary<string,double> MakeLexDic()
        {
            var dic = new Dictionary<string, double>();
            foreach (var line in LexiconFullFile)
            {
                var lineArray = line.Trim().Split('\t');
                dic.Add(lineArray[0], Double.Parse(lineArray[1]));
            }
            return dic;
        }

        /// <summary>
        /// Return metrics for positive, negative and neutral sentiment based on the input text.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SentimentAnalysisResults PolarityScores(string input)
        {
            SentiText sentiText = new SentiText(input);
            IList<double> sentiments = new List<double>();
            IList<string> wordsAndEmoticons = sentiText.WordsAndEmoticons;

            for (int i = 0; i < wordsAndEmoticons.Count; i++)
            {
                string item = wordsAndEmoticons[i];
                double valence = 0;
                if (i < wordsAndEmoticons.Count - 1 && item.ToLower() == "kind" && wordsAndEmoticons[i + 1] == "of"
                    || SentimentUtils.BoosterDict.ContainsKey(item.ToLower()))
                {
                    sentiments.Add(valence);
                    continue;
                }
                sentiments = SentimentValence(valence, sentiText, item, i, sentiments);
            }

            sentiments = ButCheck(wordsAndEmoticons, sentiments);

            return ScoreValence(sentiments, input);
        }

        private IList<double> SentimentValence(double valence, SentiText sentiText, string item, int i, IList<double> sentiments)
        {
            string itemLowerCase = item.ToLower();
            if (!Lexicon.ContainsKey(itemLowerCase))
            {
                sentiments.Add(valence);
                return sentiments;
            }
            bool isCapDiff = sentiText.IsCapDifferential;
            IList<string> wordsAndEmoticons = sentiText.WordsAndEmoticons;
            valence = Lexicon[itemLowerCase];
            if (item.IsUpper() && isCapDiff)
            {
                if (valence > 0)
                {
                    valence += SentimentUtils.CIncr;
                }
                else
                {
                    valence -= SentimentUtils.CIncr;
                }
            }

            for (int startI = 0; startI < 3; startI++)
            {
                if (i > startI && !Lexicon.ContainsKey(wordsAndEmoticons[i - (startI + 1)].ToLower()))
                {
                    double s = SentimentUtils.ScalarIncDec(wordsAndEmoticons[i - (startI + 1)], valence, isCapDiff);
                    if (startI == 1 && s != 0)
                        s = s * 0.95;
                    if (startI == 2 && s != 0)
                        s = s * 0.9;
                    valence = valence + s;

                    valence = NeverCheck(valence, wordsAndEmoticons, startI, i);

                    if (startI == 2)
                    {
                        valence = IdiomsCheck(valence, wordsAndEmoticons, i);
                    }

                }
            }

            valence = LeastCheck(valence, wordsAndEmoticons, i);
            sentiments.Add(valence);
            return sentiments;
        }

        private IList<double> ButCheck(IList<string> wordsAndEmoticons, IList<double> sentiments)
        {
            bool containsBUT = wordsAndEmoticons.Contains("BUT");
            bool containsbut = wordsAndEmoticons.Contains("but");
            if (!containsBUT && !containsbut)
                return sentiments;

            int butIndex = (containsBUT) 
                ? wordsAndEmoticons.IndexOf("BUT") 
                : wordsAndEmoticons.IndexOf("but");

            for (int i = 0; i < sentiments.Count; i++)
            {
                double sentiment = sentiments[i];
                if (i < butIndex)
                {
                    sentiments.RemoveAt(i);
                    sentiments.Insert(i,sentiment*0.5);
                }
                else if (i > butIndex)
                {
                    sentiments.RemoveAt(i);
                    sentiments.Insert(i, sentiment * 1.5);
                }
            }
            return sentiments;
        }

        private double LeastCheck(double valence, IList<string> wordsAndEmoticons, int i)
        {
            if (i > 1 && !Lexicon.ContainsKey(wordsAndEmoticons[i - 1].ToLower()) &&
                wordsAndEmoticons[i - 1].ToLower() == "least")
            {
                if (wordsAndEmoticons[i - 2].ToLower() != "at" && wordsAndEmoticons[i - 2].ToLower() != "very")
                {
                    valence = valence * SentimentUtils.NScalar;
                }
            }
            else if (i > 0 && !Lexicon.ContainsKey(wordsAndEmoticons[i-1].ToLower()) 
                && wordsAndEmoticons[i - 1].ToLower() == "least")
            {
                valence = valence * SentimentUtils.NScalar;
            }

            return valence;
        }

        private double NeverCheck(double valence, IList<string> wordsAndEmoticons, int startI, int i)
        {
            if (startI == 0)
            {
                if (SentimentUtils.Negated(new List<string> {wordsAndEmoticons[i - 1]}))
                    valence = valence * SentimentUtils.NScalar;
            }
            if (startI == 1)
            {
                if (wordsAndEmoticons[i - 2] == "never" &&
                    (wordsAndEmoticons[i - 1] == "so" || wordsAndEmoticons[i - 1] == "this"))
                {
                    valence = valence * 1.5;
                }
                else if (SentimentUtils.Negated(new List<string> {wordsAndEmoticons[i - (startI + 1)]}))
                {
                    valence = valence * SentimentUtils.NScalar;
                }
            }
            if (startI == 2)
            {
                if (wordsAndEmoticons[i - 3] == "never"
                    && (wordsAndEmoticons[i - 2] == "so" || wordsAndEmoticons[i - 2] == "this")
                    || (wordsAndEmoticons[i - 1] == "so" || wordsAndEmoticons[i - 1] == "this"))
                {
                    valence = valence * 1.25;
                }
                else if (SentimentUtils.Negated(new List<string> { wordsAndEmoticons[i - (startI + 1)] }))
                {
                    valence = valence * SentimentUtils.NScalar;
                }
            }

            return valence;
        }

        private double IdiomsCheck(double valence, IList<string> wordsAndEmoticons, int i)
        {
            string oneZero = String.Format("{0} {1}", wordsAndEmoticons[i - 1], wordsAndEmoticons[i]);

            string twoOneZero = String.Format("{0} {1} {2}", wordsAndEmoticons[i-2], wordsAndEmoticons[i-1], wordsAndEmoticons[i]);

            string twoOne = String.Format("{0} {1}", wordsAndEmoticons[i - 2], wordsAndEmoticons[i - 1]);

            string threeTwoOne = String.Format("{0} {1} {2}", wordsAndEmoticons[i - 3], wordsAndEmoticons[i - 2],
                wordsAndEmoticons[i - 1]);

            string threeTwo = String.Format("{0} {1}", wordsAndEmoticons[i - 3], wordsAndEmoticons[i - 2]);

            string[] sequences = new String[] {oneZero, twoOneZero, twoOne, threeTwoOne, threeTwo};

            foreach (var seq in sequences)
            {
                if (SentimentUtils.SpecialCaseIdioms.ContainsKey(seq))
                {
                    valence = SentimentUtils.SpecialCaseIdioms[seq];
                    break;
                }
            }

            if (wordsAndEmoticons.Count - 1 > i)
            {
                string zeroOne = String.Format("{0} {1}", wordsAndEmoticons[i], wordsAndEmoticons[i + 1]);
                if (SentimentUtils.SpecialCaseIdioms.ContainsKey(zeroOne))
                {
                    valence = SentimentUtils.SpecialCaseIdioms[zeroOne];
                }
            }
            if (wordsAndEmoticons.Count - 1 > i + 1)
            {
                string zeroOneTwo = String.Format("{0} {1} {2}", wordsAndEmoticons[i], wordsAndEmoticons[i + 1],
                    wordsAndEmoticons[i + 2]);
                if (SentimentUtils.SpecialCaseIdioms.ContainsKey(zeroOneTwo))
                {
                    valence = SentimentUtils.SpecialCaseIdioms[zeroOneTwo];
                }
            }
            if (SentimentUtils.BoosterDict.ContainsKey(threeTwo) || SentimentUtils.BoosterDict.ContainsKey(twoOne))
            {
                valence += SentimentUtils.BDecr;
            }
            return valence;
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

            if (sifted.PosSum > Math.Abs(sifted.NegSum))
            {
                sifted.PosSum += puncAmplifier;
            }
            else if (sifted.PosSum < Math.Abs(sifted.NegSum))
            {
                sifted.NegSum -= puncAmplifier;
            }

            double total = sifted.PosSum + Math.Abs(sifted.NegSum) + sifted.NeuCount;
            return new SentimentAnalysisResults
            {
                Compound = Math.Round(compound,4),
                Positive = Math.Round(Math.Abs(sifted.PosSum /total), 3),
                Negative = Math.Round(Math.Abs(sifted.NegSum/total),3),
                Neutral = Math.Round(Math.Abs(sifted.NeuCount/total), 3)
            };
        }

        private class SiftSentiments
        {
            public double PosSum { get; set; }
            public double NegSum { get; set; }
            public int NeuCount { get; set; }
        }
    }

}
