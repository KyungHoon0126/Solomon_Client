using System.Windows;
using System.Windows.Controls;

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
            this.DataContext = App.bulletinData.bulletinViewModel;
        }
    }
}
