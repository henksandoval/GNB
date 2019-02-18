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
        public async Task GetStringRatesIsNotNull()
        {
            string rates = await HerokuAppClient.GetStringRates();
            Assert.IsNotNull(rates);
        }

        [Test]
        public async Task GetStringTransactionsIsNotNull()
        {
            string transactions = await HerokuAppClient.GetStringTransactions();
            Assert.IsNotNull(transactions);
        }

        [TestCase(arg1: 2, arg2: 2, arg3: 4, TestName = "2 + 2 = 4")]
        [TestCase(arg1: 2, arg2: 4, arg3: 6, TestName = "2 + 4 = 6")]
        [TestCase(arg1: 2, arg2: -1, arg3: 1, TestName = "2 + (-1) = 1")]
        public void Prueba(int a, int b, int r)
        {
            Assert.AreEqual(a + b, r);
        }
    }
}
