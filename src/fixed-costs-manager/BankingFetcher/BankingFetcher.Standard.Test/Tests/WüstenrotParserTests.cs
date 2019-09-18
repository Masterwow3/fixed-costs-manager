using BankingFetcher.Standard.Bank;
using BankingFetcher.Standard.Bank.W�stenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BankingFetcher.Standard.Test.Tests
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
            return "725366"; //TODO test wrong tan in revenue view
        }
    }
}
