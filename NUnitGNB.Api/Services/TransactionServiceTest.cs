using GNB.Api.App.Clients;
using GNB.Api.App.Models;
using GNB.Api.App.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Services
{
    [TestFixture]
    internal class TransactionServiceTest
    {
        private Mock<IHerokuAppClient> herokuAppCliente;
        private TransactionService<TransactionModel> transactionService;

        [SetUp]
        public void SetUp()
        {
            herokuAppCliente = new Mock<IHerokuAppClient>();
            transactionService = new TransactionService<TransactionModel>(herokuAppCliente.Object);
        }

        private static IEnumerable<TestCaseData> SomeTestCases {
            get {
                yield return new TestCaseData
                    (
                        @"[]",
                        new List<TransactionModel>()
                    )
                    .SetName("ParseZeroJson");
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
                    )
                    .SetName("ParseTwoJson");
            }
        }

        [TestCaseSource(typeof(TransactionServiceTest), "SomeTestCases")]
        public async Task GetTransactionsTest(string jsonString, IEnumerable<TransactionModel> expectedResult)
        {
            herokuAppCliente.Setup(setUp => setUp.GetStringTransactions()).ReturnsAsync(jsonString);
            TransactionService<TransactionModel> service = new TransactionService<TransactionModel>(herokuAppCliente.Object);
            IEnumerable<TransactionModel> result = await service.TryGetTransactions();

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
