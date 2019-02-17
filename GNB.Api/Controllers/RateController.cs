﻿using GNB.Api.Models;
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
        private readonly RateService RateService;

        public RateController(RateService rateService)
        {
            RateService = rateService;
        }

        public async Task<IEnumerable<RateModel>> GetRates() => await RateService.GetRates();
    }
}