using GNB.Api.Controllers;
using GNB.Api.Models;
using GNB.Api.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Controllers
{
    [TestFixture]
    class RateControllerTest
    {
        public static IEnumerable<TestCaseData> RateSource()
        {
            yield return new TestCaseData(new List<RateModel> {
                new RateModel(),
                new RateModel(),
                new RateModel(),
                new RateModel()
            }).SetName("ResponseFourObjects").SetCategory("UnitTest");
            yield return new TestCaseData(new List<RateModel> {
                new RateModel(),
                new RateModel(),
                new RateModel(),
                new RateModel(),
                new RateModel(),
                new RateModel(),
                new RateModel()
            }).SetName("ResponseSevenObjects").SetCategory("UnitTest");
        }

        [TestCaseSource("RateSource")]
        public async Task GetAllTransactions(IEnumerable<RateModel> rates)
        {
            Mock<IRateService<RateModel>> mock = new Mock<IRateService<RateModel>>();
            mock.Setup(opt => opt.GetRates()).ReturnsAsync(rates);
            RateController controller = new RateController(mock.Object);
            IEnumerable<RateModel> response = await controller.GetAllRates();
            Assert.That(response.Count(), Is.EqualTo(rates.Count()));
        }
    }
}
