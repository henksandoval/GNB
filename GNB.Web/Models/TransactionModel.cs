using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Web.Models
{
    public class TransactionModel
    {
        public string Sku { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}
