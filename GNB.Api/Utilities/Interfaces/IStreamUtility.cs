using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    public interface IStreamUtility
    {
        Task<Stream> ConvertStringToStream(string data);
    }
}