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
        private Mock<IHerokuAppClient> herokuAppCliente;
        private Mock<IStreamUtility<RateModel>> streamUtility;
        private RateService<RateModel> rateService;

        [SetUp]
        public void SetUp()
        {
            herokuAppCliente = new Mock<IHerokuAppClient>();
            streamUtility = new Mock<IStreamUtility<RateModel>>();
            rateService = new RateService<RateModel>(herokuAppCliente.Object, streamUtility.Object);
        }

        [TestCase(@"[]", ExpectedResult = 0, TestName = "Parse Zero Json", Category = "UnitTest")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""rate"":""1.04""}]", ExpectedResult = 1, TestName = "Parse One Json", Category = "UnitTest")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""rate"":""1.04""},{""from"":""USD"",""to"":""AUD"",""rate"":""0.96""}]", ExpectedResult = 2, TestName = "Parse Two Json", Category = "UnitTest")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""rate"":""1.04""},{""from"":""USD"",""to"":""AUD"",""rate"":""0.96""},{""from"":""AUD"",""to"":""CAD"",""rate"":""1.11""}, {""from"":""USD"",""to"":""AUD"",""rate"":""0.96""}]", ExpectedResult = 4, TestName = "Parse Multiple Json", Category = "UnitTest")]
        public async Task<int> GetRatesTest(string JSON)
        {
            herokuAppCliente.Setup(setUp => setUp.GetStringRates()).ReturnsAsync(JSON);
            streamUtility.Setup(setUp => setUp.ConvertStringToStream(JSON)).ReturnsAsync(new MemoryStream(Encoding.ASCII.GetBytes(JSON)));
            RateService<RateModel> service = new RateService<RateModel>(herokuAppCliente.Object, streamUtility.Object);
            IEnumerable<RateModel> result = await service.TryGetRates();
            return result.Count();
        }
    }
}
