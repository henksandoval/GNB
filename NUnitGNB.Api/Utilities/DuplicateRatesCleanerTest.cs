using GNB.Api.App.Models;
using GNB.Api.App.Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GNB.Api.Tests.Utilities
{
    [TestFixture]
    internal class DuplicateRatesCleanerTest
    {
        [TestCaseSource(typeof(DuplicateRatesCleanerTest), "SomeTestsCases")]
        public void DeletingDuplicates(IEnumerable<RateModel> rates, IEnumerable<RateModel> expected)
        {
            DuplicateRatesCleaner cleaner = new DuplicateRatesCleaner();
            IEnumerable<RateModel> result = cleaner.DeletingDuplicates(rates.ToList());
            Assert.AreEqual(result, expected);
        }

        private static IEnumerable<TestCaseData> SomeTestsCases {
            get {
                yield return new TestCaseData
                (
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                            new RateModel { From = "CAD", To = "AUD", Rate = 1.79m },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                        }
                ).SetName("CleanDuplicateSimple");
                yield return new TestCaseData
                (
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                            new RateModel { From = "CAD", To = "AUD", Rate = 1.79m },
                            new RateModel { From = "USD", To = "EUR", Rate = 1.79m },
                            new RateModel { From = "USD", To = "CAD", Rate = 1.79m },
                            new RateModel { From = "BS", To = "USD", Rate = 1.79m },
                            new RateModel { From = "USD", To = "BS", Rate = 1.79m },
                        },
                    new List<RateModel>
                        {
                            new RateModel { From = "AUD", To = "CAD", Rate = 0.56m },
                            new RateModel { From = "USD", To = "EUR", Rate = 0.56m },
                            new RateModel { From = "USD", To = "CAD", Rate = 1.79m },
                            new RateModel { From = "BS", To = "USD", Rate = 1.79m },
                        }
                ).SetName("CleanDuplicateComplex");
            }
        }
    }
}
