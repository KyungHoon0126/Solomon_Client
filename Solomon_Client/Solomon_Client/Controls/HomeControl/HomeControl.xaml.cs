using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solomon_Client.Controls.HomeControl
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl, IPage
    {
        public const string Title = "Home";

        public HomeControl()
        {
            InitializeComponent();
            Loaded += HomeControl_Loaded;
        }

        public string GetTitle()
        {
            return Title;
        }

        private void HomeControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.homeData.LoadData();
            this.DataContext = App.homeData.homeViewModel;
        }
    }
}
