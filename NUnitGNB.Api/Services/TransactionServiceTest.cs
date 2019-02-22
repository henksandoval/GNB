using GNB.Api.Clients;
using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Services
{
    [TestFixture]
    internal class TransactionServiceTest
    {
        private Mock<IHerokuAppClient> herokuAppCliente;
        private Mock<IStreamUtility<TransactionModel>> streamUtility;
        private TransactionService<TransactionModel> transactionService;

        [SetUp]
        public void SetUp()
        {
            herokuAppCliente = new Mock<IHerokuAppClient>();
            streamUtility = new Mock<IStreamUtility<TransactionModel>>();
            transactionService = new TransactionService<TransactionModel>(herokuAppCliente.Object, streamUtility.Object);
        }

        private static IEnumerable<TestCaseData> SomeTestCases {
            get {
                yield return new TestCaseData(@"[{""sku"":""W6040"",""amount"":""31.2"",""currency"":""USD""}]", new List<TransactionModel> { new TransactionModel { Sku = "W6040", Amount = 31.2m, Currency = "USD" } }).SetName("ParseOneJson");
                yield return new TestCaseData(@"[{""sku"":""R2008"",""amount"":""17.95"",""currency"":""USD""}, {""sku"":""M2007"",""amount"":""34.57"",""currency"":""CAD""}]", new List<TransactionModel>
                {
                    new TransactionModel { Sku = "R2008", Amount = 17.95M, Currency = "USD" },
                    new TransactionModel { Sku = "M2007", Amount = 34.57m, Currency = "CAD" },
                }).SetName("ParseTwoJson");
            }
        }

        [TestCaseSource(typeof(TransactionServiceTest), "SomeTestCases")]
        public async Task SomeMethod_Always_DoesSomethingWithParameters(string jsonString, IEnumerable<TransactionModel> expectedResult)
        {
            herokuAppCliente.Setup(setUp => setUp.GetStringTransactions()).ReturnsAsync(jsonString);
            streamUtility.Setup(setUp => setUp.ConvertStringToStream(jsonString)).ReturnsAsync(new MemoryStream(Encoding.ASCII.GetBytes(jsonString)));
            TransactionService<TransactionModel> service = new TransactionService<TransactionModel>(herokuAppCliente.Object, streamUtility.Object);
            IEnumerable<TransactionModel> result = await service.GetTransactions();

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
