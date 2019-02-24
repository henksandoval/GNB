using GNB.Api.App.Models;
using GNB.Api.App.Services;
using GNB.Api.App.Utilities;
using GNB.Api.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.App.Business
{
    public class TransactionBusiness : ITransactionBusiness
    {
        private readonly ITransactionService<TransactionModel> transactionService;
        private readonly ICurrencyConverter currencyConverter;
        private string newCurrency;
        private IEnumerable<TransactionViewModel> transactions;
        private const string CurrencyConversion = "EUR";

        public TransactionBusiness(ITransactionService<TransactionModel> transactionService, ICurrencyConverter currencyConverter)
        {
            this.transactionService = transactionService;
            this.currencyConverter = currencyConverter;
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions()
        {
            IEnumerable<TransactionViewModel> transactions = (await transactionService.TryGetTransactions()).Select(x => new TransactionViewModel(x));

            return transactions;
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactionsWithNewCurrency(string newCurrency)
        {
            this.newCurrency = newCurrency;

            transactions = (await transactionService.TryGetTransactions()).Select(x => new TransactionViewModel(x));
            transactions = SetCurrencyConvertedToTransactions();



            return transactions;
        }

        private IEnumerable<TransactionViewModel> SetCurrencyConvertedToTransactions()
        {
            return transactions.Select(x =>
            {
                x.CurrencyConverted = newCurrency;
                return x;
            });
        }

        private Task SetCurrencyConverted(TransactionViewModel transaction) =>
            Task.FromResult(transaction.CurrencyConverted = newCurrency);
    }
}
