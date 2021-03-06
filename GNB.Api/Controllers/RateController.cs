﻿using GNB.Api.App.Models;
using GNB.Api.App.Services;
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
        public async Task<IEnumerable<RateModel>> GetAllRates() => 
            await RateService.TryGetRates();
    }
}