using GNB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Repositories
{
    public interface IRateRepository
    {
        Task<IEnumerable<RateModel>> TryGetAllRates(Func<RateModel, bool> predicate = null);
    }
}
