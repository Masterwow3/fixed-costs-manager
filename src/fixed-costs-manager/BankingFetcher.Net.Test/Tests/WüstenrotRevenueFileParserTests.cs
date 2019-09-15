using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BankingFetcher.Net.Bank;
using BankingFetcher.Net.Bank.Wüstenrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingFetcher.Net.Test.Tests
{
    [TestClass]
    public class WüstenrotRevenueFileParserTests : TestBase
    {
        [TestMethod]
        public void ParseTest()
        {
            var content = File.ReadAllText(Path.Combine(TestDirectory, "Tests", "Umsaetze_Test.csv"), Encoding.Default);
            var parser = new RevenueFileParser(content);
            var data = parser.GetRevenues();

            Assert.IsTrue(data.Any());
            Assert.IsFalse(String.IsNullOrWhiteSpace(parser.AccountHolder));
        }
    }
}