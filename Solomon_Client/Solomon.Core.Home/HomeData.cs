using Solomon.Core.Home.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solomon.Core.Home
{
    public class HomeData
    {
        public HomeViewModel homeViewModel = new HomeViewModel();

        public void LoadData()
        {
            homeViewModel.LoadChartDatas();
        }
    }
}
