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
            using (var test = new W�stenrotParser())
            {
                test.Login(SensitiveDataConfig.W�stenrot.Username, SensitiveDataConfig.W�stenrot.Password);
                test.TwoFactorLogin("791628");
                test.SkipMessages().Wait();
                var accounts = test.GetAccounts().Result;
            }
        }
    }
}
