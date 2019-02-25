using GNB.Web.Models;
using GNB.Web.Repositories;
using GNB.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        [HttpGet]
        public IActionResult Index() => View("Index");

        [HttpGet]
        public IActionResult TransactionsNewCurrency() => View("Converter");

        [HttpPost]
        public async Task<IActionResult> GetTransactions(DataTableUtility dataTable)
        {
            IEnumerable<TransactionModel> transactions = await transactionRepository.TryGetAllTransactions();
            return Ok(dataTable.GetPropertiesDataTable(transactions));
        }


        [HttpPost]
        public async Task<IActionResult> GetTransactionsNewCurrency(DataTableUtility dataTable)
        {
            string searchSku = dataTable.GetParameterInCustomSearchByName("sku");

            IEnumerable<TransactionModel> transactions = await transactionRepository.TryGetAllTransactions(new TransactionModel { Sku = searchSku, Currency = "EUR" });
            IEnumerable<TransactionModel> returnData = transactions
                .WhereIf(!searchSku.IsNullOrEmpty(), x => x.Sku.ToLower() == searchSku.ToLower());

            return Ok(dataTable.GetPropertiesDataTable(returnData));
        }
    }
}