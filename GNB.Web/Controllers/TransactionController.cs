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

        [HttpPost]
        public async Task<IActionResult> GetTransactions(DataTableUtility dataTable)
        {
            string searchSku = dataTable.GetParameterInCustomSearchByName("sku");
            string searchCurrency = dataTable.GetParameterInCustomSearchByName("currency");

            IEnumerable<TransactionModel> transactions = await transactionRepository.TryGetAllTransactions(x => x.Sku.ToLower() == searchSku.ToLower() && x.Currency.ToLower() == searchCurrency.ToLower());

            return Ok(dataTable.GetPropertiesDataTable(transactions));
        }
    }
}