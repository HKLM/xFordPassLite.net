using System.Runtime.InteropServices;
using System.Threading.Tasks;

using xFordPassLite.net.Models;

namespace xFordPassLite.net.Services
{
    public interface IDataStore<T>
    {
        string GetErrorCode();
        Task<T> GetFordInfoAsync();
        Task<int> SendCommandAsync(FordCommand fordCommand);
        Task<ELoginStatus> ValidateFordPassAsync(string userId, string pwd, [Optional] string vin);
    }
}
