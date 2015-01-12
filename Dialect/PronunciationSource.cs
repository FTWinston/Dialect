using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public abstract class PronunciationSource
    {
        public abstract string Name { get; }
        public abstract void Lookup(string word);

        protected void Succeed(string spelling, string pronunciation)
        {
            if (LookupComplete != null)
                LookupComplete(this, new Word(spelling, pronunciation));
        }

        protected void Fail(string spelling)
        {
            if (LookupComplete != null)
                LookupComplete(this, new Word(spelling, null));
        }

        public event EventHandler<Word> LookupComplete;
    }

    public abstract class PronunciationStorageSource : PronunciationSource
    {
        public abstract void StoreWord(string spelling, string pronunciation);
    }
}
