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
    class TransactionServiceTest
    {
        private TransactionService<TransactionModel> TransactionService { get; set; }
        private Mock<IHerokuAppClient> MockHerokuAppCliente { get; set; }
        private Mock<IStreamUtility> MockStreamUtility { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockHerokuAppCliente = new Mock<IHerokuAppClient>();
            MockStreamUtility = new Mock<IStreamUtility>();
            TransactionService = new TransactionService<TransactionModel>(MockHerokuAppCliente.Object, MockStreamUtility.Object);
        }

        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""Transaction"":""1.04""}]", TestName = "Parse One Json")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""Transaction"":""1.04""},{""from"":""USD"",""to"":""AUD"",""Transaction"":""0.96""}]", TestName = "Parse Two Json")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""Transaction"":""1.04""},{""from"":""USD"",""to"":""AUD"",""Transaction"":""0.96""},{""from"":""AUD"",""to"":""CAD"",""Transaction"":""1.11""}]", TestName = "Parse Multiple Json")]
        public async Task GetTransactionsTest(string JSON)
        {
            MockHerokuAppCliente.Setup(setUp => setUp.GetStringTransactions()).ReturnsAsync(JSON);
            MockStreamUtility.Setup(setUp => setUp.ConvertStringToStream(JSON)).ReturnsAsync(new MemoryStream(Encoding.ASCII.GetBytes(JSON)));
            TransactionService<TransactionModel> service = new TransactionService<TransactionModel>(MockHerokuAppCliente.Object, MockStreamUtility.Object);
            IEnumerable<TransactionModel> result = await service.GetTransactions();
            Assert.IsNotNull(result);
        }
    }
}
