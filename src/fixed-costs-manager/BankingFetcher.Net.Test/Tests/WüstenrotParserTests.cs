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
            using (var test = new W�stenrotParser(GetCurrentTan))
            {
                test.Login(SensitiveDataConfig.W�stenrot.Username, SensitiveDataConfig.W�stenrot.Password);
                test.SkipMessages().Wait();
                var accounts = test.GetAccounts().Result;
            }
        }

        private int GetCurrentTan()
        {
            return 017510;
        }
    }
}
