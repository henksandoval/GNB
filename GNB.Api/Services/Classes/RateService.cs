using GNB.Api.Clients;
using GNB.Api.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class RateService<T> : IRateService<T> where T : class
    {
        private readonly IHerokuAppClient HerokuAppClient;
        private readonly IStreamUtility StreamUtility;

        public RateService(IHerokuAppClient herokuAppClient, IStreamUtility streamUtility)
        {
            HerokuAppClient = herokuAppClient;
            StreamUtility = streamUtility;
        }

        public async Task<IEnumerable<T>> GetRates()
        {
            string Rates = await GetRatesOfClient();
            Stream stream = await StreamUtility.ConvertStringToStream(Rates);
            return await ConvertStreamUtility<T>.ConvertStreamToModel(stream);
        }

        private async Task<string> GetRatesOfClient() => await HerokuAppClient.GetStringRates();
    }
}
