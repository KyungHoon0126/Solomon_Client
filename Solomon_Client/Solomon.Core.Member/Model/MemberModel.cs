using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace Solomon.Core.Member.Model
{
    public class MemberModel : BindableBase, ICloneable
    {
        private int _memberIdx;
        [JsonProperty("member_idx")]
        public int MemberIdx
        {
            get => _memberIdx;
            set => SetProperty(ref _memberIdx, value);
        }

        private string _id;
        [JsonProperty("id")]
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email;
        [JsonProperty("email")]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private int _birthYear;
        [JsonProperty("birth_year")]
        public int BirthYear
        {
            get => _birthYear;
            set => SetProperty(ref _birthYear, value);
        }

        private string _gender;
        [JsonProperty("gender")]
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public object Clone()
        {
            return new MemberModel
            {
                MemberIdx = this.MemberIdx,
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                BirthYear = this.BirthYear,
                Gender = this.Gender
            };
        }
    }
}
