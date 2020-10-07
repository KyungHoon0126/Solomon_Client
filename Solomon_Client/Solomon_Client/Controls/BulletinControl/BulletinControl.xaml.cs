using System;
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

        public delegate void LoadBulletinWithCommentWindowHandler(object sender, RoutedEventArgs e);
        public event LoadBulletinWithCommentWindowHandler OnLoadBulletinWithComment;

        public BulletinControl()
        {
            InitializeComponent();
            Loaded += BulletinControl_Loaded;
        }

        public string GetTitle()
        {
            return Title;
        }

        private void BulletinControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bulletinData.bulletinViewModel;
        }

        public async void LoadDataAsync()
        {
            await App.bulletinData.LoadDataAsync();
        }

        private void btnCommentCount_Click(object sender, RoutedEventArgs e)
        {
            OnLoadBulletinWithComment?.Invoke((sender as Button).Tag, e);
        }

        private void btnWriteBulletin_Click(object sender, RoutedEventArgs e)
        {
            OnLoadBulletinPostWindow?.Invoke(this, e);
        }

        private void btnWriteComment_Click(object sender, RoutedEventArgs e)
        {
            // TODO : 댓글입력시에만 작동하도록 => UpdateSourceTrigger 오류 해결 하기.
            App.bulletinData.bulletinViewModel.BulletinIdx = Convert.ToInt32((sender as Button).Tag);
        }
    }
}
