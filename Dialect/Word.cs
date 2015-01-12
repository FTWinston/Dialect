using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public struct Word
    {
        internal Word(string spelling, string pronunciation)
        {
            Spelling = spelling;
            Pronunciation = pronunciation;
        }

        public string Spelling, Pronunciation;
    }
}
