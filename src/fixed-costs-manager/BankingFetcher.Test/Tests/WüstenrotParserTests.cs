using BankingFetcher.Bank.Wüstenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingFetcher.Test.Tests
{
    [TestClass]
    public class WüstenrotParserTests : TestBase
    {
        [TestMethod]
        public void LoginTest()
        {
            
            var test = new WüstenrotParser(SensitiveDataConfig.Wüstenrot.Username, SensitiveDataConfig.Wüstenrot.Password);
        }
    }
}
