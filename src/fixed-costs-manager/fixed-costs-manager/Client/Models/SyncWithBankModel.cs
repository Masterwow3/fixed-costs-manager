using fixed_costs_manager.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace fixed_costs_manager.Client.Models
{
    public class SyncWithBankModel
    {
        private HttpClient _http;
        public SyncWithBankModel(HttpClient http)
        {
            _http = http;
        }
        public async Task<WeatherForecast[]> RetrieveForecastsAsync()
        {
            return await _http.GetJsonAsync<WeatherForecast[]>("WeatherForecast");
        }
    }
}
