using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Business
{
    public class TransactionBusiness : ITransactionBusiness
    {
        private readonly ITransactionService<TransactionModel> transactionService;
        private readonly IRateService<RateModel> rateService;
        private IEnumerable<TransactionModel> Transactions;
        private IEnumerable<RateModel> Rates;
        private const string CurrencyConversion = "EUR";

        public TransactionBusiness(ITransactionService<TransactionModel> transactionService, IRateService<RateModel> rateService)
        {
            this.transactionService = transactionService;
            this.rateService = rateService;
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions()
        {
            Task<IEnumerable<TransactionModel>> transactions = transactionService.TryGetTransactions();
            Task<IEnumerable<RateModel>> rates = rateService.TryGetRates();

            await Task.WhenAll(transactions, rates);

            Transactions = await transactions;
            Rates = await rates;

            IEnumerable<TransactionViewModel> listTransactions = Transactions.Select(model => SetCurrencyConversion(model));

            return listTransactions;

        }

        public Task<decimal> GetTotalPriceTransactions => Task.FromResult(decimal.Round(Transactions.Sum(x => x.Amount), 2));

        public async Task GetTransactionsBySkuCode(TransactionModel transactionModel)
        {

            Task<IEnumerable<TransactionModel>> getTransactions = transactionService.TryGetTransactions(x => x.Sku == transactionModel.Sku);
            Task<IEnumerable<RateModel>> getRates = rateService.TryGetRates();

            await Task.WhenAll(getTransactions, getRates);

            Transactions = await getTransactions;
            IEnumerable<RateModel> rates = await getRates;

            IEnumerable<TransactionViewModel> data = Transactions.Where(x => x.Currency != "EUR").Select(transaction => SetCurrencyConversion(transaction));
        }
        private TransactionViewModel SetCurrencyConversion(TransactionModel model)
        {
            TransactionModel condition = model;
            TransactionViewModel viewModel = new TransactionViewModel
            {
                Sku = model.Sku,
                Currency = model.Currency,
                Amount = model.Amount,
                CurrencyConverted = CurrencyConversion
            };

            if (model.Currency == CurrencyConversion)
            {
                viewModel.AmountConverted = model.Amount;
            }
            else if (Rates.Any(x => x.From == condition.Currency && x.To == "EUR"))
            {
                viewModel.AmountConverted = GetMount(Rates.SingleOrDefault(x => x.From == condition.Currency && x.To == CurrencyConversion), model);
            }
            else
            {
                viewModel.AmountConverted = GetMount2(Rates.SingleOrDefault(x => x.To == condition.Currency && x.From == CurrencyConversion), model);
            }

            return viewModel;
        }

        private decimal GetMount(RateModel rate, TransactionModel transaction) => decimal.Round((rate.Rate * transaction.Amount), 2);

        private decimal GetMount2(RateModel rate, TransactionModel transaction) => decimal.Round((transaction.Amount / rate.Rate), 2);
    }
}
