using BankingFetcher.Standard.Bank.Wüstenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BankingFetcher.Standard.Test.Tests
{
    [TestClass]
    public class WüstenrotRevenueFileParserTests : TestBase
    {
        [TestMethod]
        public void ParseTest()
        {
            var content = File.ReadAllText(Path.Combine(TestDirectory, "Tests", "Umsaetze_Test.csv"), CodePagesEncodingProvider.Instance.GetEncoding(1252));
            var parser = new RevenueFileParser(content);
            var data = parser.GetRevenues();

            Assert.IsTrue(data.Any());
            Assert.IsFalse(String.IsNullOrWhiteSpace(parser.AccountHolder));
        }
    }
}