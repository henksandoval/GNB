using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using GNB.Api.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Utilities
{
    [TestFixture]
    internal class CurrencyConverterTest
    {

        [TestCaseSource(typeof(CurrencyConverterTest), "SomeTestsCases")]
        public async Task Test(CurrencyViewModel currency, IEnumerable<RateModel> rates, decimal expectedValue)
        {
            Mock<IRateService<RateModel>> service = new Mock<IRateService<RateModel>>();
            service.Setup(opt => opt.TryGetRates()).ReturnsAsync(rates);

            CurrencyConverter converter = new CurrencyConverter(service.Object);
            await converter.ApplyConversion(currency, "EUR");
            Assert.That(Is.Equals(converter.AmountConverted, expectedValue));
        }

        private static IEnumerable<TestCaseData> SomeTestsCases {
            get {
                yield return new TestCaseData
                (
                    new CurrencyViewModel { Amount = 10.5m, Name = "USD" },
                    new List<RateModel>
                        {
                            new RateModel { From = "USD", To = "EUR", Rate = 0.736m },
                        },
                    7.728m
                ).SetName("ConversionSimple");
                yield return new TestCaseData
                (
                    new CurrencyViewModel { Amount = 28.5m, Name = "USD" },
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                            new RateModel { From = "CAD", To = "AUD", Rate = 1.79m },
                            new RateModel { From = "AUD", To = "USD", Rate = 0.92m },
                            new RateModel { From = "USD", To = "AUD", Rate = 1.09m },
                            new RateModel { From = "CAD", To = "EUR", Rate = 0.65m },
                            new RateModel { From = "EUR", To = "CAD", Rate = 1.54m },
                        },
                    11.30766m
                ).SetName("ConversionCascade");
            }
        }
    }
}
