using GNB.Api.Business;
using GNB.Api.Controllers;
using GNB.Api.Models;
using GNB.Api.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Controllers
{
    class TransactionControllerTest
    {
        public static IEnumerable<TestCaseData> TransactionSource()
        {
            yield return new TestCaseData(new List<TransactionModel> {
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel()
            }).SetName("ResponseFourObjects").SetCategory("UnitTest");
            yield return new TestCaseData(new List<TransactionModel> {
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel(),
                new TransactionModel()
            }).SetName("ResponseSevenObjects").SetCategory("UnitTest");
        }

        [TestCaseSource("TransactionSource", Category = "UnitTest")]
        public async Task GetAllTransactions(IEnumerable<TransactionModel> Transactions)
        {
            Mock<ITransactionBusiness> mock = new Mock<ITransactionBusiness>();
            mock.Setup(opt => opt.GetAllTransactions()).ReturnsAsync(Transactions);
            TransactionController controller = new TransactionController(mock.Object);
            IEnumerable<TransactionModel> response = await controller.GetAllTransactions();
            Assert.That(response.Count(), Is.EqualTo(Transactions.Count()));
        }
    }
}
