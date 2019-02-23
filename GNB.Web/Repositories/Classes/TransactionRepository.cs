using GNB.Web.Models;
using GNB.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Web.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionService transactionService;

        public TransactionRepository(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<IEnumerable<TransactionModel>> TryGetAllTransactions(Func<TransactionModel, bool> predicate = null)
        {
            //return (await transactionService.GetAllTransactions()).Where(predicate);
            IEnumerable<TransactionModel> data = (await transactionService.GetAllTransactions()).Where(predicate);
            return data.Where(predicate);
        }

        public async Task<IEnumerable<TransactionModel>> TryGetAllTransactions(IEnumerable<Func<TransactionModel, bool>> predicates = null)
        {
            IEnumerable<TransactionModel> data = await transactionService.GetAllTransactions();
            return data.WhereIf(predicates);
        }
    }

    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return true;
            }

            if (!(source == null || !source.Any()))
            {
                return false;
            }

            if (source.ToList().Count == 0)
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<T> Flat<T>(this IEnumerable<T> l, Func<T, IEnumerable<T>> f) => l.SelectMany(i => new T[] { i }.Concat(f(i).Flat(f)));

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, IEnumerable<Func<TSource, bool>> predicates)
        {
            IEnumerable<TSource> sourceFiltered = source;

            foreach (Func<TSource, bool> predicate in predicates)
            {
                if (condition)
                    sourceFiltered = sourceFiltered.Where(predicate);
            }

            return sourceFiltered;
        }
    }
}
