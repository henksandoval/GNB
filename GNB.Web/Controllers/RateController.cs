using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Web.Controllers
{
    public class RateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}