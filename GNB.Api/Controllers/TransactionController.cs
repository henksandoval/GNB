using GNB.Api.Business;
using GNB.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionBusiness TransactionBusiness;

        public TransactionController(ITransactionBusiness transactionService)
        {
            TransactionBusiness = transactionService;
        }

        //[HttpGet]
        //public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions() =>
        //    await TransactionBusiness.GetAllTransactions();
    }
}