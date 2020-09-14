using Newtonsoft.Json.Linq;
using RestSharp;
using Solomon.Network.Common;
using Solomon.Network.Data;
using System;
using System.Threading.Tasks;

namespace Solomon.Network
{
    public partial class NetworkManager : IServerProtocol
    {
        public const int TOKEN_EXPIRED = 410;

        public async Task TokenRefresh()
        {
            JObject jObject = new JObject();
            jObject["refresh_token"] = Options.tokenInfo.RefreshToken;
            var resp = await GetResponse<string>(Options.tokenRefreshUrl, Method.POST, jObject.ToString());
            if (resp.Status == (int)System.Net.HttpStatusCode.OK)
            {
                Options.tokenInfo.Token = JObject.Parse(resp.Data)?["data"]?["new_token"]?.ToString();
            }
            else
            {
                if (resp.Status == TOKEN_EXPIRED)
                {
                    if (Options.password != null)
                    {
                        jObject = new JObject();
                        jObject["device"] = "PC";
                        jObject["id"] = Options.id;
                        jObject["pw"] = LoginInfo.Sha512Hash(Options.password);
                        var response = await GetResponse<TokenInfo>(Options.loginUrl, Method.POST, jObject.ToString());

                        Options.tokenInfo = new TokenInfo()
                        {
                            RefreshToken = response.Data.RefreshToken,
                            Token = response.Data.Token
                        };
                    }
                }
            }
        }

        public Task GetResponse<T>(string counsel_url, object get, object p)
        {
            throw new NotImplementedException();
        }

        public bool CheckTokenExpired(IRestResponse response)
        {
            if ((int)response.ResponseStatus == TOKEN_EXPIRED)
            {
                return true;
            }
            return false;
        }
    }
}
