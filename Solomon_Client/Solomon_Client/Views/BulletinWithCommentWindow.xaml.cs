using System;
using System.Windows;
using System.Windows.Controls;

namespace Solomon_Client.Views
{
    /// <summary>
    /// Interaction logic for BulletinWithComment.xaml
    /// </summary>
    public partial class BulletinWithCommentWindow : Window
    {
        public delegate void OnModalBackgroundVisibility();
        public event OnModalBackgroundVisibility ModalBackGroundVisibility;

        public BulletinWithCommentWindow()
        {
            InitializeComponent();
            Loaded += BulletinWithComment_Loaded;
        }

        private void BulletinWithComment_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bulletinData.bulletinViewModel;
            btnWriteComment.DataContext = App.bulletinData.bulletinViewModel.SpecificBulletinIdx;
        }

        private void btnCloseBulletinWithComment_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            App.bulletinData.bulletinViewModel.SpecificBulletinItems.Clear();
            ModalBackGroundVisibility?.Invoke();
        }

        private void btn_WriteComment_Click(object sender, RoutedEventArgs e)
        {
            // TODO : 댓글입력시에만 작동하도록 => UpdateSourceTrigger 오류 해결 하기.
            App.bulletinData.bulletinViewModel.BulletinIdx = Convert.ToInt32((sender as Button).Tag);
        }

        private void btn_BulletinContextMenu_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void mi_DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
