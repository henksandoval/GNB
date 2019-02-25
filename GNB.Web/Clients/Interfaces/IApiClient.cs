using GNB.Web.Models;
using System.Threading.Tasks;

namespace GNB.Web.Clients
{
    public interface IApiClient
    {
        Task<string> GetStringRates();
        Task<string> GetStringTransactions(TransactionModel model = null);
    }
}
