using Dialect.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public class PronunciationManager
    {
        private PronunciationStorageSource knownWordSource;
        public PronunciationStorageSource KnownWordSource { get { return knownWordSource; } set { knownWordSource = value; value.LookupComplete += SourceLookupComplete; } }
        private List<PronunciationSource> Sources { get; set; }

        private SortedList<string, int> LookupsInProgress { get; set; }

        public PronunciationManager()
        {
            Sources = new List<PronunciationSource>();
            LookupsInProgress = new SortedList<string, int>();
        }

        public void AddSource(PronunciationSource source)
        {
            source.LookupComplete += SourceLookupComplete;
            Sources.Add(source);
        }

        public void Lookup(string word)
        {
            word = word.ToLower();

            if (LookupsInProgress.ContainsKey(word))
                return; // the "lookup complete" event should cover the existing lookup and this new one, so do nothing here.

            LookupsInProgress.Add(word, -1);
            KnownWordSource.Lookup(word);
        }

        private void SourceLookupComplete(object sender, Word e)
        {
            int lookupStage;
            if (!LookupsInProgress.TryGetValue(e.Spelling, out lookupStage))
                return; // we're not currently looking this word up. This ought not to happen.

            Console.WriteLine("{0}: {1} returned {2}", e.Spelling, (sender as PronunciationSource).Name, e.Pronunciation ?? "<not found>");

            lookupStage++;
            if (e.Pronunciation == null && lookupStage < Sources.Count)
            {
                // try the next source
                LookupsInProgress[e.Spelling] = lookupStage;
                Sources[lookupStage].Lookup(e.Spelling);
            }
            else
            {
                if (e.Pronunciation == null) // have exhausted all of our sources, need to say SOMETHING
                {
                    e.Pronunciation = "ˈsʌm.θɪŋ";

                    Console.WriteLine("{0}: No pronunciation found!", e.Spelling);
                }

                LookupsInProgress.Remove(e.Spelling);

                if (sender != KnownWordSource) // remember this word in the future, so we don't need to use slower sources again.
                    KnownWordSource.StoreWord(e.Spelling, e.Pronunciation);

                if (LookupComplete != null)
                    LookupComplete(this, e);
            }
        }

        public event EventHandler<Word> LookupComplete;

        public static PronunciationManager CreateDefault()
        {
            var manager = new PronunciationManager();
            manager.KnownWordSource = new Memory(); 

            manager.AddSource(new Cambridge());
            manager.AddSource(new Collins());
            manager.AddSource(new Oxford());
            return manager;
        }
    }
}
