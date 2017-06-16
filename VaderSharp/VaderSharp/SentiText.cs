using System.Collections.Generic;
using System.Linq;

namespace VaderSharp
{
    internal class SentiText
    {
        private string Text { get; }
        public IList<string> WordsAndEmoticons { get; }
        public bool IsCapDifferential { get; }

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
            var wordsOnly = noPuncText.Split().Where(x=>x.Length > 1);

            //for each word in wordsOnly, get each possible variant of punclist before/after
            //Seems poor. Maybe I can improve in future.
            Dictionary<string,string> puncDic = new Dictionary<string, string>();
            foreach (var word in wordsOnly)
            {
                foreach (var punc in SentimentUtils.PuncList)
                {
                    if (puncDic.ContainsKey(word + punc))
                        continue;

                    puncDic.Add(word + punc, word);
                    puncDic.Add(punc + word, word);
                }
            }
            return puncDic;
        }


        /// <summary>
        /// Removes leading and trailing punctuation. Leaves contractions and most emoticons.
        /// </summary>
        /// <returns></returns>
        private IList<string> GetWordsAndEmoticons()
        {
            IList<string> wes = Text.Split().Where(x=> x.Length > 1).ToList();
            Dictionary<string,string> wordsPuncDic = WordsPlusPunc();
            for (int i = 0; i < wes.Count; i++)
            {
                if (wordsPuncDic.ContainsKey(wes[i]))
                    wes[i] = wordsPuncDic[wes[i]];
            }

            return wes;
        }

    }
}
