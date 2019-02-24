using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.App.Models;

namespace GNB.Api.App.Services
{
    public interface IRateService<T> where T : class
    {
        Task<IEnumerable<T>> TryGetRates();
    }
}