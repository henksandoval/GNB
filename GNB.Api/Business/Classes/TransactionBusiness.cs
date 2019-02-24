using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.Utilities;
using GNB.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Business
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

        public async Task<IEnumerable<TransactionModel>> GetAllTransactions()
        {
            IEnumerable<TransactionModel> transactions = await transactionService.TryGetTransactions();

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
