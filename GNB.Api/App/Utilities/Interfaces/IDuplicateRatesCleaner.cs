using System.Collections.Generic;
using GNB.Api.App.Models;

namespace GNB.Api.App.Utilities
{
    public interface IDuplicateRatesCleaner
    {
        IEnumerable<RateModel> DeletingDuplicates(IEnumerable<RateModel> rates);
    }
}