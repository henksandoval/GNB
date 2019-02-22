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

        public IEnumerable<TransactionModel> Transactions { get; private set; }
        public Task<decimal> GetTotalPriceTransactions => 
            Task.FromResult(decimal.Round(Transactions.Sum(x => x.Amount), 2));

        public async Task GetTransactionsBySkuCode(TransactionModel transactionModel)
        {

            Task<IEnumerable<TransactionModel>> getTransactions = transactionService.TryGetTransactions(x => x.Sku == transactionModel.Sku);
            Task<IEnumerable<RateModel>> getRates = rateService.TryGetRates();

            await Task.WhenAll(getTransactions, getRates);

            Transactions = await getTransactions;
            IEnumerable<RateModel> rates = await getRates;

            Transactions.Where(x => x.Currency != "EUR").ToList().ForEach(transaction =>
            {
                ConvertCurrencyInTransaction(ref transaction, rates);
            });
        }

        private void ConvertCurrencyInTransaction(ref TransactionModel transaction, IEnumerable<RateModel> rates)
        {
            TransactionModel condition = transaction;

            if (rates.Any(x => x.From == condition.Currency && x.To == "EUR"))
            {
                transaction.Amount = GetMount(rates.SingleOrDefault(x => x.From == condition.Currency && x.To == "EUR"), transaction);
            }
            else
            {
                transaction.Amount = GetMount2(rates.SingleOrDefault(x => x.To == condition.Currency && x.From == "EUR"), transaction);
            }
        }

        private decimal GetMount(RateModel rate, TransactionModel transaction) => (rate.Rate * transaction.Amount);

        private decimal GetMount2(RateModel rate, TransactionModel transaction) => (transaction.Amount / rate.Rate);
    }


}
