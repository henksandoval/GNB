using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Api.Models;
using GNB.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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