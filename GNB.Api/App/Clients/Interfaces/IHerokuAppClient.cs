using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.App.Clients
{
    public interface IHerokuAppClient
    {
        Task<string> GetStringRates();
        Task<string> GetStringTransactions();
    }
}