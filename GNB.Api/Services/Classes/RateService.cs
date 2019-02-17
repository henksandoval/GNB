using GNB.Api.Clients;
using GNB.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class RateService : IRateService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IHerokuAppClient HerokuAppClient;

        public RateService(IHerokuAppClient herokuAppClient)
        {
            HerokuAppClient = herokuAppClient;
        }

        public async Task<IEnumerable<RateModel>> GetRates()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<RateModel>));
            Stream streamRates = await HerokuAppClient.GetStreamRates();
            return serializer.ReadObject(streamRates) as IEnumerable<RateModel>;
        }
    }
}
