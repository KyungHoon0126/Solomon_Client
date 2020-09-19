using Solomon.Core.Bulletin.ViewModel;
using System.Threading.Tasks;

namespace Solomon.Core.Bulletin
{
    public class BulletinData
    {
        public BulletinViewModel bulletinViewModel = new BulletinViewModel();

        public async Task LoadDataAsync()
        {
            await bulletinViewModel.LoadDataAsync();
        }
    }
}
