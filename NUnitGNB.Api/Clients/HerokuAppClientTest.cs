using GNB.Api.Clients;
using NUnit.Framework;
using System;
using System.IO;
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
            Stream result = await HerokuAppClient.GetStreamRates();
        }

        [Test]
        public void Test()
        {
            bool a = true;
            Assert.IsTrue(a);
        }
    }
}
