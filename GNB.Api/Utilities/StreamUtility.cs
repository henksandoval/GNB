using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    public class StreamUtility : IStreamUtility
    {
        public Task<Stream> ConvertStringToStream(string data) => 
            Task.FromResult<Stream>(new MemoryStream(Encoding.ASCII.GetBytes(data)));
    }
}
