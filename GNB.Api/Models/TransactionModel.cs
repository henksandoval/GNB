using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GNB.Api.Models
{
    [DataContract]
    public class TransactionModel
    {
        [DataMember(Name = "sku")]
        public string Sku { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }
    }
}
