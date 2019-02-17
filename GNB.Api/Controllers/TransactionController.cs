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
        private readonly TransactionService TransactionService;

        public TransactionController(TransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactions() => await TransactionService.GetTransactions();
    }
}