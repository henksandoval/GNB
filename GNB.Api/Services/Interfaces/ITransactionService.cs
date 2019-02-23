using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.Models;

namespace GNB.Api.Services
{
    public interface ITransactionService<T> where T : class
    {
        Task<IEnumerable<T>> TryGetTransactions();
        Task<IEnumerable<T>> TryGetTransactions(Func<T, bool> predicate);
    }
}