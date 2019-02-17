using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.Models;

namespace GNB.Api.Services
{
    public interface IRateService
    {
        Task<IEnumerable<RateModel>> GetRates();
    }
}