using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    public interface IConvertStreamUtility<T> where T : class
    {
        Task<IEnumerable<T>> ConvertStreamToModel(Stream stream);
    }
}
