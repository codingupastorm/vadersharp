using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaderSharp
{
    internal class SentiText
    {
        private string Text { get; set; }
        private IList<string> WordsAndEmoticons { get; set; }
        private bool IsCapDifferential { get; set; }

        public SentiText(string text)
        {
            //TODO: Encode in UTF-8 ?
            Text = text;
            WordsAndEmoticons = GetWordsAndEmoticons();
            IsCapDifferential = SentimentUtils.AllCapDifferential(WordsAndEmoticons);
        }


        /// <summary>
        /// Returns mapping of the form {'cat,': 'cat'}, {',cat': 'cat'}
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> WordsPlusPunc()
        {
            string noPuncText = Text.RemovePunctuation();
            string[] wordsOnly = noPuncText.Split().Where(x=>x.Length > 1).ToArray();

            //for each word in wordsOnly, get each possible variant of punclist before/after

            throw new NotImplementedException();
        }


        /// <summary>
        /// Removes leading and trailing punctuation. Leaves contractions and most emoticons.
        /// </summary>
        /// <returns></returns>
        private IList<string> GetWordsAndEmoticons()
        {
            IList<string> wes = Text.Split();
            Dictionary<string,string> wordsPuncDic = WordsPlusPunc();
            throw new NotImplementedException();
        }

    }
}
