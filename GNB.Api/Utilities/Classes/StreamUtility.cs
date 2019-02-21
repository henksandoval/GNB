using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    class StreamUtility<T> : IStreamUtility<T> where T : class
    {
        public Task<Stream> ConvertStringToStream(string data) =>
            Task.FromResult<Stream>(new MemoryStream(Encoding.ASCII.GetBytes(data)));

        public static Task<IEnumerable<T>> ConvertStreamToModel(Stream stream)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
            return Task.FromResult(jsonSerializer.ReadObject(stream) as IEnumerable<T>);
        }
    }
}
