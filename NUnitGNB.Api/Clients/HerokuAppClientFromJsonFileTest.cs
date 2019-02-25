using GNB.Api.App.Clients;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Tests.Clients
{
    [TestFixture]
    class HerokuAppClientFromJsonFileTest
    {
        private HerokuAppClientFromJsonFile herokuAppClient;

        [SetUp]
        public void SetUp()
        {
            herokuAppClient = new HerokuAppClientFromJsonFile();
        }

        [TestCase(Category = "UnitTest")]
        public async Task GetStringRatesIsNotNull()
        {
            string rates = await herokuAppClient.GetStringRates();
            Assert.IsNotNull(rates);
        }

        [TestCase(Category = "UnitTest")]
        public async Task GetStringTransactionsIsNotNull()
        {
            string transactions = await herokuAppClient.GetStringTransactions();
            Assert.IsNotNull(transactions);
        }
    }
}
