using BankingFetcher.Standard.Bank.Wüstenrot;
using fixed_costs_manager.Shared.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fixed_costs_manager.Server.Services
{
    public class BankFetcherJobService
    {
        private readonly string _username;
        private readonly string _password;

        public List<Account> Accounts { get; }

        private readonly DateTime _startTime;

        public BankFetcherJobService(string username, string password)
        {
            this._username = username;
            this._password = password;
            Accounts = new List<Account>();
            _startTime = DateTime.Now;
        }

        private async Task Begin()
        {
            using (var test = new WüstenrotParser(GetCurrentTan))
            {
                await test.Login(_username, _password);
                await test.SkipMessages();
                Accounts.AddRange(test.GetAccounts().Result);
            }
        }

        private async Task<string> GetCurrentTan()
        {
            throw new NotImplementedException();
        }
    }
}
