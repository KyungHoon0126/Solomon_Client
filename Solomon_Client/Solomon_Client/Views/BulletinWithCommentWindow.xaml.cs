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
            // TODO : 전체 BulletinData를 Clear하고 Load하는게 아니라 게시글의 Count만 동기화 하도록 변경.
            App.bulletinData.bulletinViewModel.CommentIdx = Convert.ToInt32((sender as Button).Tag);
        }
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get 
            {
                return (object)GetValue(DataProperty); 
            }
            set 
            {
                SetValue(DataProperty, value); 
            }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));
    }
}
