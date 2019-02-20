using GNB.Api.Models;
using GNB.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService<RateModel> RateService;

        public RateController(IRateService<RateModel> rateService)
        {
            RateService = rateService;
        }

        [HttpGet]
        public async Task<IEnumerable<RateModel>> GetRates() => await RateService.GetRates();

    }
}