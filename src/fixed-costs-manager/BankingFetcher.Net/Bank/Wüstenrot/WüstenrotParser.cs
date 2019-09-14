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
        private readonly Func<int> _getTan;
        private ChromeDriver _driver;
        private bool _loginSuceeded;

        public WüstenrotParser(Func<int> getTan)
        {
            _getTan = getTan;
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

            EnterTan();
        }

        private void EnterTan()
        {
            var tan = _getTan.Invoke();
            //txtTan
            _driver.FindElement(By.Id("txtTan")).SendKeys(tan.ToString());
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

            await WaitUntilElementIsAvailable(
                By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']"));

            _loginSuceeded = true;
        }

        public async Task<List<Account>> GetAccounts()
        {
            var accounts = new List<Account>();
            var table = _driver.FindElement(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']"));
            var rows = table.FindElements(By.ClassName("v-table-row"));
            for (var index = 0; index < rows.Count; index++)
            {
                var row = rows[index];
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

                if(index+1 < rows.Count) //navigate back if there is a next row available
                    await SwitchToAccountOverview();
            }

            return accounts;
        }

        private async Task WaitUntilElementIsAvailable(By by)
        {
            //wait until site was loaded
            for (int i = 0;
                i < 20 && !_driver.FindElements(by).Any();
                i++)
            {
                await Task.Delay(500);
            }
            if (!_driver.FindElements(by).Any())
                throw new Exception("Timeout in check element is available");
        }
        private async Task<string> GetRevenues(IWebElement accountRow)
        {
            

            var education = accountRow.FindElement(By.ClassName("v-select-select"));
            var selectElement = new SelectElement(education);
            selectElement.SelectByValue("2"); //Go to revenues

            await WaitUntilElementIsAvailable(By.Id("chcZeitraum"));

            education = _driver.FindElement(By.Id("chcZeitraum"));
            selectElement = new SelectElement(education);
            selectElement.SelectByValue("6"); //All revenues
            
            _driver.FindElement(By.Id("actSuchen")).Click(); //search

            EnterTan();

            //TODO fix submit
            //navigate to export as csv
            //class Submitlink TextButton //value Exportieren
            var test = _driver.FindElements(By.XPath("//input[@class='Submitlink TextButton']")).First(x=>x.GetAttribute("value") == "Exportieren");
                test.Submit();
            

            //click export button
            _driver.FindElement(By.Id("xview-export")).Click();

            return null; //TODO finsih
        }

        private async Task SwitchToAccountOverview()
        {
            //click Übersicht
            _driver.FindElement(By.CssSelector("li[data-nav=uebersicht]")).Click();
            
            //back to account overview
            await WaitUntilElementIsAvailable(By.XPath("//div[@class='v-scrollable v-table-body-wrapper v-table-body']"));
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
