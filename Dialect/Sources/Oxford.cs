using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialect.Sources
{
    public class Oxford : WebScraper
    {
        public override string Name { get { return "Oxford"; } }
        protected override string UrlFormat { get { return "http://www.oxforddictionaries.com/search/?direct=1&multi=1&dictCode=english&q={0}"; } }
        protected override string ElementXPath { get { return "//header[@class=\"entryHeader\"]//div[@class=\"headpron\"]/text()"; } }
    }
}
