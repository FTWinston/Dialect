using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect.Sources
{
    public class Cambridge : WebScraper
    {
        public override string Name { get { return "Cambridge"; } }
        protected override string UrlFormat { get { return "http://dictionary.cambridge.org/dictionary/british/{0}"; } }
        protected override string ElementXPath { get { return "//span[@class=\"uk\"]//span[@class=\"ipa\"]"; } }
    }
}
