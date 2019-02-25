using GNB.Api.App.Models;
using System.Collections.Generic;
using System.Linq;

namespace GNB.Api.App.Utilities
{
    public class DuplicateRatesCleaner : IDuplicateRatesCleaner
    {
        private IList<RateModel> ratesReturn;

        public IEnumerable<RateModel> DeletingDuplicates(IEnumerable<RateModel> rates)
        {
            ratesReturn = new List<RateModel>
            {
                rates.First()
            };

            foreach (RateModel rate in rates)
            {
                if (!CheckDuplicate(rate))
                {
                    ratesReturn.Add(rate);
                }
            }

            return ratesReturn;
        }

        private bool CheckDuplicate(RateModel rate) =>
            ratesReturn.Any(x => (x.From == rate.From && x.To == rate.To) || (x.From == rate.To && x.To == rate.From));
    }
}
