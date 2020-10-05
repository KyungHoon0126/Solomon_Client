using RestSharp;
using Solomon.Core.Member.Model;
using Solomon.Network;
using Solomon.Network.Data;
using System.Threading.Tasks;

namespace Solomon.Core.Member.Service
{
    public class MemberService
    {
        NetworkManager networkManager = new NetworkManager();

        private const string MEMBER_INFORMATION_URL = "/member";

        public async Task<Response<MemberModel>> GetMemberInformation(int member_idx)
        {
            string requestUrl = MEMBER_INFORMATION_URL + "/" + member_idx;
            var resp = await networkManager.GetResponse<MemberModel>(requestUrl, Method.GET);
            return resp;
        }
    }
}
