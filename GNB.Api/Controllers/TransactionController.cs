using GNB.Api.App.Business;
using GNB.Api.App.Models;
using GNB.Api.App.ViewModels;
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

        [HttpGet]
        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions() =>
            await TransactionBusiness.GetAllTransactions();

        [HttpPost]
        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions(TransactionViewModel model)
        {
            if (model.Sku == null)
            {
                return await TransactionBusiness.GetAllTransactions();
            }
            return await TransactionBusiness.GetAllTransactions(model);
        }
    }
}