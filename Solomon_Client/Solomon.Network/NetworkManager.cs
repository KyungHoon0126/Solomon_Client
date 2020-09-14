using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using Solomon.Network.Common;
using Solomon.Network.Data;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solomon.Network
{
    public partial class NetworkManager : IServerProtocol
    {
        public void SetHTTPRequestURL(string serverUrl, string loginUrl, string logoutUrl, string tokenRefreshUrl, int timeOut = 30000)
        {
            Options.serverUrl = serverUrl;
            Options.loginUrl = loginUrl;
            Options.logoutUrl = logoutUrl;
            Options.tokenRefreshUrl = tokenRefreshUrl;
            Options.tokenInfo = new TokenInfo();
            Options.timeOut = timeOut;
        }

        private static RestRequest CreateRequest(string resource, Method method, string parameterJson, TokenInfo token = null, UrlSegment[] urlSegments = null, Header[] headers = null)
        {
            var restRequest = new RestRequest(resource, method) { Timeout = Options.timeOut };
            restRequest = AddToRequest(restRequest, token, parameterJson, urlSegments, headers);

            return restRequest;
        }

        private static RestRequest AddToRequest(RestRequest restRequest, TokenInfo token, string json = null, UrlSegment[] urlSegments = null, Header[] headers = null)
        {
            if (urlSegments != null)
            {
                foreach (var urlSegment in urlSegments)
                {
                    restRequest.AddUrlSegment(urlSegment.Name, urlSegment.Value);
                }
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    restRequest.AddHeader(header.Name, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(json))
            {
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddParameter("application/json", json, ParameterType.RequestBody);
            }

            if (token != null)
            {
                restRequest.AddHeader("x-access-token", token.Token);
            }
            return restRequest;
        }

        private static T DeserializeSnakeCase<T>(string json)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            try
            {
                var resp = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented,
                });
                return resp;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return default(T);
        }

        private static RestClient CreateClient()
        {
            var restClient = new RestClient(Options.serverUrl) { Timeout = Options.timeOut };
            return restClient;
        }

        public async Task<Response<T>> GetResponse<T>(string resource, Method method, string parameterJson = null, UrlSegment[] urlSegments = null, Header[] headers = null)
        {
            var client = CreateClient();
            var restRequest = CreateRequest(resource, method, parameterJson, Options.tokenInfo, urlSegments, headers);
            var response = await client.ExecuteTaskAsync(restRequest);

            if (CheckTokenExpired(response))
            {
                await TokenRefresh();
                return await GetResponse<T>(resource, method, parameterJson, urlSegments, headers);
            }

            var resp = DeserializeSnakeCase<Response<T>>(response.Content);
            return resp;
        }

        public async Task<IRestResponse> PostFormAsync(string formatFileUrl, string contentType, byte[] formData, string extension)
        {
            var restClient = new RestClient(Options.serverUrl);

            var request = new RestRequest(Options.serverUrl + formatFileUrl, Method.POST);
            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.AddHeader("Content-Type", contentType);

            request.AddHeader("token", Options.tokenInfo.Token);

            request.AddHeader("Content-Length", Convert.ToString(formData.Length));
            byte[] byte1 = formData;

            request.AddFileBytes(extension, byte1, extension);

            var dt = await restClient.ExecuteTaskAsync(request);

            return dt;
        }

        public class UrlSegment
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public UrlSegment(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        public class Header
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Header(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}
