using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[assembly: InternalsVisibleTo("BankingFetcher.Net.Test")]
namespace BankingFetcher.Net.Bank.Wüstenrot
{
    class WüstenrotParser : IDisposable
    {
        private ChromeDriver _driver;

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

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
