using GNB.Web.Models;
using GNB.Web.Repositories;
using GNB.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
            string searchSku = dataTable.GetParameterInCustomSearchByName("sku");
            string searchCurrency = dataTable.GetParameterInCustomSearchByName("currency");

            IEnumerable<TransactionModel> transactions = await transactionRepository.TryGetAllTransactions();

            transactions
                .WhereIf(!searchSku.IsNullOrEmpty(), x => x.Sku.ToLower() == searchSku.ToLower())
                .WhereIf(!searchCurrency.IsNullOrEmpty(), x => x.Currency.ToLower() == searchCurrency.ToLower());

            return Ok(dataTable.GetPropertiesDataTable(transactions));
        }


    }
}