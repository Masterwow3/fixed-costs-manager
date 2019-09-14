using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Html.Forms;
using ScrapySharp.Network;

namespace BankingFetcher.Extensions.ScrapySharp
{
    static class ScrapySharpWebPageExtensions
    {
        public static PageWebForm FindFormByActionStartsWith(this WebPage page, string actionStartText)
        {
            HtmlNode html = page.Html.Descendants("form").FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)
                (f => f.Attributes.FirstOrDefault(x=>x.Name == "action")?.Value.StartsWith(actionStartText)??false));
            if (html != null)
                return new PageWebForm(html, page.Browser);
            return (PageWebForm)null;
        }
    }
}
