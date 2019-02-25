using GNB.Web.Models;
using GNB.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Web.Repositories
{
    public class RateRepository : IRateRepository
    {
        private readonly IRateService RateService;

        public RateRepository(IRateService RateService)
        {
            this.RateService = RateService;
        }

        public async Task<IEnumerable<RateModel>> TryGetAllRates(Func<RateModel, bool> predicate = null)
        {
            return await RateService.GetAllRates();
            //return (await RateService.GetAllRates()).Where(predicate);
            //IEnumerable<RateModel> data = (await RateService.GetAllRates()).Where(predicate);
            //return data.Where(predicate);
        }
    }
}
