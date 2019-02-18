using GNB.Api.Clients;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Clients
{
    [TestFixture]
    public class HerokuAppClientTest
    {
        private HerokuAppClient HerokuAppClient { get; set; }

        [SetUp]
        public void SetUp()
        {
            HerokuAppClient = new HerokuAppClient(new HttpClient
            {
                BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com")
            });
        }

        [Test]
        public async Task GetRates()
        {
            string rates = await HerokuAppClient.GetStringRates();
            Assert.IsNotNull(rates);
        }

        [Test]
        public async Task GetTransactions()
        {
            string transactions = await HerokuAppClient.GetStringTransactions();
            Assert.IsNotNull(transactions);
        }
    }
}
