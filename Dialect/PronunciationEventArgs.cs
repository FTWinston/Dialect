using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public struct PronunciationEventArgs
    {
        internal PronunciationEventArgs(string spelling, string pronunciation)
        {
            Spelling = spelling;
            Pronunciation = pronunciation;
        }

        public string Spelling, Pronunciation;
    }
}
