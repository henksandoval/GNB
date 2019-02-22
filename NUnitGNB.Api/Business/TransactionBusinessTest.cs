using GNB.Api.Business;
using GNB.Api.Models;
using GNB.Api.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Business
{
    [TestFixture]
    internal class TransactionBusinessTest
    {
        private Mock<ITransactionService<TransactionModel>> transactionService;
        private Mock<IRateService<RateModel>> rateService;
        private TransactionBusiness transactionBusiness;

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
                    14.99m
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
                    14.99m
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
                    14.99m
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
                    14.99m
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
                    25.99m
                ).SetName("FilterDataAndComplexConversion");
            }
        }

        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService<TransactionModel>>();
            rateService = new Mock<IRateService<RateModel>>();
            transactionBusiness = new TransactionBusiness(transactionService.Object, rateService.Object);
        }

        [TestCaseSource(typeof(TransactionBusinessTest), "SomeTestsCases")]
        public async Task GetProcessedTransactions(IEnumerable<TransactionModel> transactions, IEnumerable<RateModel> rates, decimal totalPrice)
        {
            transactionService.Setup(opt => opt.TryGetTransactions(It.IsAny<Func<TransactionModel, bool>>())).ReturnsAsync(transactions);
            rateService.Setup(opt => opt.TryGetRates()).ReturnsAsync(rates);
            await transactionBusiness.GetTransactionsBySkuCode(new TransactionModel { Sku = string.Empty });

            Assert.That(await transactionBusiness.GetTotalPriceTransactions, Is.EqualTo(totalPrice), "Total Price is not equal");
        }
    }
}
