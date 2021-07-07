using Newtonsoft.Json.Linq;
using RestSharp;
using Solomon.Network;
using Solomon.Network.Common;
using Solomon.Network.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Solomon.Core.Login.Service
{
    public class LoginService
    {
        private NetworkManager networkManager;
        //private readonly Encoding encoding = Encoding.UTF8;

        private const string MEMBER_URL = "/members";
        private const string LOGIN_URL = "/auth/login";
        private const string LOGOUT_URL = MEMBER_URL + "logout";
        private const string TOKEN_REFRESH_URL = "/token/refresh";

        public LoginService(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public async Task<Response<TokenInfo>> Login(string id, string pw)
        {
            var jObject = new JObject();
            jObject["id"] = id;
            jObject["pw"] = Sha512Hash(pw);

            var resp = await networkManager.GetResponse<TokenInfo>(Options.loginUrl, Method.POST, jObject.ToString());

            if (resp.Data != null)
            {
                Options.tokenInfo.Token = resp.Data.Token;
                Options.tokenInfo.RefreshToken = resp.Data.RefreshToken;
            }

            return resp;
        }

        private string Sha512Hash(string str)
        {
            var sha512 = new SHA512CryptoServiceProvider();
            byte[] resultHash = sha512.ComputeHash(Encoding.Default.GetBytes(str));
            string transPwd = string.Empty;

            foreach (byte x in resultHash)
            {
                transPwd += $"{x:x2}";
            }

            return transPwd;
        }

        public void SettingHttpRequest(string serverUrl)
        {
            networkManager.SetHTTPRequestURL(serverUrl, LOGIN_URL, LOGOUT_URL, TOKEN_REFRESH_URL);
        }
    }
}
