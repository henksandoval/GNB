using System.Runtime.Serialization;

namespace GNB.Api.Models
{
    [DataContract]
    public class RateModel
    {
        [DataMember(Name = "from")]
        public string From { get; set; }

        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "rate")]
        public decimal Rate { get; set; }
    }
}
