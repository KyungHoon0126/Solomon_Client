using RestSharp;
using System.Threading.Tasks;

namespace Solomon.Network
{
    public interface IServerProtocol
    {
        bool CheckTokenExpired(IRestResponse response);
        Task TokenRefresh();
    }
}
