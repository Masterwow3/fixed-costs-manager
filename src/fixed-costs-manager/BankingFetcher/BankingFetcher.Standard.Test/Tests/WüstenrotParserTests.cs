using BankingFetcher.Standard.Bank;
using BankingFetcher.Standard.Bank.Wüstenrot;
using fixed_costs_manager.Shared.BankAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingFetcher.Standard.Test.Tests
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
                test.Login(SensitiveDataConfig.Wüstenrot.Username, SensitiveDataConfig.Wüstenrot.Password).Wait();
                test.SkipMessages().Wait();
                accounts.AddRange(test.GetAccounts().Result);
            }

            Assert.IsTrue(accounts.Any());
        }

        private async Task<string> GetCurrentTan()
        {
            return "725366"; //TODO test wrong tan in revenue view
        }
    }
}
