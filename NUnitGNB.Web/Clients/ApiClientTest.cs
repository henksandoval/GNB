using GNB.Web.Clients;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Web.Tests.Clients
{
    [TestFixture]
    internal class ApiClientTest
    {
        private ApiClient apiClient;

        [SetUp]
        public void SetUp()
        {
            apiClient = new ApiClient(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            });
        }

        [TestCase(Category = "UnitTest")]
        public async Task CallApiSucessfully()
        {
            string rates = await apiClient.GetStringTransactions();
            Assert.IsNotNull(rates);
        }
    }
}
