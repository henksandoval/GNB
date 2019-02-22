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

        private static IEnumerable<TestCaseData> SomeTestCases {
            get {
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                        {
                            new TransactionModel { Sku = "T2006", Amount = 10.00m, Currency = "USD" },
                            new TransactionModel { Sku = "M2007", Amount = 34.57m, Currency = "CAD" },
                            new TransactionModel { Sku = "R2008", Amount = 17.95m, Currency = "USD" },
                            new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                            new TransactionModel { Sku = "B2009", Amount = 21.23m, Currency = "USD" },
                        },
                    new TransactionModel
                    {
                        Sku = "T2006"
                    },
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
                ).SetName("FilterT2006");
                //yield return new TestCaseData
                //(
                //    new List<TransactionModel>
                //        {
                //            new TransactionModel { Sku = "T2007", Amount = 10.00m, Currency = "USD" },
                //            new TransactionModel { Sku = "M2007", Amount = 34.57m, Currency = "CAD" },
                //            new TransactionModel { Sku = "R2008", Amount = 17.95m, Currency = "USD" },
                //            new TransactionModel { Sku = "T2007", Amount = 7.89m, Currency = "EUR" },
                //            new TransactionModel { Sku = "B2009", Amount = 21.23m, Currency = "USD" },
                //        }, 
                //    new TransactionModel
                //        {
                //            Sku = "T2007"
                //        }, 
                //    new List<TransactionModel>
                //        {
                //            new TransactionModel { Sku = "T2007", Amount = 7.36m, Currency = "EUR" },
                //            new TransactionModel { Sku = "T2007", Amount = 7.89m, Currency = "EUR" },
                //        },
                //    15.25m
                //).SetName("FilterT2007");
            }
        }

        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService<TransactionModel>>();
            rateService = new Mock<IRateService<RateModel>>();
            transactionBusiness = new TransactionBusiness(transactionService.Object, rateService.Object);
        }

        [TestCaseSource(typeof(TransactionBusinessTest), "SomeTestCases")]
        public async Task GetTransactionsBySkuCodeFilters(IEnumerable<TransactionModel> transactionsList, TransactionModel filteringCondition, IEnumerable<TransactionModel> filteredList, IEnumerable<RateModel> rates, decimal totalPrice)
        {
            transactionService.Setup(opt => opt.TryGetTransactions(It.IsAny<Func<TransactionModel, bool>>())).ReturnsAsync(filteredList);
            rateService.Setup(opt => opt.GetRates()).ReturnsAsync(rates);
            IEnumerable<TransactionModel> result = await transactionBusiness.GetTransactionsBySkuCode(filteringCondition);

            Assert.That(filteredList.All(x => x.Sku == filteringCondition.Sku), Is.EqualTo(result.All(x => x.Sku == filteringCondition.Sku)), "List filtered is not equal to list filtered expected");
            Assert.That(result.Sum(x => x.Amount), Is.EqualTo(totalPrice), "Total Price is not equal");
            Assert.That(filteredList, Is.EqualTo(result), "All data in filteredList is not equal to resultList");
        }
    }
}
