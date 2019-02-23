using GNB.Api.Business;
using GNB.Api.Models;
using GNB.Api.Services;
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
        private Mock<IRateService<RateModel>> rateService;
        private TransactionBusiness transactionBusiness;


        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService<TransactionModel>>();
            rateService = new Mock<IRateService<RateModel>>();
            transactionBusiness = new TransactionBusiness(transactionService.Object, rateService.Object);
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
            }
        }
    }
}
