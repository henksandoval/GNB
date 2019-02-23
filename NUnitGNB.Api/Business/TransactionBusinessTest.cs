using GNB.Api.Business;
using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using GNB.Api.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Business
{
    [TestFixture]
    internal class TransactionBusinessTest
    {
        private Mock<ITransactionService<TransactionModel>> transactionService;
        private Mock<ICurrencyConverter> currencyConverter;
        private TransactionBusiness transactionBusiness;


        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService<TransactionModel>>();
            currencyConverter = new Mock<ICurrencyConverter>();
            transactionBusiness = new TransactionBusiness(transactionService.Object, currencyConverter.Object);
        }

        [TestCaseSource(typeof(TransactionBusinessTest), "SomeTestsCases")]
        public async Task GetProcessedTransactions(IEnumerable<TransactionModel> models, IEnumerable<RateModel> rates, IEnumerable<TransactionViewModel> expectedResult)
        {
            transactionService.Setup(opt => opt.TryGetTransactions()).ReturnsAsync(models);
            rateService.Setup(opt => opt.TryGetRates()).ReturnsAsync(rates);

            IEnumerable<TransactionViewModel> result = await transactionBusiness.GetAllTransactions();

            Assert.That(result, Is.EqualTo(expectedResult), "ViewModel result not is equal expected value");
        }

        private static IEnumerable<TestCaseData> SomeTestsCases {
            get {
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                            new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "EUR", To = "USD", Rate = 1.359m }
                        },
                    new List<TransactionViewModel>
                        {
                            new TransactionViewModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR", AmountConverted = 7.36m, CurrencyConverted = "EUR" },
                            new TransactionViewModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR", AmountConverted = 7.63m, CurrencyConverted = "EUR"},
                        }
                ).SetName("WithOutConversion");
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2006", Amount = 10.0m, Currency = "USD" },
                            new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "USD", To = "EUR", Rate = 0.736m }
                        },
                    new List<TransactionViewModel>
                        {
                            new TransactionViewModel { Sku = "T2006", Amount = 10.0m, Currency = "USD", AmountConverted = 7.36m, CurrencyConverted = "EUR" },
                            new TransactionViewModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR", AmountConverted = 7.63m, CurrencyConverted = "EUR"},
                        }
                ).SetName("SimpleConversion");
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2006", Amount = 10.0m, Currency = "USD" },
                            new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "EUR", To = "USD", Rate = 1.359m}
                        },
                    new List<TransactionViewModel>
                        {
                            new TransactionViewModel { Sku = "T2006", Amount = 10.0m, Currency = "USD", AmountConverted = 7.36m, CurrencyConverted = "EUR" },
                            new TransactionViewModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR", AmountConverted = 7.63m, CurrencyConverted = "EUR"},
                        }
                ).SetName("InvertedConversion");
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2006", Amount = 10.0m, Currency = "USD" },
                            new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "EUR", To = "USD", Rate = 1.359m},
                            new RateModel { From = "USD", To = "EUR", Rate = 0.736m }
                        },
                    new List<TransactionViewModel>
                        {
                            new TransactionViewModel { Sku = "T2006", Amount = 10.0m, Currency = "USD", AmountConverted = 7.36m, CurrencyConverted = "EUR" },
                            new TransactionViewModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR", AmountConverted = 7.63m, CurrencyConverted = "EUR"},
                        }
                ).SetName("InvertedConversionMultipleRates");
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2007", Amount = 12.32m, Currency = "CAD" },
                            new TransactionModel { Sku = "T2007", Amount = 14.6m, Currency = "USD" },
                            new TransactionModel { Sku = "T2007", Amount = 6.23m, Currency = "EUR" },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "EUR", To = "USD", Rate = 1.359m },
                            new RateModel { From = "CAD", To = "EUR", Rate = 0.732m },
                            new RateModel { From = "USD", To = "EUR", Rate = 0.736m },
                            new RateModel { From = "EUR", To = "CAD", Rate = 1.366m },
                        },
                    new List<TransactionViewModel>
                        {
                            new TransactionViewModel { Sku = "T2007", Amount = 12.32m, Currency = "CAD", AmountConverted = 9.02m, CurrencyConverted = "EUR" },
                            new TransactionViewModel { Sku = "T2007", Amount = 14.6m, Currency = "USD", AmountConverted = 10.75m, CurrencyConverted = "EUR"},
                            new TransactionViewModel { Sku = "T2007", Amount = 6.23m, Currency = "EUR", AmountConverted = 6.23m, CurrencyConverted = "EUR"},
                        }
                ).SetName("FilterDataAndComplexConversion");
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2007", Amount = 28.5m, Currency = "USD" },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                            new RateModel { From = "CAD", To = "AUD", Rate = 1.79m },
                            new RateModel { From = "AUD", To = "USD", Rate = 0.92m },
                            new RateModel { From = "USD", To = "AUD", Rate = 1.09m },
                            new RateModel { From = "CAD", To = "EUR", Rate = 0.65m },
                            new RateModel { From = "EUR", To = "CAD", Rate = 1.54m },
                        },
                    new List<TransactionViewModel>
                        {
                            new TransactionViewModel { Sku = "T2007", Amount = 28.5m, Currency = "USD", AmountConverted = 11.31m, CurrencyConverted = "EUR" },
                        }
                ).SetName("CascadeComplexConversion");
            }
        }
    }
}
