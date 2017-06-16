using System;
using System.Collections.Generic;

namespace VaderSharp
{
    internal static class SentimentUtils
    {
        #region Constants

        public const double BIncr = 0.293;
        public const double BDecr = -0.293;
        public const double CIncr = 0.733;
        public const double NScalar = -0.74;

        public static readonly string[] PuncList = 
        {
            ".", "!", "?", ",", ";", ":", "-", "'", "\"","!!", "!!!",
            "??", "???", "?!?", "!?!", "?!?!", "!?!?"
        };

        public static readonly string[] Negate = 
        {
            "aint", "arent", "cannot", "cant", "couldnt", "darent", "didnt", "doesnt",
            "ain't", "aren't", "can't", "couldn't", "daren't", "didn't", "doesn't",
            "dont", "hadnt", "hasnt", "havent", "isnt", "mightnt", "mustnt", "neither",
            "don't", "hadn't", "hasn't", "haven't", "isn't", "mightn't", "mustn't",
            "neednt", "needn't", "never", "none", "nope", "nor", "not", "nothing", "nowhere",
            "oughtnt", "shant", "shouldnt", "uhuh", "wasnt", "werent",
            "oughtn't", "shan't", "shouldn't", "uh-uh", "wasn't", "weren't",
            "without", "wont", "wouldnt", "won't", "wouldn't", "rarely", "seldom", "despite"
        };

        public static readonly Dictionary<string, double> BoosterDict = new Dictionary<string, double>
        {
            {"absolutely", BIncr},
            {"amazingly", BIncr},
            {"awfully", BIncr},
            {"completely", BIncr},
            {"considerably", BIncr},
            {"decidedly", BIncr},
            {"deeply", BIncr},
            {"effing", BIncr},
            {"enormously", BIncr},
            {"entirely", BIncr},
            {"especially", BIncr},
            {"exceptionally", BIncr},
            {"extremely", BIncr},
            {"fabulously", BIncr},
            {"flipping", BIncr },
            {"flippin", BIncr},
            {"fricking", BIncr},
            {"frickin", BIncr},
            {"frigging", BIncr},
            {"friggin", BIncr},
            {"fully", BIncr},
            {"fucking", BIncr},
            {"greatly", BIncr},
            {"hella", BIncr},
            {"highly", BIncr},
            {"hugely", BIncr},
            {"incredibly", BIncr},
            {"intensely", BIncr},
            {"majorly", BIncr},
            {"more", BIncr},
            {"most", BIncr},
            {"particularly", BIncr},
            {"purely", BIncr},
            {"quite", BIncr},
            {"really", BIncr},
            {"remarkably", BIncr},
            {"so", BIncr},
            {"substantially", BIncr},
            {"thoroughly", BIncr},
            {"totally", BIncr},
            {"tremendously", BIncr},
            {"uber", BIncr},
            {"unbelievably", BIncr},
            {"unusually", BIncr},
            {"utterly", BIncr},
            {"very", BIncr},
            { "almost", BDecr},
            { "barely", BDecr},
            { "hardly", BDecr},
            { "just enough", BDecr},
            { "kind of", BDecr},
            { "kinda", BDecr},
            { "kindof", BDecr},
            { "kind-of", BDecr},
            { "less", BDecr},
            { "little", BDecr},
            { "marginally", BDecr},
            { "occasionally", BDecr},
            { "partly", BDecr},
            { "scarcely", BDecr},
            { "slightly", BDecr},
            { "somewhat", BDecr},
            {"sort of", BDecr},
            { "sorta", BDecr},
            { "sortof", BDecr},
            { "sort-of", BDecr}
        };

        public static readonly Dictionary<string, double> SpecialCaseIdioms = new Dictionary<string, double>
        {
            {"the shit", 3},
            { "the bomb", 3},
            { "bad ass", 1.5},
            { "yeah right", -2},
            { "cut the mustard", 2},
            { "kiss of death", -1.5},
            { "hand to mouth", -2}
        };

        #endregion

        #region Util static methods
        /// <summary>
        /// Determine if input contains negation words
        /// </summary>
        /// <param name="inputWords"></param>
        /// <param name="includenT"></param>
        /// <returns></returns>
        public static bool Negated(IList<string> inputWords, bool includenT = true)
        {
            foreach (var word in Negate)
            {
                if (inputWords.Contains(word))
                    return true;
            }

            if (includenT)
            {
                foreach (var word in inputWords)
                {
                    if (word.Contains(@"n't"))
                        return true;
                }
            }

            if (inputWords.Contains("least"))
            {
                int i = inputWords.IndexOf("least");
                if (i > 0 && inputWords[i - 1] != "at")
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Normalizes score to be between -1 and 1
        /// </summary>
        /// <param name="score"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static double Normalize(double score, double alpha = 15)
        {
            double normScore = score / Math.Sqrt(score * score + alpha);

            if (normScore < -1.0)
                return -1.0;

            if (normScore > 1.0)
                return 1.0;

            return normScore;
        }

        /// <summary>
        /// Checks whether some but not all of words in input are ALL CAPS
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool AllCapDifferential(IList<string> words)
        {
            int allCapWords = 0;

            foreach (var word in words)
            {
                if (word.IsUpper())
                    allCapWords++;
            }

            int capDifferential = words.Count - allCapWords;
            return (capDifferential > 0 && capDifferential < words.Count);
        }

        /// <summary>
        /// Check if preceding words increase, decrease or negate the valence
        /// </summary>
        /// <param name="word"></param>
        /// <param name="valence"></param>
        /// <param name="isCapDiff"></param>
        /// <returns></returns>
        public static double ScalarIncDec(string word, double valence, bool isCapDiff)
        {
            string wordLower = word.ToLower();
            if (!BoosterDict.ContainsKey(wordLower))
                return 0.0;

            double scalar = BoosterDict[wordLower];
            if (valence < 0)
                scalar *= -1;

            if (word.IsUpper() && isCapDiff)
            {
                scalar += (valence > 0) ? CIncr : -CIncr;
            }

            return scalar;
        }

        #endregion
    }
}
