using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using xFordPassLite.net.Models;
using xFordPassLite.net.Utils;

namespace xFordPassLite.net.Services
{
    /// <summary>
    /// Logic to interact with Ford API
    /// This class is based off the PassiveLocker app by Oxymoron290 at https://github.com/Oxymoron290/PassiveLocker
    /// </summary>
    public class Client : IDisposable
    {
        private readonly Uri baseEndpoint;
        private readonly HttpClient client;
        private TokenResponseModel token;

        public string ErrorCode { get; private set; }

        private string rawJSON;
        /// <summary>
        /// The raw JSON returned from the server
        /// </summary>
        public string RawJSON { get => rawJSON; set => rawJSON = value; }

        public Client()
        {
            baseEndpoint = new Uri(ConfigData.BaseEndpoint);
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ConfigData.UserAgentString);
            token = new TokenResponseModel();
            RawJSON = "";
        }

        public void Dispose()
        {
            client.Dispose();
        }

        /// <summary>
        /// Authenticates the user with Ford API
        /// </summary>
        /// <returns>if sucessful, returns the 'auth-token'</returns>
        private async Task<TokenResponseModel> AuthAsync([System.Runtime.InteropServices.OptionalAttribute] string userId, [System.Runtime.InteropServices.OptionalAttribute] string pwd)
        {
            ErrorCode = "";
            TokenResponseModel trm = await AuthStage1Async(userId, pwd);
            return (trm != null && trm.AccessToken != null && trm.AccessToken != "") ? await AuthStage2Async(trm.AccessToken) : trm;
        }

        private async Task<TokenResponseModel> AuthStage1Async([System.Runtime.InteropServices.OptionalAttribute] string userId, [System.Runtime.InteropServices.OptionalAttribute] string pwd)
        {
            Uri endpoint = new Uri(ConfigData.IdpEndpoint);

            if (!client.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded")))
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            string _user = userId ?? ConfigData.USERNAME;
            string _pw = pwd ?? ConfigData.PW;
            if (_user == null || _user == "" || _pw == null || _pw == "" || !xFordPassLite.net.Utils.ConfigData.IsValidRegion(xFordPassLite.net.Utils.ConfigData.REGION))
            {
#if DEBUG
                Debugger.Log(1, "AuthAsync", "_user, _pw, and or regioncode is null");
#endif
                return null;
            }
            var data = new Dictionary<string, string>
                {
                    { "client_id", ConfigData.ClientId },
                    { "grant_type", "password" },
                    { "username", _user },
                    { "password", _pw }
                };

            TokenResponseModel result = null;
            FormUrlEncodedContent body = new FormUrlEncodedContent(data);
            try
            {
                using (HttpResponseMessage response = await client.PostAsync(endpoint, body))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (responseBody != null && responseBody != "")
                            result = JsonConvert.DeserializeObject<TokenResponseModel>(responseBody);
                        if (result != null)
                        {
                            token = result;
                            if (client.DefaultRequestHeaders.Contains("auth-token"))
                                client.DefaultRequestHeaders.Remove("auth-token");

                            client.DefaultRequestHeaders.Add("auth-token", result.AccessToken);

                            if (!client.DefaultRequestHeaders.Contains("Application-Id"))
                                client.DefaultRequestHeaders.Add("Application-Id", ConfigData.RegionToAppID());
                        }
                        return result;
                    }
                    else
                    {
                        if (ConfigData.DebugMode)
                            ErrorCode += "bad response.StatusCode " + response.StatusCode + Environment.NewLine;

#if DEBUG
                        Debugger.Log(1, "AuthAsync", "bad response.StatusCode " + response.StatusCode);
#endif
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.Unauthorized:
                                ErrorCode = "Unauthorized";
                                break;
                            case System.Net.HttpStatusCode.Forbidden:
                                ErrorCode = "Forbidden";
                                break;
                            case System.Net.HttpStatusCode.InternalServerError:
                                ErrorCode = "InternalServerError";
                                break;
                            case System.Net.HttpStatusCode.BadGateway:
                                ErrorCode = "BadGateway";
                                break;
                            case System.Net.HttpStatusCode.BadRequest:
                                ErrorCode = "BadRequest";
                                break;
                            case System.Net.HttpStatusCode.Conflict:
                                ErrorCode = "Conflict";
                                break;
                            case System.Net.HttpStatusCode.ExpectationFailed:
                                ErrorCode = "ExpectationFailed";
                                break;
                            case System.Net.HttpStatusCode.GatewayTimeout:
                                ErrorCode = "GatewayTimeout";
                                break;
                            case System.Net.HttpStatusCode.Gone:
                                ErrorCode = "Gone";
                                break;
                            case System.Net.HttpStatusCode.HttpVersionNotSupported:
                                ErrorCode = "HttpVersionNotSupported";
                                break;
                            case System.Net.HttpStatusCode.LengthRequired:
                                ErrorCode = "LengthRequired";
                                break;
                            case System.Net.HttpStatusCode.MethodNotAllowed:
                                ErrorCode = "MethodNotAllowed";
                                break;
                            case System.Net.HttpStatusCode.NotFound:
                                ErrorCode = "NotFound";
                                break;
                            case System.Net.HttpStatusCode.NotImplemented:
                                ErrorCode = "NotImplemented";
                                break;
                            case System.Net.HttpStatusCode.PaymentRequired:
                                ErrorCode = "PaymentRequired";
                                break;
                            case System.Net.HttpStatusCode.PreconditionFailed:
                                ErrorCode = "PreconditionFailed";
                                break;
                            case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                                ErrorCode = "ProxyAuthenticationRequired";
                                break;
                            case System.Net.HttpStatusCode.RequestEntityTooLarge:
                                ErrorCode = "RequestEntityTooLarge";
                                break;
                            case System.Net.HttpStatusCode.RequestTimeout:
                                ErrorCode = "RequestTimeout";
                                break;
                            case System.Net.HttpStatusCode.RequestUriTooLong:
                                ErrorCode = "RequestUriTooLong";
                                break;
                            case System.Net.HttpStatusCode.ServiceUnavailable:
                                ErrorCode = "ServiceUnavailable";
                                break;
                            case System.Net.HttpStatusCode.UnsupportedMediaType:
                                ErrorCode = "UnsupportedMediaType";
                                break;
                            case System.Net.HttpStatusCode.UpgradeRequired:
                                ErrorCode = "UpgradeRequired";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debugger.Log(1, "Client.AuthAsync", "HttpRequestException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "Client.AuthAsync", "Exception: " + ex.Message);
            }

            if (result != null)
            {
                this.token = result;
                if (client.DefaultRequestHeaders.Contains("auth-token"))
                    client.DefaultRequestHeaders.Remove("auth-token");
                client.DefaultRequestHeaders.Add("auth-token", result.AccessToken);

                if (!client.DefaultRequestHeaders.Contains("Application-Id"))
                    client.DefaultRequestHeaders.Add("Application-Id", ConfigData.RegionToAppID());
            }
            return result;
        }

        #region STAGE2
        private async Task<TokenResponseModel> AuthStage2Async(string accessToken)
        {
            if (!client.DefaultRequestHeaders.Contains("Application-Id"))
                client.DefaultRequestHeaders.Add("Application-Id", ConfigData.RegionToAppID());

            Task<HttpResponseMessage> operation;
            TokenResponseModel result = null;

            Uri endpoint = new Uri(ConfigData.OAuthEndpoint + "/token");
            string data = "{\"code\": \"" + accessToken + "\"}";

            HttpContent c = new StringContent(data, Encoding.UTF8, "application/json");
            operation = client.PutAsync(endpoint, c);

            try
            {
                using (HttpResponseMessage response = await operation)
                {
                    if (ConfigData.DebugMode)
                        ErrorCode += response + Environment.NewLine;
#if DEBUG
                    Debugger.Log(1, "AuthStage2Async", response + Environment.NewLine);
#endif
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<TokenResponseModel>(responseBody);

                        if (result != null)
                            this.token = result;
                        if (client.DefaultRequestHeaders.Contains("auth-token"))
                            client.DefaultRequestHeaders.Remove("auth-token");

                        client.DefaultRequestHeaders.Add("auth-token", result.AccessToken);

                        if (client.DefaultRequestHeaders.Contains("Application-Id"))
                            client.DefaultRequestHeaders.Remove("Application-Id");

                        if (!client.DefaultRequestHeaders.Contains("Application-Id"))
                            client.DefaultRequestHeaders.Add("Application-Id", ConfigData.RegionToAppID());
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.Unauthorized:
                                ErrorCode = "Unauthorized";
                                break;
                            case System.Net.HttpStatusCode.Forbidden:
                                ErrorCode = "Forbidden";
                                break;
                            case System.Net.HttpStatusCode.InternalServerError:
                                ErrorCode = "InternalServerError";
                                break;
                            case System.Net.HttpStatusCode.BadGateway:
                                ErrorCode = "BadGateway";
                                break;
                            case System.Net.HttpStatusCode.BadRequest:
                                ErrorCode = "BadRequest";
                                break;
                            case System.Net.HttpStatusCode.Conflict:
                                ErrorCode = "Conflict";
                                break;
                            case System.Net.HttpStatusCode.ExpectationFailed:
                                ErrorCode = "ExpectationFailed";
                                break;
                            case System.Net.HttpStatusCode.GatewayTimeout:
                                ErrorCode = "GatewayTimeout";
                                break;
                            case System.Net.HttpStatusCode.Gone:
                                ErrorCode = "Gone";
                                break;
                            case System.Net.HttpStatusCode.HttpVersionNotSupported:
                                ErrorCode = "HttpVersionNotSupported";
                                break;
                            case System.Net.HttpStatusCode.LengthRequired:
                                ErrorCode = "LengthRequired";
                                break;
                            case System.Net.HttpStatusCode.MethodNotAllowed:
                                ErrorCode = "MethodNotAllowed";
                                break;
                            case System.Net.HttpStatusCode.NotFound:
                                ErrorCode = "NotFound";
                                break;
                            case System.Net.HttpStatusCode.NotImplemented:
                                ErrorCode = "NotImplemented";
                                break;
                            case System.Net.HttpStatusCode.PaymentRequired:
                                ErrorCode = "PaymentRequired";
                                break;
                            case System.Net.HttpStatusCode.PreconditionFailed:
                                ErrorCode = "PreconditionFailed";
                                break;
                            case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                                ErrorCode = "ProxyAuthenticationRequired";
                                break;
                            case System.Net.HttpStatusCode.RequestEntityTooLarge:
                                ErrorCode = "RequestEntityTooLarge";
                                break;
                            case System.Net.HttpStatusCode.RequestTimeout:
                                ErrorCode = "RequestTimeout";
                                break;
                            case System.Net.HttpStatusCode.RequestUriTooLong:
                                ErrorCode = "RequestUriTooLong";
                                break;
                            case System.Net.HttpStatusCode.ServiceUnavailable:
                                ErrorCode = "ServiceUnavailable";
                                break;
                            case System.Net.HttpStatusCode.UnsupportedMediaType:
                                ErrorCode = "UnsupportedMediaType";
                                break;
                            case System.Net.HttpStatusCode.UpgradeRequired:
                                ErrorCode = "UpgradeRequired";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debugger.Log(1, "IssueCommand", "HttpRequestException: " + ex.Message);
                ErrorCode += ex.Message;
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "IssueCommand", "Exception: " + ex.Message);
                ErrorCode += ex.Message;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Gets the vehicles current status from Ford API server
        /// </summary>
        /// <returns>FordInfo</returns>
        public async Task<FordInfo> GetInfo()
        {
            TokenResponseModel A = await AuthAsync();
            if (A != null)
            {
                Uri endpoint = new Uri(baseEndpoint, $"/api/vehicles/v4/{ConfigData.VIN}/status");

                try
                {
                    if (ConfigData.DebugMode)
                        ErrorCode += endpoint.ToString() + Environment.NewLine;

#if DEBUG
                    Debugger.Log(1, "Client.GetInfo", "GET - " + endpoint.ToString());
#endif
                    using (HttpResponseMessage response = await client.GetAsync(endpoint))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string jSON = await response.Content.ReadAsStringAsync();
                            VehicleStatusResponse result = JsonConvert.DeserializeObject<VehicleStatusResponse>(jSON);
                            VehicleStatus vdata = result.VehicleStatus;
                            FordInfo ford = new FordInfo(vdata, jSON);

                            RawJSON = jSON;
                            return ford;
                        }
                        else
                        {
                            switch (response.StatusCode)
                            {
                                case System.Net.HttpStatusCode.Unauthorized:
                                    ErrorCode = "Unauthorized";
                                    break;
                                case System.Net.HttpStatusCode.Forbidden:
                                    ErrorCode = "Forbidden";
                                    break;
                                case System.Net.HttpStatusCode.InternalServerError:
                                    ErrorCode = "InternalServerError";
                                    break;
                                case System.Net.HttpStatusCode.BadGateway:
                                    ErrorCode = "BadGateway";
                                    break;
                                case System.Net.HttpStatusCode.BadRequest:
                                    ErrorCode = "BadRequest";
                                    break;
                                //case System.Net.HttpStatusCode.Conflict:
                                //    ErrorCode = "Conflict";
                                //    break;
                                //case System.Net.HttpStatusCode.ExpectationFailed:
                                //    ErrorCode = "ExpectationFailed";
                                //    break;
                                case System.Net.HttpStatusCode.GatewayTimeout:
                                    ErrorCode = "GatewayTimeout";
                                    break;
                                //case System.Net.HttpStatusCode.Gone:
                                //    ErrorCode = "Gone";
                                //    break;
                                //case System.Net.HttpStatusCode.HttpVersionNotSupported:
                                //    ErrorCode = "HttpVersionNotSupported";
                                //    break;
                                case System.Net.HttpStatusCode.LengthRequired:
                                    ErrorCode = "LengthRequired";
                                    break;
                                case System.Net.HttpStatusCode.MethodNotAllowed:
                                    ErrorCode = "MethodNotAllowed";
                                    break;
                                case System.Net.HttpStatusCode.NotFound:
                                    ErrorCode = "NotFound";
                                    break;
                                case System.Net.HttpStatusCode.NotImplemented:
                                    ErrorCode = "NotImplemented";
                                    break;
                                //case System.Net.HttpStatusCode.PaymentRequired:
                                //    ErrorCode = "PaymentRequired";
                                //    break;
                                case System.Net.HttpStatusCode.PreconditionFailed:
                                    ErrorCode = "PreconditionFailed";
                                    break;
                                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                                    ErrorCode = "ProxyAuthenticationRequired";
                                    break;
                                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                                    ErrorCode = "RequestEntityTooLarge";
                                    break;
                                case System.Net.HttpStatusCode.RequestTimeout:
                                    ErrorCode = "RequestTimeout";
                                    break;
                                case System.Net.HttpStatusCode.RequestUriTooLong:
                                    ErrorCode = "RequestUriTooLong";
                                    break;
                                case System.Net.HttpStatusCode.ServiceUnavailable:
                                    ErrorCode = "ServiceUnavailable";
                                    break;
                                case System.Net.HttpStatusCode.UnsupportedMediaType:
                                    ErrorCode = "UnsupportedMediaType";
                                    break;
                                //case System.Net.HttpStatusCode.UpgradeRequired:
                                //    ErrorCode = "UpgradeRequired";
                                //    break;
                                default:
                                    ErrorCode = "Other";
                                    break;
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debugger.Log(1, "Client.GetInfo", "HttpRequestException: " + ex.Message);
                    ErrorCode += ex.Message;
                }
                catch (Exception ex)
                {
                    Debugger.Log(1, "Client.GetInfo", "Exception: " + ex.Message);
                    ErrorCode += ex.Message;
                }
            }
            return new FordInfo(null, null);
        }

        public async Task<CommandResponse> IssueCommand(FordCommand command)
        {
            Task<HttpResponseMessage> operation;
            Uri endpoint = null;
            CommandResponse result = new CommandResponse();
            TokenResponseModel A = await AuthAsync();
            if (A != null && ConfigData.VIN != null && ConfigData.VIN != "")
            {
                switch (command)
                {
                    case FordCommand.Refresh:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/status");
#if DEBUG
                        Debugger.Log(1, "IssueCommand", "PUT - " + endpoint.ToString());
#endif
                        operation = client.PutAsync(endpoint, null);
                        break;
                    case FordCommand.Start:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/engine/start");
                        operation = client.PutAsync(endpoint, null);
                        break;
                    case FordCommand.Stop:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/engine/start");
                        operation = client.DeleteAsync(endpoint);
                        break;
                    case FordCommand.Lock:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/doors/lock");
#if DEBUG
                        Debugger.Log(1, "IssueCommand", "PUT - " + endpoint.ToString());
#endif
                        operation = client.PutAsync(endpoint, null);
                        break;
                    case FordCommand.Unlock:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/doors/lock");
#if DEBUG
                        Debugger.Log(1, "IssueCommand", "DELETE - " + endpoint.ToString());
#endif
                        operation = client.DeleteAsync(endpoint);
                        break;
                    default:
                        throw new Exception("Invalid Ford Command");
                }

                if (endpoint != null)
                {
                    //try
                    //{
                    using (HttpResponseMessage response = await operation)
                    {
                        if (ConfigData.DebugMode)
                            ErrorCode += response + Environment.NewLine;

#if DEBUG
                        Debugger.Log(1, "IssueCommand", "response: " + response + Environment.NewLine);
#endif

                        if (response.IsSuccessStatusCode)
                        {
                            RawJSON = await response.Content.ReadAsStringAsync();
                            if (ConfigData.DebugMode)
                                ErrorCode += RawJSON + Environment.NewLine;

#if DEBUG
                            Debugger.Log(1, "IssueCommand", "RawJSON: " + RawJSON + Environment.NewLine);
#endif
                            result = JsonConvert.DeserializeObject<CommandResponse>(RawJSON);
                            Debugger.Log(1, "IssueCommand", "result: " + result.ToString() + Environment.NewLine);
                        }
                        else
                        {
                            switch (response.StatusCode)
                            {
                                case System.Net.HttpStatusCode.Unauthorized:
                                    ErrorCode = "Unauthorized";
                                    break;
                                case System.Net.HttpStatusCode.Forbidden:
                                    ErrorCode = "Forbidden";
                                    break;
                                case System.Net.HttpStatusCode.InternalServerError:
                                    ErrorCode = "InternalServerError";
                                    break;
                                case System.Net.HttpStatusCode.BadGateway:
                                    ErrorCode = "BadGateway";
                                    break;
                                case System.Net.HttpStatusCode.BadRequest:
                                    ErrorCode = "BadRequest";
                                    break;
                                case System.Net.HttpStatusCode.Conflict:
                                    ErrorCode = "Conflict";
                                    break;
                                case System.Net.HttpStatusCode.ExpectationFailed:
                                    ErrorCode = "ExpectationFailed";
                                    break;
                                case System.Net.HttpStatusCode.GatewayTimeout:
                                    ErrorCode = "GatewayTimeout";
                                    break;
                                case System.Net.HttpStatusCode.Gone:
                                    ErrorCode = "Gone";
                                    break;
                                case System.Net.HttpStatusCode.HttpVersionNotSupported:
                                    ErrorCode = "HttpVersionNotSupported";
                                    break;
                                case System.Net.HttpStatusCode.LengthRequired:
                                    ErrorCode = "LengthRequired";
                                    break;
                                case System.Net.HttpStatusCode.MethodNotAllowed:
                                    ErrorCode = "MethodNotAllowed";
                                    break;
                                case System.Net.HttpStatusCode.NotFound:
                                    ErrorCode = "NotFound";
                                    break;
                                case System.Net.HttpStatusCode.NotImplemented:
                                    ErrorCode = "NotImplemented";
                                    break;
                                case System.Net.HttpStatusCode.PaymentRequired:
                                    ErrorCode = "PaymentRequired";
                                    break;
                                case System.Net.HttpStatusCode.PreconditionFailed:
                                    ErrorCode = "PreconditionFailed";
                                    break;
                                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                                    ErrorCode = "ProxyAuthenticationRequired";
                                    break;
                                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                                    ErrorCode = "RequestEntityTooLarge";
                                    break;
                                case System.Net.HttpStatusCode.RequestTimeout:
                                    ErrorCode = "RequestTimeout";
                                    break;
                                case System.Net.HttpStatusCode.RequestUriTooLong:
                                    ErrorCode = "RequestUriTooLong";
                                    break;
                                case System.Net.HttpStatusCode.ServiceUnavailable:
                                    ErrorCode = "ServiceUnavailable";
                                    break;
                                case System.Net.HttpStatusCode.UnsupportedMediaType:
                                    ErrorCode = "UnsupportedMediaType";
                                    break;
                                case System.Net.HttpStatusCode.UpgradeRequired:
                                    ErrorCode = "UpgradeRequired";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //}
                    //catch (HttpRequestException ex)
                    //{
                    //    Debugger.Log(1, "IssueCommand", "HttpRequestException: " + ex.Message);
                    //    ErrorCode = ex.Message;
                    //}
                    //catch (Exception ex)
                    //{
                    //    Debugger.Log(1, "IssueCommand", "Exception: " + ex.Message);
                    //    ErrorCode = ex.Message;
                    //}
                }
            }
            return result;
        }

        public async Task<CommandStatus> CommandStatusAsync(FordCommand command, CommandResponse commandId)
        {
            CommandStatus result = new CommandStatus();
            Uri endpoint = null;
            TokenResponseModel A = await AuthAsync();
            if (A != null)
            {
                switch (command)
                {
                    case FordCommand.Start:
                    case FordCommand.Stop:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/engine/start/{commandId.CommandId}");
                        break;
                    case FordCommand.Lock:
                    case FordCommand.Unlock:
                        endpoint = new Uri(baseEndpoint, $"api/vehicles/v2/{ConfigData.VIN}/doors/lock/{commandId.CommandId}");
                        break;
                    case FordCommand.Refresh:
                        break;
                    default:
                        throw new Exception("Invalid Ford Command");
                }
                if (endpoint != null)
                {
                    //try
                    //{
#if DEBUG
                    Debugger.Log(1, "CommandStatusAsync", "GET - " + endpoint.ToString());
#endif
                    using (HttpResponseMessage response = await client.GetAsync(endpoint))
                    {
                        if (ConfigData.DebugMode)
                            ErrorCode += "response: " + response + Environment.NewLine;

#if DEBUG
                        Debugger.Log(1, "CommandStatusAsync", "response: " + response);
#endif
                        if (response.IsSuccessStatusCode)
                        {
                            RawJSON = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<CommandStatus>(RawJSON);
                            if (ConfigData.DebugMode)
                                ErrorCode += "response: " + result + Environment.NewLine;

                            Debugger.Log(1, "CommandStatusAsync", "result: " + result.ToString() + Environment.NewLine);

                        }
                    }
                    //}
                    //catch (HttpRequestException ex)
                    //{
                    //    Debugger.Log(1, "CommandStatusAsync", "HttpRequestException: " + ex.Message);
                    //    ErrorCode = ex.Message;
                    //}
                    //catch (Exception ex)
                    //{
                    //    Debugger.Log(1, "CommandStatusAsync", "Exception: " + ex.Message);
                    //    ErrorCode = ex.Message;
                    //}
                }
            }
            return result;
        }

        public async Task<ELoginStatus> ValidateLoginInfo(string userId, string pwd, [System.Runtime.InteropServices.OptionalAttribute] string _vin)
        {
            TokenResponseModel A = await AuthAsync(userId, pwd);
            if (A == null)
            {
#if DEBUG
                Debugger.Log(1, "ValidateLoginInfo", "ELoginStatus.InvalidUserID_or_PW" + Environment.NewLine);
#endif
                return ELoginStatus.InvalidUserID_or_PW;
            }
            else if (A != null)
            {
                if (_vin == null || _vin == "")
                {
#if DEBUG
                    Debugger.Log(1, "ValidateLoginInfo", "ELoginStatus.ValidUserID_BadVIN No VIN" + Environment.NewLine);
#endif
                    return ELoginStatus.ValidUserID_BadVIN;
                }
                else
                {
                    Uri endpoint = new Uri(baseEndpoint, $"/api/vehicles/v4/{_vin}/status");
                    try
                    {
#if DEBUG
                        Debugger.Log(1, "ValidateLoginInfo", "GET - " + endpoint.ToString());
#endif
                        using (HttpResponseMessage response = await client.GetAsync(endpoint))
                        {
                            if (response.IsSuccessStatusCode)
                            {
#if DEBUG
                                Debugger.Log(1, "ValidateLoginInfo", "ELoginStatus.Sucess" + Environment.NewLine);
#endif
                                return ELoginStatus.Sucess;
                            }
                            else
                            {
#if DEBUG
                                Debugger.Log(1, "ValidateLoginInfo", "ELoginStatus.Bad VIN" + Environment.NewLine);
#endif
                                return ELoginStatus.ValidUserID_BadVIN;
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Debugger.Log(1, "ValidateLoginInfo", "HttpRequestException: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debugger.Log(1, "ValidateLoginInfo", "Exception: " + ex.Message);
                    }
                }
            }
#if DEBUG
            Debugger.Log(1, "ValidateLoginInfo", "ELoginStatus.Error" + Environment.NewLine);
#endif
            return ELoginStatus.Error;
        }

        public string GetErrorCode()
        {
            return ErrorCode;
        }
    }
}

