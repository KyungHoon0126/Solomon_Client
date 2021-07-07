using Solomon.Network.Data;
using System.Windows;

namespace Solomon_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CtrlLogin.SignUpResultReceived += CtrlLogin_SignUpReceived;
            App.signUpData.signUpViewModel.SignUpResultRecieved += SignUpViewModel_SignUpResultRecieved;
            CtrlSignup.BackWardLoginPage += CtrlSignup_BackWardLoginPage;
        }

        private void CtrlLogin_SignUpReceived(object sender, RoutedEventArgs e)
        {
            CtrlLogin.Visibility = Visibility.Collapsed;
            CtrlSignup.Visibility = Visibility.Visible;
        }

        private void CtrlSignup_BackWardLoginPage(object sender, RoutedEventArgs e)
        {   
            CtrlSignup.Visibility = Visibility.Collapsed;
            CtrlLogin.Visibility = Visibility.Visible;
            InitSignUpData();
        }

        private void SignUpViewModel_SignUpResultRecieved(Response<Nothing> signUpArgs)
        {
            if (signUpArgs.Status == 201)
            {
                CtrlSignup.Visibility = Visibility.Collapsed;
                CtrlLogin.Visibility = Visibility.Visible;
                MessageBox.Show("회원가입에 성공하였습니다.");
                InitSignUpData();
                CtrlSignup.DeselectGender();
            }
        }

        private void LoginCtrl_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                CtrlLogin.Visibility = Visibility.Collapsed;
                ctrlNavi.Visibility = Visibility.Visible;
                ctrlNavi.ctrlBulletin.LoadDataAsync();
                ctrlNavi.InitView();
            }
        }

        private void InitSignUpData()
        {
            App.signUpData.InitVariables();
        }
    }
}
