using Solomon.Core.Bulletin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solomon.Core.Bulletin.ViewModel
{
    public class BulletinViewModel
    {
        BulletinService bulletinService = new BulletinService();

        private async Task GetBulletinList()
        {
            var resp = await bulletinService.GetBulletinList();
        }

        public async Task LoadDataAsync()
        {
            await GetBulletinList();
        }
    }
}
