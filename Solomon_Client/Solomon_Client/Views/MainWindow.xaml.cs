using Solomon.Network.Data;
using System.Net;
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
            CtrlLogin.SignUpReceived += CtrlLogin_SignUpReceived;
            App.signUpData.signUpViewModel.SignUpResultRecieved += this.SignUpViewModel_SignUpResultRecieved;
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
            App.signUpData.signUpViewModel.InitVariables();
        }

        private void SignUpViewModel_SignUpResultRecieved(Response<Nothing> signUpArgs)
        {
            if (signUpArgs.Status == (int)HttpStatusCode.Created)
            {
                CtrlSignup.Visibility = Visibility.Collapsed;
                MessageBox.Show("회원가입을 성공하였습니다.");
                CtrlLogin.Visibility = Visibility.Visible;
                App.signUpData.signUpViewModel.InitVariables();
            }
        }

        private void LoginCtrl_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                CtrlLogin.Visibility = Visibility.Collapsed;
                MessageBox.Show("로그인에 성공하셨습니다.");
                ctrlNavi.Visibility = Visibility.Visible;
                ctrlNavi.InitView();

                //await App.memberData.memberViewModel.LoadDataAsync();
                //await App.mealData.mealViewModel.LoadDataAsync();
            }
        }
    }
}
