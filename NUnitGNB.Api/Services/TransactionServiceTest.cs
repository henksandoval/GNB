using GNB.Api.Clients;
using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Services
{
    [TestFixture]
    class TransactionServiceTest
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

        [TestCase(@"[]", ExpectedResult = 0, TestName = "Parse Zero Json", Category = "UniTest")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""Transaction"":""1.04""}]", ExpectedResult = 1, TestName = "Parse One Json", Category = "UniTest")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""Transaction"":""1.04""},{""from"":""USD"",""to"":""AUD"",""Transaction"":""0.96""}]", ExpectedResult = 2, TestName = "Parse Two Json", Category = "UniTest")]
        [TestCase(@"[{""from"":""AUD"",""to"":""USD"",""Transaction"":""1.04""},{""from"":""USD"",""to"":""AUD"",""Transaction"":""0.96""},{""from"":""AUD"",""to"":""CAD"",""Transaction"":""1.11""}]", ExpectedResult = 3, TestName = "Parse Multiple Json", Category = "UniTest")]
        public async Task<int> GetTransactionsTest(string JSON)
        {
            herokuAppCliente.Setup(setUp => setUp.GetStringTransactions()).ReturnsAsync(JSON);
            streamUtility.Setup(setUp => setUp.ConvertStringToStream(JSON)).ReturnsAsync(new MemoryStream(Encoding.ASCII.GetBytes(JSON)));
            TransactionService<TransactionModel> service = new TransactionService<TransactionModel>(herokuAppCliente.Object, streamUtility.Object);
            IEnumerable<TransactionModel> result = await service.GetTransactions();
            return result.Count();
        }
    }
}
