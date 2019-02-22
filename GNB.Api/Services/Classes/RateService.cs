using GNB.Api.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class RateService<T> : IRateService<T> where T : class
    {
        private readonly IHerokuAppClient herokuAppClient;

        public RateService(IHerokuAppClient herokuAppClient)
        {
            this.herokuAppClient = herokuAppClient;
        }

        public async Task<IEnumerable<T>> TryGetRates() => await GetRates();

        public async Task<IEnumerable<T>> TryGetRates(Func<T, bool> predicate)
        {
            IEnumerable<T> data = await GetRates();
            return data.Where(predicate);
        }

        private async Task<IEnumerable<T>> GetRates() => JsonConvert.DeserializeObject<IEnumerable<T>>(await herokuAppClient.GetStringRates());
    }
}
