using Newtonsoft.Json.Linq;
using RestSharp;
using Solomon.Network;
using Solomon.Network.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public NetworkManager networkManager = new NetworkManager();

        /// <summary>
        /// 회원가입 메서드
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pw"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response<Nothing>> SignUp(string id, string pw, string name, string email)
        {
            JObject jObject = new JObject();
            jObject["id"] = id;
            jObject["pw"] = Sha512Hash(pw);
            jObject["name"] = name;
            jObject["email"] = email;
            return await networkManager.GetResponse<Nothing>(SIGNUP_URL, Method.POST, jObject.ToString());
        }

        // TODO : 이메일 중복 확인 메서드 제작.

        // TODO : 전화번호 중복 확인 메서드 제작.

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
