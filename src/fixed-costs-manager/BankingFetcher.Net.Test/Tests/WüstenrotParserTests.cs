using System.Collections.Generic;
using System.Linq;
using BankingFetcher.Net.Bank;
using BankingFetcher.Net.Bank.W�stenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingFetcher.Net.Test.Tests
{
    [TestClass]
    public class W�stenrotParserTests : TestBase
    {
        [TestMethod]
        public void LoginTest()
        {
            var accounts = new List<Account>();
            using (var test = new W�stenrotParser(GetCurrentTan))
            {
                test.Login(SensitiveDataConfig.W�stenrot.Username, SensitiveDataConfig.W�stenrot.Password);
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
