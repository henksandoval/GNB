using GNB.Api.Clients;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Clients
{
    [TestFixture]
    class HerokuAppClientTest
    {
        private HerokuAppClient herokuAppClient;

        [SetUp]
        public void SetUp()
        {
            herokuAppClient = new HerokuAppClient(new HttpClient
            {
                BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com")
            });
        }

        [TestCase(Category = "UnitTest")]
        public async Task GetStringRatesIsNotNull()
        {
            string rates = await herokuAppClient.GetStringRates();
            Assert.IsNotNull(rates);
        }

        [TestCase(Category = "UnitTest")]
        public async Task GetStringTransactionsIsNotNull()
        {
            string transactions = await herokuAppClient.GetStringTransactions();
            Assert.IsNotNull(transactions);
        }
    }
}
