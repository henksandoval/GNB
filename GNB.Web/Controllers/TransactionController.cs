using GNB.Web.Models;
using GNB.Web.Repositories;
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
        public async Task<IActionResult> Index()
        {
            IEnumerable<TransactionModel> transactions = await transactionRepository.GetAllTransactions();

            return View("Index", transactions);
        }
    }
}