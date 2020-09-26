using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using Solomon.Core.Bulletin.Model;
using Solomon.Core.Bulletin.Service.Response;
using Solomon.Network;
using Solomon.Network.Data;

namespace Solomon.Core.Bulletin.Test
{
    [TestClass]
    public class UnitTest1
    {
        private NetworkManager networkManager = new NetworkManager();

        private const string BULLETIN_URL = "/bulletin/post";

        public static bool WRITE_RESULT = false;
        public static bool DELETE_RESULT = false;
        public static bool PUT_RESULT = false;

        [TestMethod]
        public async void TestMethod()
        {
            Assert.IsNotNull(await GetBulletinList());
            
            await WriteBulletin("TestTitle", "TestContent", "TestWriter");
            Assert.IsTrue(UnitTest1.WRITE_RESULT);
            
            await PutBulletin(5, "TestTitleUpdate", "TestContentUpdate", "TesterUpdate");
            Assert.IsTrue(UnitTest1.PUT_RESULT);

            Assert.IsNotNull(await GetSpecificBulletin(5));
            
            await DeleteBulletin(5, "TesterUpdate");
            Assert.IsTrue(UnitTest1.DELETE_RESULT);
        }

        public async Task<Response<GetBulletinListResponse>> GetBulletinList()
        {
            var resp = await networkManager.GetResponse<GetBulletinListResponse>(UnitTest1.BULLETIN_URL, Method.GET);
            return resp;
        }

        public async Task<Response<Nothing>> WriteBulletin(string title, string content, string writer)
        {
            JObject jObject = new JObject();
            jObject["title"] = title;
            jObject["content"] = content;
            jObject["writer"] = writer;
            var resp = await networkManager.GetResponse<Nothing>(UnitTest1.BULLETIN_URL, Method.POST, jObject.ToString());
            if (resp.Status == (int)(HttpStatusCode.OK))
            {
                UnitTest1.WRITE_RESULT = true;
            }
            else
            {
                UnitTest1.WRITE_RESULT = false;
            }
            return resp;
        }

        public async Task<Response<Nothing>> PutBulletin(int bulletinIdx, string title, string content, string writer)
        {
            JObject jObject = new JObject();
            jObject["bulletin_idx"] = bulletinIdx;
            jObject["title"] = title;
            jObject["content"] = content;
            jObject["writer"] = writer;
            var resp = await networkManager.GetResponse<Nothing>(UnitTest1.BULLETIN_URL, Method.PUT, jObject.ToString());
            if (resp.Status == (int)(HttpStatusCode.OK))
            {
                UnitTest1.PUT_RESULT = true;
            }
            else
            {
                UnitTest1.PUT_RESULT = false;
            }
            return resp;

        }
        public async Task<Response<Nothing>> DeleteBulletin(int bulletinIdx, string writer)
        {
            JObject jObject = new JObject();
            jObject["bulletin_idx"] = bulletinIdx;
            jObject["writer"] = writer;
            var resp = await networkManager.GetResponse<Nothing>(BULLETIN_URL, Method.DELETE, jObject.ToString());
            if (resp.Status == (int)(HttpStatusCode.OK))
            {
                UnitTest1.DELETE_RESULT = true;
            }
            else
            {
                UnitTest1.DELETE_RESULT = false;
            }
            return resp;
        }

        public async Task<Response<BulletinModel>> GetSpecificBulletin(int bulletinIdx)
        {
            JObject jObject = new JObject();
            jObject["bulletin_idx"] = bulletinIdx;
            var resp = await networkManager.GetResponse<BulletinModel>(UnitTest1.BULLETIN_URL, Method.GET, jObject.ToString());
            return resp;
        }
    }
}
