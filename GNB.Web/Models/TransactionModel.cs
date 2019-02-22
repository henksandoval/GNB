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

        public override bool Equals(object other)
        {
            if (!(other is TransactionModel toCompareWith))
                return false;
            return Sku == toCompareWith.Sku && Amount == toCompareWith.Amount && Currency == toCompareWith.Currency;
        }
    }
}
