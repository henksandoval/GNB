using GNB.Api.Business;
using GNB.Api.Models;
using GNB.Api.Services;
using Moq;
using NUnit.Framework;
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

        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService<TransactionModel>>();
            rateService = new Mock<IRateService<RateModel>>();
            transactionBusiness = new TransactionBusiness(transactionService.Object, rateService.Object);
        }

        public static IEnumerable<TestCaseData> TransactionSource()
        {
            yield return new TestCaseData().SetName("GetResultBySkuCode").SetCategory("UnitTest");
        }

        [TestCase(Category = "UnitTest")]
        public void GetTransactionsBySkuCode()
        {
            (IEnumerable<TransactionModel> TransactionsList, TransactionModel FilteringCondition, IEnumerable<TransactionModel> FilteredList, decimal TotalPrice) rules = 
            (
                TransactionsList: new List<TransactionModel>
                    {
                        new TransactionModel { Sku = "T2006", Amount = 10.00m, Currency = "USD" },
                        new TransactionModel { Sku = "M2007", Amount = 34.57m, Currency = "CAD" },
                        new TransactionModel { Sku = "R2008", Amount = 17.95m, Currency = "USD" },
                        new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                        new TransactionModel { Sku = "B2009", Amount = 21.23m, Currency = "USD" },
                    },
                FilteringCondition: new TransactionModel { Sku = "T2006" },
                FilteredList: new List<TransactionModel>
                    {
                        new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                        new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
                    },
                TotalPrice: 14.99m
            );


            IEnumerable<TransactionModel> expectedResult = new List<TransactionModel> {
                new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
            };

            IEnumerable<TransactionModel> re = new List<TransactionModel> {
                new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
            };

            var d = expectedResult;

            transactionService.Setup(opt => opt.GetTransactions()).ReturnsAsync(rules.TransactionsList);
            IEnumerable<TransactionModel> result = transactionBusiness.GetTransactionsBySkuCode(rules.FilteringCondition);
            var sum = result.Sum(x => x.Amount);
            Assert.That(sum, Is.EqualTo(rules.TotalPrice));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Test()
        {
            IEnumerable<TransactionModel> expectedResult = new List<TransactionModel> {
                new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
            };

            IEnumerable<TransactionModel> result = new List<TransactionModel> {
                new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
            };

            var result2 = expectedResult;

            Assert.AreEqual(expectedResult.ToList(), result.ToList());
        }


        [Test]
        public void Test3()
        {
            var result = new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" };

            var expectedResult = new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" };

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Test2()
        {
            IEnumerable<string> expectedResult = new List<string> {
                "HENK",
                "ALEXANDER",
                "SANDOVAL",
            };

            IEnumerable<string> result = new List<string> {
                "HENK",
                "ALEXANDER",
                "SANDOVAL",
            };

            var result2 = expectedResult;

            Assert.AreNotSame(expectedResult, result);
        }
    }
}
