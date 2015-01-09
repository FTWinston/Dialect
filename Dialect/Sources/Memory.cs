using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dialect.Sources
{
    public class Memory : PronunciationStorageSource
    {
        public override string Name { get { return "Memory"; } }

        private SortedList<string, string> KnownWords = new SortedList<string, string>();

        public override void Lookup(string word)
        {
            string pronunciation;
            if (KnownWords.TryGetValue(word, out pronunciation))
                Succeed(word, pronunciation);
            else
                Fail(word);
        }

        public override void StoreWord(string spelling, string pronunciation)
        {
            KnownWords[spelling] = pronunciation;
        }
    }
}
