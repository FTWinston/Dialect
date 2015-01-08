﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dialect.Sources
{
    public class Memory : PronounciationSource
    {
        public override string Name { get { return "Memory"; } }

        SortedList<string, string> KnownWords = new SortedList<string, string>();

        public override void Lookup(string word)
        {
            string ipa;
            if (KnownWords.TryGetValue(word.ToLower(), out ipa))
                Succeed(word, ipa);
            else
                Fail(word);

            // any time ANY source returns a word, want this to remember it. That requires linking this in with the Manager, doesn't it?
        }
    }
}
