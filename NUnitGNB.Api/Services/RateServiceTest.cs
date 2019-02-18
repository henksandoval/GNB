using GNB.Api.Clients;
using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Services
{
    [TestFixture]
    public class RateServiceTest
    {
        private const string JSON = "[{'from':'AUD','to':'EUR','rate':1.5}]";

        private RateService RateService { get; set; }
        private Mock<IHerokuAppClient> MockHerokuAppCliente { get; set; }
        private Mock<IStreamUtility> MockStreamUtility { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockHerokuAppCliente = new Mock<IHerokuAppClient>();
            MockStreamUtility = new Mock<IStreamUtility>();
            RateService = new RateService(MockHerokuAppCliente.Object, MockStreamUtility.Object);
        }

        //[Test]
        public async Task GetRatesOfClientTest()
        {
            MockHerokuAppCliente.Setup(setUp => setUp.GetStringRates()).Returns(Task.FromResult(JSON));
            IEnumerable<RateModel> result = await RateService.GetRates();

        }
    }
}
