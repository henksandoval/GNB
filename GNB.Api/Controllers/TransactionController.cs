using GNB.Api.Models;
using GNB.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService<TransactionModel> TransactionService;

        public TransactionController(ITransactionService<TransactionModel> transactionService)
        {
            TransactionService = transactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionModel>> GetAllTransactions() =>
            await TransactionService.TryGetTransactions();
    }
}