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
        private List<Tuple<PronounciationSource, SourceUsage>> Sources { get; set; }

        public enum SourceUsage
        {
            Simultaneous, // this source will be used at the same time as the previous source. The first one to respond with valid output will be used.
            Fallback, // this source will be used only if the previous source doesn't return a pronounciation
        }

        public PronounciationManager()
        {
            Sources = new List<Tuple<PronounciationSource, SourceUsage>>();
        }

        public void AddSource(PronounciationSource source, SourceUsage usage)
        {
            source.LookupComplete += SourceLookupComplete;
            Sources.Add(new Tuple<PronounciationSource, SourceUsage>(source, usage));
        }

        public void Lookup(string word)
        {
            word = word.ToLower();

            // this should take account of SourceUsage
            foreach (var tuple in Sources)
                tuple.Item1.Lookup(word);
        }

        private void SourceLookupComplete(object sender, PronounciationSource.LookupEventArgs e)
        {
            if (LookupComplete != null)
                LookupComplete(this, e);

            Console.WriteLine("{0}: {1} returned {2}", e.Spelling, (sender as PronounciationSource).Name, e.Pronounciation ?? "<not found>");

            if (e.Pronounciation == null)
            {
                // move to the next stage of source lookupgs
            }
            else
            {
                // terminate all source lookups that are currently running
            }
        }

        public event EventHandler<PronounciationSource.LookupEventArgs> LookupComplete;

        public static PronounciationManager CreateDefault()
        {
            var manager = new PronounciationManager();
            manager.AddSource(new Memory(), SourceUsage.Simultaneous);
            manager.AddSource(new Cambridge(), SourceUsage.Fallback);
            manager.AddSource(new Collins(), SourceUsage.Simultaneous);
            manager.AddSource(new Oxford(), SourceUsage.Simultaneous);
            return manager;
        }
    }
}
