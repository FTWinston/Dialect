using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dialect.Sources
{
    public abstract class WebScraper : PronounciationSource
    {
        protected abstract string UrlFormat { get; }
        protected abstract string ElementXPath { get; }

        public override void Lookup(string word)
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.DownloadStringCompleted += (o, e) => {
                if (e.Cancelled || e.Error != null)
                {
                    Fail(word);
                    return;
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(e.Result);

                var node = doc.DocumentNode.SelectSingleNode(ElementXPath);
                if (node == null)
                {
                    Fail(word);
                    return;
                }

                Succeed(word, node.InnerText.Replace("&nbsp;", " ").Trim(trimChars));
            };
            client.DownloadStringAsync(new Uri(string.Format(UrlFormat, word)));
        }

        private static readonly char[] trimChars = { ' ', '/', '(', ')' }; // collins has the pronounciation inside brackets, but is it always safe to strip these?
    }
}
