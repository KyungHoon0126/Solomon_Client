using Solomon.Core.Home.ViewModel;

namespace Solomon.Core.Home
{
    public class HomeData
    {
        public HomeViewModel homeViewModel = new HomeViewModel();

        public void LoadData()
        {
            homeViewModel.LoadGenderRatioDatas();
            homeViewModel.LoadAgeRatioDatas();
        }
    }
}
