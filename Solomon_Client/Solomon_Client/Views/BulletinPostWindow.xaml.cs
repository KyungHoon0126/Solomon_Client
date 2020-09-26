using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Solomon_Client.Views
{
    /// <summary>
    /// Interaction logic for BulletinPostWindow.xaml
    /// </summary>
    public partial class BulletinPostWindow : Window
    {
        public delegate void OnModalBackgroundVisibility();
        public event OnModalBackgroundVisibility ModalBackGroundVisibility;

        public string strName, imageName;

        public BulletinPostWindow()
        {
            InitializeComponent();
            Loaded += BulletinPostWindow_Loaded;
        }

        private void BulletinPostWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bulletinData.bulletinViewModel;
            App.bulletinData.bulletinViewModel.BulletinPostResultReceived += BulletinViewModel_BulletinPostResultReceived;
            btnWrite.IsEnabled = false;
        }

        private void BulletinViewModel_BulletinPostResultReceived(object sender)
        {
            this.Close();
            ModalBackGroundVisibility?.Invoke();
        }

        private void btnClosePostWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            App.bulletinData.bulletinViewModel.ClearWritePostDatas();
            ModalBackGroundVisibility?.Invoke();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CheckEmpty())
            {
                btnWrite.IsEnabled = true;
            }
            else
            {
                btnWrite.IsEnabled = false;
            }
        }

        private bool CheckEmpty()
        {
            string id = tbTitle.Text.Trim();
            string pw = tbContent.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
            {
                return true;
            }
            return false;
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "ImageFile(*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*gif";
                fldlg.Filter = "File(*)|*";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    uploadImg.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString());
            }
        }
    }
}
