using System.Collections.Generic;
using System.Linq;

namespace GNB.Web.Utilities
{
    public class DataTableUtility
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<Column> Columns { get; set; }
        public Search Search { get; set; }
        public List<CustomSearch> CustomSearches { get; set; }
        public List<Order> Order { get; set; }

        internal object GetPropertiesDataTable(IList<object> source) => new
        {
            draw = Draw,
            recordsTotal = source.Count(),
            recordsFiltered = source.Count(),
            data = GetDataFiltered(source)
        };

        internal object GetPropertiesDataTable(IEnumerable<object> source) => new
        {
            draw = Draw,
            recordsTotal = source.Count(),
            recordsFiltered = source.Count(),
            data = GetDataFiltered(source.ToList())
        };

        internal string GetParameterInCustomSearchByName(string nameParameter) => CustomSearches?.FirstOrDefault(x => x.Name.ToLower() == nameParameter.ToLower())?.Value?.ToString() ?? string.Empty;

        private IEnumerable<object> GetDataFiltered(IList<object> source) => source.Skip(Start).Take(Length);
    }


    public class Column
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class CustomSearch
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
