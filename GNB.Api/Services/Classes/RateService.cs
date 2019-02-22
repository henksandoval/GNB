using GNB.Api.Clients;
using GNB.Api.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class RateService<T> : IRateService<T> where T : class
    {
        private readonly IHerokuAppClient herokuAppClient;
        private readonly IStreamUtility<T> streamUtility;

        public RateService(IHerokuAppClient herokuAppClient, IStreamUtility<T> streamUtility)
        {
            this.herokuAppClient = herokuAppClient;
            this.streamUtility = streamUtility;
        }

        public async Task<IEnumerable<T>> TryGetRates() => await GetRates();

        public async Task<IEnumerable<T>> TryGetRates(Func<T, bool> predicate)
        {
            IEnumerable<T> data = await GetRates();
            return data.Where(predicate);
        }

        private async Task<IEnumerable<T>> GetRates()
        {
            string Rates = await GetRatesOfClient();
            Stream stream = await streamUtility.ConvertStringToStream(Rates);
            return await StreamUtility<T>.ConvertStreamToModel(stream);
        }

        private async Task<string> GetRatesOfClient() => await herokuAppClient.GetStringRates();
    }
}
