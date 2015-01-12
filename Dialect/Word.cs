using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public struct Word
    {
        internal Word(string spelling, string pronunciation, bool isPunctuation = false)
        {
            Spelling = spelling;
            Pronunciation = pronunciation;
            IsPunctuation = false;
        }

        public string Spelling, Pronunciation;
        public bool IsPunctuation;

        public static string WritePronounciation(IEnumerable<Word> words)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var word in words)
            {
                sb.Append(" ");
                sb.Append(word.Pronunciation);
            }
            return sb.Remove(0, 1).ToString();
        }
    }
}
