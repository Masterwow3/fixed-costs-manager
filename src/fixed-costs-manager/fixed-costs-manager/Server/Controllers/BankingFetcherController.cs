using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingFetcher.Standard.Bank.Wüstenrot;
using fixed_costs_manager.Server.Services;
using fixed_costs_manager.Shared.BankAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fixed_costs_manager.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankingFetcherController : ControllerBase
    {
        private Dictionary<string, BankFetcherJobService> _fetchJobs;

        [HttpPost("GetWWAccountData")]
        public string GetWWAccountData([FromBody]string username, string password)
        {
            var jobId = Guid.NewGuid().ToString();
            _fetchJobs.Add(jobId, new BankFetcherJobService(username,password));
            return jobId;
        }
        [HttpPost("GetJobActions")]
        public void GetJobActions([FromBody]string jobId)
        {
            //TODO implement
        }

    }
}