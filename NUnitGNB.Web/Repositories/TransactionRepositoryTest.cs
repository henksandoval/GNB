using GNB.Web.Models;
using GNB.Web.Repositories;
using GNB.Web.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Tests.Repositories
{
    [TestFixture]
    internal class TransactionRepositoryTest
    {
        private Mock<ITransactionService> transactionService;
        private TransactionRepository transactionRepositoryTest;

        [SetUp]
        public void SetUp()
        {
            transactionService = new Mock<ITransactionService>();
            transactionRepositoryTest = new TransactionRepository(transactionService.Object);
        }

        [TestCaseSource(typeof(TransactionRepositoryTest), "SomeTestsCases")]
        public async Task TryGetAllTransactionsTest(IEnumerable<TransactionModel> expectedResult)
        {
            transactionService.Setup(opt => opt.GetAllTransactions(It.IsAny<Func<TransactionModel, bool>>())).ReturnsAsync(expectedResult);
            IEnumerable<TransactionModel> result = await transactionRepositoryTest.TryGetAllTransactions();
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static IEnumerable<TestCaseData> SomeTestsCases {
            get {
                yield return new TestCaseData
                (
                    new List<TransactionModel>
                    {
                        new TransactionModel { Amount = 83.4m, Currency = "USD", Sku = "TD2006" },
                        new TransactionModel { Amount = 13.4m, Currency = "USD", Sku = "TD2007" },
                        new TransactionModel { Amount = 23.1m, Currency = "USD", Sku = "TD2706" },
                        new TransactionModel { Amount = 13.4m, Currency = "USD", Sku = "TD2046" },
                        new TransactionModel { Amount = 53.4m, Currency = "USD", Sku = "TD2306" }
                    }
                )
                .SetName("TestViewIndex");
            }
        }
    }
}
