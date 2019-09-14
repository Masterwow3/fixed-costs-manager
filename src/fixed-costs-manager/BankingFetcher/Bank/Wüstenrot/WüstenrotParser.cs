using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("BankingFetcher.Test")]
namespace BankingFetcher.Bank.Wüstenrot
{
    class WüstenrotParser
    {
        public WüstenrotParser(string user, string password)
        {
            //ScrapingBrowser browser = new ScrapingBrowser();
            //browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            //browser.AllowMetaRedirect = true;
            //browser.IgnoreCookies = true;
            

            //WebPage pageResult = browser.NavigateToPage(new Uri("https://www.banking-wuestenrotdirect.de/banking-private/entry"));
            
            ////HtmlNode TitleNode = PageResult.Html.CssSelect(".navbar-brand").First();
            ////string PageTitle = TitleNode.InnerText;

            //// find a form and send back data
            //PageWebForm form = pageResult.FindFormByActionStartsWith("/banking-private/portal?");
            //// assign values to the form fields
            //form["txtBenutzerkennung"] = user;
            //form["pwdPin"] = password;
            //form.Method = HttpVerb.Post;
            //WebPage resultsPage = form.Submit();
        }
    }
}
