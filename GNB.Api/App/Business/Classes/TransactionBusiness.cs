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

        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactions(TransactionViewModel viewModel)
        {
            try
            {
                newCurrency = viewModel.Currency;

                transactions = (await transactionService.TryGetTransactions(x => x.Sku == viewModel.Sku)).Select(x => new TransactionViewModel(x));
                transactions = SetCurrencyConvertedToTransactions();

                IList<TransactionViewModel> result = new List<TransactionViewModel>();

                TransactionViewModel[] tasks = await Task.WhenAll(transactions.Take(100).Select(transaction => currencyConverter.ApplyConversion(transaction)));

                var data = tasks.Where(x => x != null).ToList();
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private IEnumerable<TransactionViewModel> SetCurrencyConvertedToTransactions()
        {
            return transactions.Select(x =>
            {
                x.CurrencyConverted = newCurrency;
                return x;
            });
        }
    }
}
