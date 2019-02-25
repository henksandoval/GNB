using System.Threading.Tasks;

namespace GNB.Web.Clients
{
    public interface IApiClient
    {
        Task<string> GetStringTransactions();
        Task<string> GetStringRates();
    }
}
