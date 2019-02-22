using GNB.Api.Business;
using GNB.Api.Models;
using GNB.Api.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GNB.Api.Tests.Business
{
    [TestFixture]
    internal class TransactionBusinessTest
    {
        private Mock<ITransactionService<TransactionModel>> transactionService;
        private Mock<IRateService<RateModel>> rateService;
        private TransactionBusiness transactionBusiness;
        private (IEnumerable<TransactionModel> TransactionsList, TransactionModel FilteringCondition, IEnumerable<TransactionModel> FilteredList, decimal TotalPrice) Validations => (
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

        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService<TransactionModel>>();
            rateService = new Mock<IRateService<RateModel>>();
            transactionBusiness = new TransactionBusiness(transactionService.Object, rateService.Object);
        }

        [TestCase(Category = "UnitTest")]
        public void GetTransactionsBySkuCode()
        {
            var (TransactionsList, FilteringCondition, FilteredList, TotalPrice) = Validations;

            transactionService.Setup(opt => opt.GetTransactions()).ReturnsAsync(TransactionsList);
            IEnumerable<TransactionModel> result = transactionBusiness.GetTransactionsBySkuCode(FilteringCondition);

            Assert.That(result.Sum(x => x.Amount), Is.EqualTo(TotalPrice));
            Assert.AreEqual(FilteredList, result);
        }


    }
}
