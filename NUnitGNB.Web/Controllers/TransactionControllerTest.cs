using GNB.Web.Controllers;
using GNB.Web.Models;
using GNB.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Tests.Controllers
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

        [TestCaseSource(typeof(TransactionControllerTest), "SomeTestsCases")]
        public async Task Index(IEnumerable<TransactionModel> expectedResult)
        {
            transactionRespository.Setup(opt => opt.TryGetAllTransactions(It.IsAny<Func<TransactionModel, bool>>())).ReturnsAsync(expectedResult);
            Task<IActionResult> taskResult = controller.Index();

            ViewResult viewResult = await taskResult as ViewResult;
            IEnumerable<TransactionModel> modelData = viewResult.Model as IEnumerable<TransactionModel>;

            Assert.IsInstanceOf(typeof(ViewResult), viewResult, "Is not instance of type ViewResult");
            Assert.AreEqual("Index", viewResult.ViewName, "Return view is not Index");
            Assert.That(modelData, Is.EqualTo(expectedResult), "Object model not is expectedResult");
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
