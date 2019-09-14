using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BankingFetcher.Extensions.ScrapySharp;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using ScrapySharp.Html.Forms;

[assembly: InternalsVisibleTo("BankingFetcher.Test")]
namespace BankingFetcher.Bank.Wüstenrot
{
    class WüstenrotParser
    {
        public WüstenrotParser(string user, string password)
        {
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.IgnoreCookies = true;

            WebPage PageResult = Browser.NavigateToPage(new Uri("https://www.banking-wuestenrotdirect.de/banking-private/entry"));
            //HtmlNode TitleNode = PageResult.Html.CssSelect(".navbar-brand").First();
            //string PageTitle = TitleNode.InnerText;

            // find a form and send back data
            PageWebForm form = PageResult.FindFormByActionStartsWith("/banking-private/portal?");
            // assign values to the form fields
            form["txtBenutzerkennung"] = user;
            form["pwdPin"] = password;
            form.Method = HttpVerb.Post;
            WebPage resultsPage = form.Submit();
        }
    }
}
