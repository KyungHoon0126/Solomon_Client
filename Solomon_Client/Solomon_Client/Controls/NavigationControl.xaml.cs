using Solomon_Client.Common;
using Solomon_Client.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Solomon_Client.Controls
{
    /// <summary>
    /// Interaction logic for NavigationControl.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {
        List<NaviData> naviDatas = new List<NaviData>();
        List<string> naviDataImages = new List<string>();

        public NavigationControl()
        {
            InitializeComponent();
            Loaded += NavigationControl_Loaded;
            ctrlBulletin.OnLoadBulletinPostWindow += CtrlBulletin_OnLoadBulletinPostWindow;
        }

        private void NavigationControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetNaviDatas();
        }

        private void CtrlBulletin_OnLoadBulletinPostWindow(object sender, RoutedEventArgs e)
        {
            BulletinPostWindow bambooPostWindow = new BulletinPostWindow();
            bambooPostWindow.ModalBackGroundVisibility += BambooPostWindow_ModalBackGroundVisibility; 
            ncModalBackGround.Visibility = Visibility.Visible; 
            bambooPostWindow.ShowDialog();
        }

        private void BambooPostWindow_ModalBackGroundVisibility()
        {
            ncModalBackGround.Visibility = Visibility.Collapsed;
        }

        private void SetNaviDatas()
        {
            #region LoadNaviDatas
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
            #endregion

            // TODO : 나중에 Converter로 바꾸기.
            // NaviDataImages for Converter
            for (int i = 0; i < naviDatas.Count; i++)
            {
                naviDataImages.Add(naviDatas[i].NaviImagePath);
            }
            naviDataImages.Add(ComDef.Path + "ColorHomeIcon.png");
            naviDataImages.Add(ComDef.Path + "ColorBulletinIcon.png");
            naviDataImages.Add(ComDef.Path + "ColorOptionIcon.png");
        }

        private void NaviImageConverter(NaviData naviData)
        {
            int idx = 0;
            for (int i = 0; i < naviDataImages.Count - 3; i++)
            {
                naviDatas[idx].NaviImagePath = naviDataImages[i];
                idx++;
            }
            
            idx = 0;

            switch (naviData.Title)
            {
                case "Home":
                    naviDatas[0].NaviImagePath = naviDataImages[3];
                    break;
                case "Bulletin":
                    naviDatas[1].NaviImagePath = naviDataImages[4];
                    break;
                case "Option":
                    naviDatas[2].NaviImagePath = naviDataImages[5];
                    break;
            }
        }

        public void InitView()
        {
            naviDatas[0].NaviImagePath = naviDataImages[3];
            ShowPage(ctrlBulletin);
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
            ICollectionView view = CollectionViewSource.GetDefaultView(naviDatas); 

            switch (selectData.NaviMenu)
            {
                case NaviMenu.Home:
                    page = ctrlHome;
                    NaviImageConverter(selectData);
                    view.Refresh();
                    break;
                case NaviMenu.Bulletin:
                    page = ctrlBulletin;
                    NaviImageConverter(selectData);
                    ctrlBulletin.LoadDataAsync();
                    view.Refresh();
                    break;
                case NaviMenu.Option:
                    NaviImageConverter(selectData);
                    page = ctrlOption;
                    view.Refresh();
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
