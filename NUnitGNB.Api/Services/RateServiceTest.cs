using GNB.Api.Clients;
using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Services
{
    [TestFixture]
    class RateServiceTest
    {
        private RateService<RateModel> RateService { get; set; }
        private Mock<IHerokuAppClient> MockHerokuAppCliente { get; set; }
        private Mock<IStreamUtility<RateModel>> MockStreamUtility { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockHerokuAppCliente = new Mock<IHerokuAppClient>();
            MockStreamUtility = new Mock<IStreamUtility<RateModel>>();
            RateService = new RateService<RateModel>(MockHerokuAppCliente.Object, MockStreamUtility.Object);
        }

        [TestCase(@"[]", ExpectedResult = 0, TestName = "Parse Zero Json")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""rate"":""1.04""}]", ExpectedResult = 1, TestName = "Parse One Json")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""rate"":""1.04""},{""from"":""USD"",""to"":""AUD"",""rate"":""0.96""}]", ExpectedResult = 2, TestName = "Parse Two Json")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""rate"":""1.04""},{""from"":""USD"",""to"":""AUD"",""rate"":""0.96""},{""from"":""AUD"",""to"":""CAD"",""rate"":""1.11""}, {""from"":""USD"",""to"":""AUD"",""rate"":""0.96""}]", ExpectedResult = 4, TestName = "Parse Multiple Json")]
        public async Task<int> GetRatesTest(string JSON)
        {
            MockHerokuAppCliente.Setup(setUp => setUp.GetStringRates()).ReturnsAsync(JSON);
            MockStreamUtility.Setup(setUp => setUp.ConvertStringToStream(JSON)).ReturnsAsync(new MemoryStream(Encoding.ASCII.GetBytes(JSON)));
            RateService<RateModel> service = new RateService<RateModel>(MockHerokuAppCliente.Object, MockStreamUtility.Object);
            IEnumerable<RateModel> result = await service.GetRates();
            return result.Count();
        }
    }
}
