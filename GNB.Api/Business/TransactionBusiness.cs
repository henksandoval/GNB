using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Api.Models;
using GNB.Api.Services;

namespace GNB.Api.Business
{
    public class TransactionBusiness
    {
        private readonly ITransactionService<TransactionModel> transactionService;
        private readonly IRateService<RateModel> rateService;

        public TransactionBusiness(ITransactionService<TransactionModel> transactionService, IRateService<RateModel> rateService)
        {
            this.transactionService = transactionService;
            this.rateService = rateService;
        }

        public IEnumerable<TransactionModel> GetTransactionsBySkuCode(TransactionModel transactionModel) => new List<TransactionModel> {
                new TransactionModel { Sku = "T2006", Amount = 7.36m, Currency = "EUR" },
                new TransactionModel { Sku = "T2006", Amount = 7.63m, Currency = "EUR" },
            };
    }
}
