using System.Collections.Generic;
using System.Linq;
using BankingFetcher.Net.Bank;
using BankingFetcher.Net.Bank.Wüstenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingFetcher.Net.Test.Tests
{
    [TestClass]
    public class WüstenrotParserTests : TestBase
    {
        [TestMethod]
        public void LoginTest()
        {
            var accounts = new List<Account>();
            using (var test = new WüstenrotParser(GetCurrentTan))
            {
                test.Login(SensitiveDataConfig.Wüstenrot.Username, SensitiveDataConfig.Wüstenrot.Password);
                test.SkipMessages().Wait();
                accounts.AddRange(test.GetAccounts().Result);
            }

            Assert.IsTrue(accounts.Any());
        }

        private string GetCurrentTan()
        {
            return "232655"; //TODO handle wrong tan
        }
    }
}
