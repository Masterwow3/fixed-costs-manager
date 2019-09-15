﻿using System;

namespace BankingFetcher.Net.Bank
{
    public class RevenueEntry
    {
        public DateTime BookingDay { get; set; }
        public DateTime Valuta { get; set; }
        public string SoldToPartyOrPayee { get; set; }
        public RevenueRole AccountHolderRole => Revenue > 0 ? RevenueRole.Recipient : RevenueRole.Sender;
        public string RecipientOrPayer { get; set; }
        public string PurposeOfUse { get; set; }
        public decimal Revenue { get; set; }

    }
}