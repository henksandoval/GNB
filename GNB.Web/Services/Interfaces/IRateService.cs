using GNB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Services
{
    public interface IRateService
    {
        Task<IEnumerable<RateModel>> GetAllRates();
    }
}
