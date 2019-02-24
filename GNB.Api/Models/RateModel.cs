using System;

namespace GNB.Api.Models
{
    public class RateModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Rate { get; set; }

        public override bool Equals(object other)
        {
            if (!(other is RateModel toCompareWith))
                return false;
            return From == toCompareWith.From && To == toCompareWith.To;
        }

        public override int GetHashCode() => HashCode.Combine(From, To);
    }
}
