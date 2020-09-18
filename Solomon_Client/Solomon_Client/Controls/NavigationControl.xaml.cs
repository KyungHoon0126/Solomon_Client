using Solomon_Client.Common;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Solomon_Client.Controls
{
    /// <summary>
    /// Interaction logic for NavigationControl.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {
        List<NaviData> naviDatas = new List<NaviData>();

        public NavigationControl()
        {
            InitializeComponent();
            Loaded += NavigationControl_Loaded;
        }

        private void NavigationControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetNaviDatas();
        }

        private void SetNaviDatas()
        {
            naviDatas.Add(new NaviData
            {
                Title = "Home",
                NaviImagePath = ComDef.Path + "HomeIcon.png",
                NaviMenu = NaviMenu.Home
            });
            naviDatas.Add(new NaviData
            {
                Title = "Bulletin",
                NaviImagePath = ComDef.Path + "BulletinIcon.png",
                NaviMenu = NaviMenu.Bulletin
            });
            naviDatas.Add(new NaviData
            {
                Title = "Option",
                NaviImagePath = ComDef.Path + "OptionIcon.png",
                NaviMenu = NaviMenu.Option
            });

            lvNavi.ItemsSource = naviDatas;
        }

        public void InitView()
        {
            ShowPage(ctrlHome);
        }   

        private void ShowPage(IPage page)
        {
            if (page != null && page is FrameworkElement element)
            {
                CollapsePages();
                element.Visibility = Visibility.Visible;
            }
        }

        private void CollapsePages()
        {
            foreach (FrameworkElement element in gdPage.Children)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        private void lvNavi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IPage page = null;
            NaviData selectData = lvNavi.SelectedItem as NaviData;

            switch (selectData.NaviMenu)
            {
                case NaviMenu.Home:
                    page = ctrlHome;
                    break;
                case NaviMenu.Bulletin:
                    page = ctrlBulletin;
                    ctrlBulletin.LoadDataAsync();
                    break;
                case NaviMenu.Option:
                    page = ctrlOption;
                    break;
            }

            ShowPage(page);
        }
    }

    public class NaviData
    {
        public NaviMenu NaviMenu { get; set; }
        public string Title { get; set; }
        public string NaviImagePath { get; set; }
    }

    public enum NaviMenu
    {
        Home, Bulletin, Option
    }

    interface IPage
    {
        string GetTitle();
    }
}
