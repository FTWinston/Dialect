using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect.Sources
{
    public class Collins : WebScraper
    {
        public override string Name { get { return "Collins"; } }
        protected override string UrlFormat { get { return "http://www.collinsdictionary.com/dictionary/english/{0}"; } }
        protected override string ElementXPath { get { return "//h2[@class=\"orth\"]//span[@class=\"pron\"]/text()"; } }
    }
}
