using GNB.Api.App.Models;
using GNB.Api.App.Services;
using GNB.Api.App.Utilities;
using GNB.Api.App.ViewModels;
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
        public async Task Test(TransactionViewModel transaction, IEnumerable<RateModel> rates, decimal expectedValue)
        {
            Mock<IRateService<RateModel>> service = new Mock<IRateService<RateModel>>();
            Mock<IDuplicateRatesCleaner> duplicateRatesCleaner = new Mock<IDuplicateRatesCleaner>();
            service.Setup(opt => opt.TryGetRates()).ReturnsAsync(rates);
            duplicateRatesCleaner.Setup(opt => opt.DeletingDuplicates(rates)).Returns(rates);

            ICurrencyConverter converter = new CurrencyConverter(service.Object, duplicateRatesCleaner.Object);
            TransactionViewModel result = await converter.ApplyConversion(transaction);
            Assert.That(Equals(result.AmountConverted, expectedValue), $"Value expected {expectedValue}, value result {result.AmountConverted}");
        }

        private static IEnumerable<TestCaseData> SomeTestsCases {
            get {
                yield return new TestCaseData
                (
                    new TransactionViewModel { Amount = 10.5m, Currency = "EUR", CurrencyConverted = "EUR" },
                    new List<RateModel>
                        {
                            new RateModel { From = "USD", To = "EUR", Rate = 0.736m },
                        },
                    10.5m
                ).SetName("WithOutConversion");
                yield return new TestCaseData
                (
                    new TransactionViewModel { Amount = 10.5m, Currency = "USD", CurrencyConverted = "EUR" },
                    new List<RateModel>
                        {
                            new RateModel { From = "USD", To = "EUR", Rate = 0.736m },
                        },
                    7.73m
                ).SetName("ConversionSimpleFromTo");
                yield return new TestCaseData
                (
                    new TransactionViewModel { Amount = 10.5m, Currency = "USD", CurrencyConverted = "EUR" },
                    new List<RateModel>
                        {
                            new RateModel { From = "EUR", To = "USD", Rate = 1.358m },
                        },
                    7.73m
                ).SetName("ConversionSimpleToFrom");
                yield return new TestCaseData
                (
                    new TransactionViewModel { Amount = 28.5m, Currency = "USD", CurrencyConverted = "EUR" },
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                            new RateModel { From = "AUD", To = "USD", Rate = 0.92m },
                            new RateModel { From = "CAD", To = "EUR", Rate = 0.65m },
                        },
                    11.28m
                ).SetName("ConversionCascadeFrom");
                yield return new TestCaseData
                (
                    new TransactionViewModel { Amount = 40.6m, Currency = "USD", CurrencyConverted = "EUR" },
                    new List<RateModel>
                        {
                            new RateModel { From = "CAD", To = "AUD", Rate = 1.79m },
                            new RateModel { From = "AUD", To = "USD", Rate = 0.92m },
                            new RateModel { From = "CAD", To = "EUR", Rate = 0.65m },
                        },
                    16.02m
                ).SetName("ConversionCascadeTo");
            }
        }
    }
}
