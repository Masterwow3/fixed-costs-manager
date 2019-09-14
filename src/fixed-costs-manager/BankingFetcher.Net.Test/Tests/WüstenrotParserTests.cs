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
            using (var test = new WüstenrotParser(GetCurrentTan))
            {
                test.Login(SensitiveDataConfig.Wüstenrot.Username, SensitiveDataConfig.Wüstenrot.Password);
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
