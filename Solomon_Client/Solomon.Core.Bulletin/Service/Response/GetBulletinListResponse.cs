using Newtonsoft.Json;
using Prism.Mvvm;
using Solomon.Core.Bulletin.Model;
using System.Collections.Generic;

namespace Solomon.Core.Bulletin.Service.Response
{
    public class GetBulletinListResponse : BindableBase
    {
        private List<BulletinModel> _bulletins;
        [JsonProperty("bulletins")]
        public List<BulletinModel> Bulletins
        {
            get => _bulletins;
            set
            {
                SetProperty(ref _bulletins, value);
            }
        }
    }
}
