using GNB.Api.Clients;
using GNB.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
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
            MemoryStream streamTransactions = new MemoryStream(Encoding.ASCII.GetBytes(await HerokuAppClient.GetStringTransactions()));
            return serializer.ReadObject(streamTransactions) as IEnumerable<TransactionModel>;
        }
    }
}
