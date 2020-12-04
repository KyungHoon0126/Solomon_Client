using Prism.Mvvm;
using Solomon.Core.Member.Model;
using Solomon.Core.Member.Service;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solomon.Core.Member.ViewModel
{
    public class MemberViewModel : BindableBase
    {
        MemberService memberService = new MemberService();

        public delegate void OnMessageHandler(string msg);
        public event OnMessageHandler Message;
        
        public async Task<MemberModel> GetMemberInformation(string id)
        {
            var resp = await memberService.GetMemberInformation(id);

            if (resp != null && resp.Data != null && resp.Status == 200)
            {
                try
                {
                    var member = new MemberModel();
                    member.MemberIdx = resp.Data.MemberIdx;
                    member.Id = resp.Data.Id;
                    member.Name = resp.Data.Name;
                    member.Email = resp.Data.Email;
                    return member;
                }
                catch (Exception e)
                {
                    Message?.Invoke(e.Message);
                    Debug.WriteLine("GET MEMBER INFORMATION URL ERROR : " + e.Message);
                }
            }
            else
            {
                Message?.Invoke(resp.Message);
            }

            return new MemberModel();
        }
    }
}
