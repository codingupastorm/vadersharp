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

        private Dictionary<string, string> WordsPlusPunc()
        {
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
