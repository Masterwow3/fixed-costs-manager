using BankingFetcher.Bank.W�stenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingFetcher.Test.Tests
{
    [TestClass]
    public class W�stenrotParserTests : TestBase
    {
        [TestMethod]
        public void LoginTest()
        {
            
            var test = new W�stenrotParser(SensitiveDataConfig.W�stenrot.Username, SensitiveDataConfig.W�stenrot.Password);
        }
    }
}
