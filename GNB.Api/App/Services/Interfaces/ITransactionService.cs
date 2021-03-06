﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.App.Models;

namespace GNB.Api.App.Services
{
    public interface ITransactionService<T> where T : class
    {
        Task<IEnumerable<T>> TryGetTransactions();
        Task<IEnumerable<T>> TryGetTransactions(Func<T, bool> predicate);
    }
}