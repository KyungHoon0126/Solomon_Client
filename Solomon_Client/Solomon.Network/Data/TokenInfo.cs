using Newtonsoft.Json;

namespace Solomon.Network.Data
{
    public class TokenInfo
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public TokenInfo(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;  
        }

        public TokenInfo()
        {

        }
    }
}
