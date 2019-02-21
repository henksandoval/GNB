using System.Runtime.Serialization;

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

        public override bool Equals(object other)
        {
            if (!(other is TransactionModel toCompareWith))
                return false;
            return Sku == toCompareWith.Sku && Amount == toCompareWith.Amount && Currency == toCompareWith.Currency;
        }
    }
}
