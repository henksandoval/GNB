using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    class ConvertStreamUtility<T> where T : class
    {
        public static Task<IEnumerable<T>> ConvertStreamToModel(Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
            return Task.FromResult(serializer.ReadObject(stream) as IEnumerable<T>);
        }
    }
}
