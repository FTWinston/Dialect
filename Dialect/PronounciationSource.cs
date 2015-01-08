using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public abstract class PronounciationSource
    {
        public abstract string Name { get; }
        public abstract void Lookup(string word);

        protected void Succeed(string spelling, string pronounciation)
        {
            if (LookupComplete != null)
                LookupComplete(this, new LookupEventArgs(spelling, pronounciation));
        }

        protected void Fail(string spelling)
        {
            if (LookupComplete != null)
                LookupComplete(this, new LookupEventArgs(spelling, null));
        }

        public event EventHandler<LookupEventArgs> LookupComplete;

        public struct LookupEventArgs
        {
            internal LookupEventArgs(string spelling, string pronounciation)
            {
                Spelling = spelling;
                Pronounciation = pronounciation;
            }

            public string Spelling, Pronounciation;
        }
    }
}
