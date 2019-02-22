using GNB.Api.Models;
using GNB.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<TransactionModel>> GetTransactionsBySkuCode(TransactionModel transactionModel)
        {

            Task<IEnumerable<TransactionModel>> getTransactions = transactionService.TryGetTransactions(x => x.Sku == transactionModel.Sku);
            Task<IEnumerable<RateModel>> getRates = rateService.GetRates();

            await Task.WhenAll(getTransactions, getRates);

            IEnumerable<TransactionModel> transactions = await getTransactions;
            IEnumerable<RateModel> rates = await getRates;

            transactions.Where(x => x.Currency != "EUR").ToList().ForEach(transaction => {
                ConvertCurrencyInTransaction(ref transaction, rates);
            });

            return transactions.Where(x => x.Sku == transactionModel.Sku);
        }

        private void ConvertCurrencyInTransaction(ref TransactionModel transaction, IEnumerable<RateModel> rates)
        {
            transaction.Amount = GetMount(rates.SingleOrDefault(x => x.From == "EUR"), transaction);
        }

        private decimal GetMount(RateModel rate, TransactionModel transaction)
        {
            return rate.Rate * transaction.Amount;
        }
    }
}
