using System.Collections.Generic;

namespace fixed_costs_manager.Shared.BankAccount
{
    public class Account
    {
        public Account(List<RevenueEntry> revenues)
        {
            Revenues = new List<RevenueEntry>();
            Revenues.AddRange(revenues ?? new List<RevenueEntry>());
        }
        public string Name { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public List<RevenueEntry> Revenues { get; }
        public string AccountHolder { get; set; }
    }
}