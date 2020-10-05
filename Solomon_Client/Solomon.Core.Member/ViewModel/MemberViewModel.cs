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

        #region Properties
        public MemberModel MemberModel 
        {
            get; 
            set;
        }
        #endregion

        public async Task GetMemberInformation(int member_idx)
        {
            var resp = await memberService.GetMemberInformation(member_idx);

            if (resp != null && resp.Data != null && resp.Status == 200)
            {
                try
                {
                    MemberModel.MemberIdx = resp.Data.MemberIdx;
                    MemberModel.Id = resp.Data.Id;
                    MemberModel.Name = resp.Data.Name;
                    MemberModel.Email = resp.Data.Email;
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
        }
    }
}
