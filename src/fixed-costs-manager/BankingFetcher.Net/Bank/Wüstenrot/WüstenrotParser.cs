using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[assembly: InternalsVisibleTo("BankingFetcher.Net.Test")]
namespace BankingFetcher.Net.Bank.Wüstenrot
{
    class WüstenrotParser : IDisposable
    {
        private ChromeDriver _driver;
        private bool _loginSuceeded;

        public WüstenrotParser()
        {
            _driver = new ChromeDriver();
           
        }

        public void Login(string user, string password)
        {
            _driver.Navigate().GoToUrl("https://www.banking-wuestenrotdirect.de/banking-private/entry");
            //the driver can now provide you with what you need (it will execute the script)
            //get the source of the page
            var source = _driver.PageSource;
            //fully navigate the dom
            _driver.FindElement(By.Id("txtBenutzerkennung")).SendKeys(user);
            _driver.FindElement(By.Id("pwdPin")).SendKeys(password);
            //To submit form.
            //You can use any other Input field's(First Name, Last Name etc.) xpath too In bellow given syntax.
            _driver.FindElement(By.Id("xview-anmelden")).Click();
        }

        public void TwoFactorLogin(string tan)
        {
            //txtTan
            _driver.FindElement(By.Id("txtTan")).SendKeys(tan);
            _driver.FindElement(By.Id("xview-weiter")).Click();
        }

        public async Task SkipMessages()
        {
            try
            {
                //txtTan
                while (_driver.FindElement(By.Id("xview-weiter")) != null)
                {
                    _driver.FindElement(By.Id("xview-weiter")).Click();
                }
            }
            catch (NoSuchElementException)
            {
                //no element on this site, all fine. Method end
            }

            //wait until site was loaded
            for (int i = 0; 
                i < 4 && !_driver.FindElements(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']")).Any();
                i++)
            {
                await Task.Delay(500);
            }

            if (!_driver.FindElements(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']")).Any())
                throw new Exception("Error loading financial status page");

            _loginSuceeded = true;
        }

        public async Task<List<Account>> GetAccounts()
        {
            var accounts = new List<Account>();
            var table = _driver.FindElement(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']"));
            foreach (var row in table.FindElements(By.ClassName("v-table-row")))
            {
                var data = row.FindElements(By.ClassName("v-table-cell-wrapper"));
                var name = data[0].Text;
                var accountNr = data[1].Text;
                Regex digitsOnly = new Regex(@"[^\d\.\,\-]");
                var balance = Convert.ToDecimal(digitsOnly.Replace(data[2].Text, ""));

                var revenues = await GetRevenues(row);
                accounts.Add(new Account()
                {
                    Name = name,
                    Number = accountNr,
                    Balance = balance
                });
            }

            return accounts;
        }

        private async Task<string> GetRevenues(IWebElement accountRow)
        {
            // select the drop down list
            var education = accountRow.FindElement(By.ClassName("v-select-select"));
            //create select element object 
            var selectElement = new SelectElement(education);

            //select by value
            selectElement.SelectByValue("2");




            await SwitchToAccountOverview();

            return null; //TODO finsih
        }

        private async Task SwitchToAccountOverview()
        {
            //click Übersicht
            _driver.FindElement(By.CssSelector("li[data-nav=uebersicht]")).Click();

            //back to account overview
            //wait until site was loaded
            for (int i = 0;
                i < 4 && !_driver.FindElements(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']")).Any();
                i++)
            {
                await Task.Delay(500);
            }

            if (!_driver.FindElements(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']")).Any())
                throw new Exception("Error loading financial status page");
        }

        private void Logout()
        {
            //actAbmelden
            _driver.FindElement(By.Id("actAbmelden")).Click();
        }
        public void Dispose()
        {
            _driver?.Dispose();
            if(_loginSuceeded)
                Logout();
        }
    }
}
