using Dialect.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public class PronounciationManager
    {
        private PronounciationStorageSource knownWordSource;
        public PronounciationStorageSource KnownWordSource { get { return knownWordSource; } set { knownWordSource = value; value.LookupComplete += SourceLookupComplete; } }
        private List<PronounciationSource> Sources { get; set; }

        private SortedList<string, int> LookupsInProgress { get; set; }

        public PronounciationManager()
        {
            Sources = new List<PronounciationSource>();
            LookupsInProgress = new SortedList<string, int>();
        }

        public void AddSource(PronounciationSource source)
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

        private void SourceLookupComplete(object sender, PronounciationSource.LookupEventArgs e)
        {
            int lookupStage;
            if (!LookupsInProgress.TryGetValue(e.Spelling, out lookupStage))
                return; // we're not currently looking this word up. This ought not to happen.

            Console.WriteLine("{0}: {1} returned {2}", e.Spelling, (sender as PronounciationSource).Name, e.Pronounciation ?? "<not found>");

            lookupStage++;
            if (e.Pronounciation == null && lookupStage < Sources.Count)
            {
                // try the next source
                LookupsInProgress[e.Spelling] = lookupStage;
                Sources[lookupStage].Lookup(e.Spelling);
            }
            else
            {
                if (e.Pronounciation == null) // have exhausted all of our sources, need to say SOMETHING
                {
                    e.Pronounciation = "something";

                    Console.WriteLine("{0}: No pronounciation found!", e.Spelling);
                }

                LookupsInProgress.Remove(e.Spelling);

                if (sender != KnownWordSource) // remember this word in the future, so we don't need to use slower sources again.
                    KnownWordSource.StoreWord(e.Spelling, e.Pronounciation);

                if (LookupComplete != null)
                    LookupComplete(this, e);
            }
        }

        public event EventHandler<PronounciationSource.LookupEventArgs> LookupComplete;

        public static PronounciationManager CreateDefault()
        {
            var manager = new PronounciationManager();
            manager.KnownWordSource = new Memory(); 

            manager.AddSource(new Cambridge());
            manager.AddSource(new Collins());
            manager.AddSource(new Oxford());
            return manager;
        }
    }
}
