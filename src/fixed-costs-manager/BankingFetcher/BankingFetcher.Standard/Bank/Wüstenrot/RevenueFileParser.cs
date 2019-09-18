using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingFetcher.Standard.Bank.Wüstenrot
{
    public class RevenueFileParser
    {
        private readonly string _fileContent;

        public RevenueFileParser(string fileContent)
        {
            _fileContent = fileContent;
            AccountHolder = GetAccountHolder();
        }

        public string AccountHolder { get; }

        private string GetAccountHolder()
        {
            string accountHolder = "";
            foreach (var row in _fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!row.Contains("Kontoinhaber:"))
                    continue;
                accountHolder = row.Split(';')[4].Trim('"');
                break;
            }

            return accountHolder;
        }
        public List<RevenueEntry> GetRevenues()
        {
            var revenues = new List<RevenueEntry>();
            var begin = false;
            foreach (var row in _fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                if (!begin)
                {
                    if (row.StartsWith("\"Buchungstag\";\"Valuta\";"))
                        begin = true;
                    continue;
                }
                //check end
                if (row == String.Empty)
                    break;

                var columns = row.Split(';').Select(x => x.Trim('"')).ToArray();

                revenues.Add(new RevenueEntry()
                {
                    BookingDay = DateTime.Parse(columns[0]),
                    Valuta = DateTime.Parse(columns[1]),
                    SoldToPartyOrPayee = columns[2],
                    RecipientOrPayer = columns[3],
                    PurposeOfUse = columns[8],
                    Revenue = Convert.ToDecimal((columns[12] == "S" ? "-" : "") + columns[11])
                });
            }

            return revenues;
        }
    }
}