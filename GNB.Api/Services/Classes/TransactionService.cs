using GNB.Api.Clients;
using GNB.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class TransactionService : ITransactionService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IHerokuAppClient HerokuAppClient;

        public TransactionService(IHerokuAppClient herokuAppClient)
        {
            HerokuAppClient = herokuAppClient;
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactions()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<TransactionModel>));
            Stream streamTransactions = await HerokuAppClient.GetStreamTransactions();
            return serializer.ReadObject(streamTransactions) as IEnumerable<TransactionModel>;
        }
    }
}
