using System;
using System.Collections.Generic;
using System.Linq;

namespace VaderSharp
{
    public partial class SentimentIntensityAnalyzer
    {
        

        public SentimentIntensityAnalyzer() { }

        #region Static Methods

        /// <summary>
        /// Determine if word is ALL CAPS
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static bool IsUpper(string word)
        {
            return !word.Any(char.IsLower);
        }

        /// <summary>
        /// Determine if input contains negation words
        /// </summary>
        /// <param name="inputWords"></param>
        /// <param name="includenT"></param>
        /// <returns></returns>
        private static bool Negated(IList<string> inputWords, bool includenT)
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
        private static double Normalize(double score, double alpha = 15)
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
        private static bool AllCapDifferential(IList<string> words)
        {
            int allCapWords = 0;

            foreach (var word in words)
            {
                if (IsUpper(word))
                    allCapWords++;
            }

            int capDifferential = words.Count - allCapWords;
            if (capDifferential > 0 && capDifferential < words.Count)
                return true;

            return false;
        }

        /// <summary>
        /// Check if preceding words increase, decrease or negate the valence
        /// </summary>
        /// <param name="word"></param>
        /// <param name="valence"></param>
        /// <param name="isCapDiff"></param>
        /// <returns></returns>
        private static double ScalarIncDec(string word, double valence, bool isCapDiff)
        {
            string wordLower = word.ToLower();
            if (!BoosterDict.ContainsKey(wordLower))
                return 0.0;

            double scalar = BoosterDict[word];
            if (valence < 0)
                scalar *= -1;

            if (IsUpper(word) && isCapDiff)
            {
                scalar += (valence > 0) ? CIncr :  -CIncr;
            }

            return scalar;
        }

        #endregion


    }
}
