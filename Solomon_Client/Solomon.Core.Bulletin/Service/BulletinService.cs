using Newtonsoft.Json.Linq;
using RestSharp;
using Solomon.Core.Bulletin.Model;
using Solomon.Core.Bulletin.Service.Response;
using Solomon.Network;
using Solomon.Network.Data;
using System.Threading.Tasks;

namespace Solomon.Core.Bulletin.Service
{
    public class BulletinService
    {
        NetworkManager networkManager = new NetworkManager();

        private const string BULLETIN_URL = "/bulletin/post";

        public async Task<Response<BulletinResponse>> GetBulletinList()
        {
            var resp = await networkManager.GetResponse<BulletinResponse>(BULLETIN_URL, Method.GET);
            return resp;
        }

        public async Task<Response<Nothing>> WriteBulletin(string title, string content, string writer)
        {
            JObject jObject = new JObject();
            jObject["title"] = title;
            jObject["content"] = content;
            jObject["writer"] = writer;
            var resp = await networkManager.GetResponse<Nothing>(BULLETIN_URL, Method.POST, jObject.ToString());
            return resp;
        }

        public async Task<Response<Nothing>> DeleteBulletin(int bulletinIdx, string writer)
        {
            JObject jObject = new JObject();
            jObject["bulletin_idx"] = bulletinIdx;
            jObject["writer"] = writer;
            var resp = await networkManager.GetResponse<Nothing>(BULLETIN_URL, Method.DELETE, jObject.ToString());
            return resp;
        }

        public async Task<Response<Nothing>> PutBulletin(int bulletinIdx, string title, string content, string writer)
        {
            JObject jObject = new JObject();
            jObject["bulletin_idx"] = bulletinIdx;
            jObject["title"] = title;
            jObject["content"] = content;
            jObject["writer"] = writer;
            var resp = await networkManager.GetResponse<Nothing>(BULLETIN_URL, Method.PUT, jObject.ToString());
            return resp;
        }

        public async Task<Response<BulletinModel>> GetSpecificBulletin(int bulletinIdx)
        {
            JObject jObject = new JObject();
            jObject["bulletin_idx"] = bulletinIdx;
            var resp = await networkManager.GetResponse<BulletinModel>(BULLETIN_URL, Method.GET, jObject.ToString());
            return resp;
        }
    }
}
