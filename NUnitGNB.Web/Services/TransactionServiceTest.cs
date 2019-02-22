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

        [Test]
        public async Task g()
        {
            IEnumerable<TransactionModel> result = await transactionService.GetAllTransactions();
        }

        private static IEnumerable<TestCaseData> SomeTestsCases {
            get {
                yield return new TestCaseData
                (
                    
                )
                .SetName("TestViewIndex");
            }
        }
    }
}
