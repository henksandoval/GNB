using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    public interface IStreamUtility<T> where T : class
    {
        Task<Stream> ConvertStringToStream(string data);
    }
}