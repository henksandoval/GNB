using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.Clients
{
    public interface IHerokuAppClient
    {
        Task<Stream> GetStreamRates();
        Task<Stream> GetStreamTransactions();
    }
}