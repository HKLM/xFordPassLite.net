using System.Diagnostics;
using System.Threading.Tasks;

using xFordPassLite.net.Models;

namespace xFordPassLite.net.Services
{
    public class FordDataStore : IDataStore<FordInfo>
    {
        private Client client;

        public string RawJSON => client.RawJSON;

        public FordDataStore()
        {
            client = new Client();
        }

        /// <summary>
        /// Sends the FordCommand
        /// </summary>
        /// <returns>CommandStatus.Status</returns>
        public async Task<int> SendCommandAsync(FordCommand fordCommand)
        {
            CommandResponse response = await client.IssueCommand(fordCommand);
            GetErrorCode();

#if DEBUG
            Debugger.Log(1, "SendCommandAsync", "IssueCommand - " + response.ToString());
#endif
            if (response != null)
            {
                CommandStatus result = await client.CommandStatusAsync(fordCommand, response);
                GetErrorCode();

#if DEBUG
                Debugger.Log(1, "SendCommandAsync", "CommandStatusAsync - " + result.ToString());
#endif
                await Task.Delay(4000);
                GetErrorCode();

#if DEBUG
                Debugger.Log(1, "SendCommandAsync", "result.Status: " + result.Status.ToString());
#endif
                return await Task.FromResult(result.Status);
            }
            else
                return await Task.FromResult(418);
        }

        public async Task<FordInfo> GetFordInfoAsync()
        {
            FordInfo ford = await client.GetInfo();
            if (ford.Vehicle != null)
            {
                VehicleStatus VehicleInfo = ford.Vehicle;
                ford.Vehicle = VehicleInfo;
            }
            GetErrorCode();

            return await Task.FromResult(ford);
        }

        public async Task<ELoginStatus> ValidateFordPassAsync(string userId, string pwd, [System.Runtime.InteropServices.OptionalAttribute] string vin)
        {
            ELoginStatus ford = await client.ValidateLoginInfo(userId, pwd, vin);
            return await Task.FromResult(ford);
        }

        public string GetErrorCode()
        {
            return client.GetErrorCode();
        }
    }
}