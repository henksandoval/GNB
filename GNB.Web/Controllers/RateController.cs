using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Web.Models;
using GNB.Web.Repositories;
using GNB.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Web.Controllers
{
    public class RateController : Controller
    {
        private readonly IRateRepository RateRepository;

        public RateController(IRateRepository RateRepository)
        {
            this.RateRepository = RateRepository;
        }

        [HttpGet]
        public IActionResult Index() => View("Index");

        [HttpPost]
        public async Task<IActionResult> GetRates(DataTableUtility dataTable)
        {
            IEnumerable<RateModel> Rates = await RateRepository.TryGetAllRates();
            return Ok(dataTable.GetPropertiesDataTable(Rates));
        }
    }
}