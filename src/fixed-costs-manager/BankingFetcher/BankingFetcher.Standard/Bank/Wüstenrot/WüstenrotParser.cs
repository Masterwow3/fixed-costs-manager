using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[assembly: InternalsVisibleTo("BankingFetcher.Standard.Test")]
namespace BankingFetcher.Standard.Bank.Wüstenrot
{
    class WüstenrotParser : IDisposable
    {
        private readonly Func<string> _getTan;
        private ChromeDriver _driver;
        private bool _loginSuceeded;
        private readonly string _downloadDirectory;

        public WüstenrotParser(Func<string> getTan)
        {
            _getTan = getTan;
            _downloadDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Downloads", Path.GetRandomFileName());
            InitializeChrome();
        }

        private void InitializeChrome()
        {
            if (Directory.Exists(_downloadDirectory))
                Directory.Delete(_downloadDirectory, true);
            Directory.CreateDirectory(_downloadDirectory);

            ChromeOptions options = new ChromeOptions();
            options.AddLocalStatePreference("profile.default_content_settings.popups", 0);
            options.AddLocalStatePreference("download.default_directory", _downloadDirectory);
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);
            options.AddUserProfilePreference("download.default_directory", _downloadDirectory);
            options.LeaveBrowserRunning = true;

            //silent start            
            options.AddArguments("headless", "disable-gpu", "start-maximized", "window-size=1920,1080");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            //start chrome
            _driver = new ChromeDriver(options);
            var param = new Dictionary<string, object>();
            param.Add("behavior", "allow");
            param.Add("downloadPath", _downloadDirectory);
            _driver.ExecuteChromeCommand("Page.setDownloadBehavior", param);
        }

        public void Login(string user, string password)
        {
            _driver.Navigate().GoToUrl("https://www.banking-wuestenrotdirect.de/banking-private/entry");
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
            var tan = _getTan.Invoke()?.Trim();
            //validate
            if (tan == null || tan.Length != 6)
                throw new Exception("The tan must consist of 6 numbers");
            Match match = Regex.Match(tan, @"^[0-9]*$");
            if (!match.Success)
                throw new Exception("The tan may consist only of numbers");

            if (!_driver.FindElements(By.Id("txtTan")).Any())
                throw new Exception("An error has occurred while entering Tan, please test the login manually at your bank.");

            //txtTan
            _driver.FindElement(By.Id("txtTan")).SendKeys(tan.ToString());
            _driver.FindElement(By.Id("xview-weiter")).Click();

            if (_driver.FindElements(By.Id("errorMessages")).Any())
                throw new Exception("Invalid tan. Please restart the process.");
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

                var revenuesData = await GetRevenues(row);
                accounts.Add(new Account(revenuesData.Revenues)
                {
                    Name = name,
                    Number = accountNr,
                    Balance = balance,
                    AccountHolder = revenuesData.AccountHolder
                });

                if (index + 1 < rows.Count) //navigate back if there is a next row available
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
        private async Task WaitUntilDownloadFileIsAvailable(string endsWith)
        {
            Func<bool> isFileAvailable = () =>
            {
                var files = Directory.GetFiles(_downloadDirectory);
                if (!files.Any(x => x.EndsWith(endsWith)))
                    return false;
                return true;
            };

            //wait until site was loaded
            for (int i = 0;
                i < 20 && !isFileAvailable.Invoke();
                i++)
            {
                await Task.Delay(500);
            }
            if (!isFileAvailable.Invoke())
                throw new Exception("Timeout reached in wait for download file");
        }
        private async Task<(List<RevenueEntry> Revenues, string AccountHolder)> GetRevenues(IWebElement accountRow)
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
            //navigate to export as csv
            var test = _driver.FindElement(By.XPath("//input[@class='Submitlink ImageButton']"));
            test.Click();

            await WaitUntilElementIsAvailable(By.Id("xview-export"));
            //click export button
            _driver.FindElement(By.Id("xview-export")).Click();
            //wait for download
            var revenues = await ReadRevenuesFile();
            return revenues;
        }

        private async Task<(List<RevenueEntry> Revenues, string AccountHolder)> ReadRevenuesFile()
        {
            await WaitUntilDownloadFileIsAvailable(".csv");
            var files = Directory.GetFiles(_downloadDirectory);
            if (!files.Any())
                return (null, null);
            var csvFilePath = files.First(x => x.ToLower().EndsWith(".csv"));

            var csv = File.ReadAllText(csvFilePath, CodePagesEncodingProvider.Instance.GetEncoding(1252));
            File.Delete(csvFilePath);

            var parser = new RevenueFileParser(csv);
            return (parser.GetRevenues(), parser.AccountHolder);
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
            if (_loginSuceeded)
                Logout();
            _driver?.Dispose();
            Directory.Delete(_downloadDirectory, true);
        }
    }
}
