using GNB.Web.Controllers;
using GNB.Web.Models;
using GNB.Web.Repositories;
using GNB.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Tests
{
    [TestFixture]
    internal class TransactionControllerTest
    {
        private Mock<ITransactionRepository> transactionRespository;
        private TransactionController controller;

        [SetUp]
        public void SetUp()
        {
            transactionRespository = new Mock<ITransactionRepository>();
            controller = new TransactionController(transactionRespository.Object);
        }

        [Test]
        public async Task Index()
        {
            List<TransactionModel> expectedResult = new List<TransactionModel>
            {
                new TransactionModel { Amount = 83.4m, Currency = "USD", Sku = "TD2006" },
                new TransactionModel { Amount = 13.4m, Currency = "USD", Sku = "TD2007" },
                new TransactionModel { Amount = 23.1m, Currency = "USD", Sku = "TD2706" },
                new TransactionModel { Amount = 13.4m, Currency = "USD", Sku = "TD2046" },
                new TransactionModel { Amount = 53.4m, Currency = "USD", Sku = "TD2306" }
            };

            transactionRespository.Setup(opt => opt.GetAllTransactions()).ReturnsAsync(expectedResult);
            Task<IActionResult> taskResult = controller.Index();

            ViewResult viewResult = await taskResult as ViewResult;
            IEnumerable<TransactionModel> modelData = viewResult.Model as IEnumerable<TransactionModel>;

            Assert.IsInstanceOf(typeof(ViewResult), viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.That(modelData, Is.EqualTo(expectedResult));
        }
    }
}
