using GNB.Api.Clients;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Clients
{
    [TestFixture]
    internal class HerokuAppClientTest
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


        [TestCase(Category = "UnitTest")]
        public void ThrowOnBadRequest()
        {
            HerokuAppClient request = new HerokuAppClient(new HttpClient
            {
                BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/badRequest")
            });

            Exception exception = Assert.ThrowsAsync<Exception>(async () => await request.GetStringTransactions());
            //Assert.That(exception..StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Tests()
        {
            // Using a method as a delegate
            var data = Assert.ThrowsAsync<ArgumentException>(async () => await MethodThatThrows());
        }

        async Task MethodThatThrows()
        {
            await Task.Delay(100);
            throw new ArgumentException();
        }
    }
}
