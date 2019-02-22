using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Web.Controllers
{
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            IEnumerable<TransactionModel> data = new List<TransactionModel> 
            {
                new TransactionModel { Amount = 83.4m, Currency = "USD", Sku = "TD2006" },
                new TransactionModel { Amount = 13.4m, Currency = "USD", Sku = "TD2007" },
                new TransactionModel { Amount = 23.1m, Currency = "USD", Sku = "TD2706" },
                new TransactionModel { Amount = 13.4m, Currency = "USD", Sku = "TD2046" },
                new TransactionModel { Amount = 53.4m, Currency = "USD", Sku = "TD2306" }
            };
            return View(data);
        }
    }
}