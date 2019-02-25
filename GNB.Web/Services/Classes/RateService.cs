using GNB.Web.Clients;
using GNB.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Services
{
    public class RateService : IRateService
    {
        private readonly IApiClient apiClient;

        public RateService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<IEnumerable<RateModel>> GetAllRates()
        {
            string data = await apiClient.GetStringRates();
            return JsonConvert.DeserializeObject<IEnumerable<RateModel>>(data);
        }
    }
}
