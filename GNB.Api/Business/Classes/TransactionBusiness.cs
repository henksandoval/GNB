using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using GNB.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Business
{
    public class TransactionBusiness : ITransactionBusiness
    {
        private readonly ITransactionService<TransactionModel> transactionService;
        private readonly ICurrencyConverter currencyConverter;
        private IEnumerable<TransactionModel> Transactions;
        private IEnumerable<RateModel> Rates;
        private const string CurrencyConversion = "EUR";

        public TransactionBusiness(ITransactionService<TransactionModel> transactionService, ICurrencyConverter currencyConverter)
        {
            this.transactionService = transactionService;
            this.currencyConverter = currencyConverter;
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions()
        {
            IEnumerable<TransactionModel> transactions = await transactionService.TryGetTransactions();

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
            else if (Rates.Any(x => x.To == condition.Currency && x.From == CurrencyConversion))
            {
                viewModel.AmountConverted = GetMount2(Rates.SingleOrDefault(x => x.To == condition.Currency && x.From == CurrencyConversion), model);
            }
            else if(Rates.Any(x => x.From == condition.Currency) || Rates.Any(x => x.To == condition.Currency))
            {
                var monedaEncontrada = false;
                //while (!monedaEncontrada)
                //{
                //    var rate = Rates.SingleOrDefault(x => x.From == condition.Currency);

                    
                //}
                viewModel.AmountConverted = GetMount2(Rates.SingleOrDefault(x => x.From == CurrencyConversion && x.To == condition.Currency), model);
            }

            return viewModel;
        }

        private decimal GetMount(RateModel rate, TransactionModel transaction) => decimal.Round((rate.Rate * transaction.Amount), 2);

        private decimal GetMount2(RateModel rate, TransactionModel transaction) => decimal.Round((transaction.Amount / rate.Rate), 2);
    }
}
