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
            using (var test = new WüstenrotParser())
            {
                test.Login(SensitiveDataConfig.Wüstenrot.Username, SensitiveDataConfig.Wüstenrot.Password);
                test.TwoFactorLogin("791628");
                test.SkipMessages().Wait();
                var accounts = test.GetAccounts().Result;
            }
        }
    }
}
