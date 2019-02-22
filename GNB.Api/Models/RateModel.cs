using System.Runtime.Serialization;

namespace GNB.Api.Models
{
    public class RateModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Rate { get; set; }
    }
}
