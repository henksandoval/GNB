using GNB.Web.Clients;
using GNB.Web.Models;
using GNB.Web.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Tests.Services
{
    [TestFixture]
    internal class TransactionServiceTest
    {
        private Mock<IApiClient> apiClient;
        private TransactionService transactionService;

        [SetUp]
        public void SetUp()
        {
            apiClient = new Mock<IApiClient>();
            transactionService = new TransactionService(apiClient.Object);
        }

        [TestCaseSource(typeof(TransactionServiceTest), "SomeTestCases")]
        public async Task GetAllTransactionsTest(string jsonString, IEnumerable<TransactionModel> expectedResult)
        {
            apiClient.Setup(opt => opt.GetStringTransactions()).ReturnsAsync(jsonString);
            IEnumerable<TransactionModel> result = await transactionService.GetAllTransactions();
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static IEnumerable<TestCaseData> SomeTestCases {
            get {
                yield return new TestCaseData
                    (
                        @"[{""sku"":""W6040"",""amount"":""31.2"",""currency"":""USD""}]",
                        new List<TransactionModel>
                            {
                                new TransactionModel { Sku = "W6040", Amount = 31.2m, Currency = "USD" }
                            }
                    )
                    .SetName("ParseOneJson");
                yield return new TestCaseData
                    (
                        @"[{""sku"":""R2008"",""amount"":""17.95"",""currency"":""USD""}, {""sku"":""M2007"",""amount"":""34.57"",""currency"":""CAD""}]", 
                        new List<TransactionModel>
                            {
                                new TransactionModel { Sku = "R2008", Amount = 17.95M, Currency = "USD" },
                                new TransactionModel { Sku = "M2007", Amount = 34.57m, Currency = "CAD" },
                            }
                    ).SetName("ParseTwoJson");
            }
        }
    }
}
