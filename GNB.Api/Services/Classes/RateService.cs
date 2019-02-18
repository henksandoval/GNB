using GNB.Api.Clients;
using GNB.Api.Models;
using GNB.Api.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class RateService : IRateService
    {
        private readonly IHerokuAppClient HerokuAppClient;
        private readonly IStreamUtility StreamUtility;

        public RateService(IHerokuAppClient herokuAppClient, IStreamUtility streamUtility)
        {
            HerokuAppClient = herokuAppClient;
            StreamUtility = streamUtility;
        }

        public async Task<IEnumerable<RateModel>> GetRates()
        {
            string rates = await GetRatesOfClient();
            Stream stream = await StreamUtility.ConvertStringToStream(rates);
            IEnumerable<RateModel> response = await ConvertStreamToRateModel(stream);
            return response;
        }

        private Task<IEnumerable<RateModel>> ConvertStreamToRateModel(Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<RateModel>));
            return Task.FromResult(serializer.ReadObject(stream) as IEnumerable<RateModel>);
        }

        private Task<string> GetRatesOfClient() => HerokuAppClient.GetStringRates();
    }
}
