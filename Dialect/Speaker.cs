using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect
{
    public class Speaker
    {
        public Dialect Dialect { get; set; }
        public PronounciationManager Pronounciation { get; set; }

        public Speaker()
        {
            Dialect = new Dialect();
            Pronounciation = PronounciationManager.CreateDefault();
        }

        public string GenerateIPA(string text)
        {
            // lookup each word in turn, generate IPA text.
            string ipa_rp = null;


            string ipa = Dialect.PerformSubstitution(ipa_rp);

            throw new NotImplementedException();
        }
    }
}
