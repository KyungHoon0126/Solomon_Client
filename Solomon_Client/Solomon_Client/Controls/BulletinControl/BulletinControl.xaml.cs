using System.Windows;
using System.Windows.Controls;

namespace Solomon_Client.Controls.BulletinControl
{
    /// <summary>
    /// Interaction logic for BulletinControl.xaml
    /// </summary>
    public partial class BulletinControl : UserControl, IPage
    {
        public const string Title = "Bulletin";

        public delegate void LoadBulletinPostWindowHandler(object sender, RoutedEventArgs e);
        public event LoadBulletinPostWindowHandler OnLoadBulletinPostWindow;

        public BulletinControl()
        {
            InitializeComponent();
            Loaded += BulletinControl_Loaded;
        }

        private void BulletinControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bulletinData.bulletinViewModel;
        }

        public string GetTitle()
        {
            return Title;
        }

        public async void LoadDataAsync()
        {
            await App.bulletinData.LoadDataAsync();
        }

        private void btnWriteBulletin_Click(object sender, RoutedEventArgs e)
        {
            OnLoadBulletinPostWindow?.Invoke(this, e);
        }

        private void btnBulletinReplyCount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
