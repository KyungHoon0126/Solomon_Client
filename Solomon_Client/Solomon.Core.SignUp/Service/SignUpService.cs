using Newtonsoft.Json.Linq;
using RestSharp;
using Solomon.Network;
using Solomon.Network.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Solomon.Core.SignUp.Service
{
    public class SignUpService
    {
        #region SettingHttpRequest
        private const string MEMBER_URL = "/members";
        private const string LOGIN_URL = "/auth/login";
        private const string LOGOUT_URL = MEMBER_URL + "logout";
        private const string TOKEN_REFRESH_URL = "/token/refresh";
        #endregion

        public const string SIGNUP_URL = "/auth/register";
        public const string CHECK_EMAIL_OVERLAP_URL = "/auth/check/email?email=";

        public NetworkManager networkManager = new NetworkManager();

        public async Task<Response<Nothing>> SignUp(string id, string pw, string name, string email)
        {
            JObject jObject = new JObject();
            jObject["id"] = id;
            jObject["pw"] = Sha512Hash(pw);
            jObject["name"] = name;
            jObject["email"] = email;
            return await networkManager.GetResponse<Nothing>(SIGNUP_URL, Method.POST, jObject.ToString());
        }

        public async Task<Response<Nothing>> CheckEmailOverlap(string email)
        {
            string requestUrl = CHECK_EMAIL_OVERLAP_URL + email;
            return await networkManager.GetResponse<Nothing>(requestUrl, Method.GET, null);
        }

        public void SettingHttpRequest(string serverUrl)
        {
            networkManager.SetHTTPRequestURL(serverUrl, LOGIN_URL, LOGOUT_URL, TOKEN_REFRESH_URL);
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
    }
}
