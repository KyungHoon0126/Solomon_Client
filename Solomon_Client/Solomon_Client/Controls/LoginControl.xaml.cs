using Solomon.Core.Login.ViewModel;
using Solomon_Client.Common;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solomon_Client.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private bool isAutoLogin = false;
        private LoginViewModel loginViewModel;

        public delegate void OnSignUpRecievedHandler(object sender, RoutedEventArgs e);
        public event OnSignUpRecievedHandler SignUpResultReceived;

        public delegate void OnLoginResultRecievedHandler(object sender, bool success);
        public event OnLoginResultRecievedHandler LoginResultRecieved;

        public LoginControl()
        {
            InitializeComponent();
            this.Loaded += LoginControl_Loaded;
        }

        private void LoginControl_Loaded(object sender, RoutedEventArgs e)
        {
            loginViewModel = App.loginData.loginViewModel;
            loginViewModel.ServerAddress = ComDef.SERVER_ADDRESS;
            CheckAutoLoginAsync();
            loginViewModel.OnLoginResultRecieved += LoginViewModel_OnLoginResultRecieved;
            this.DataContext = loginViewModel;
        }

        private void LoginViewModel_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                Setting.SaveUserdata(loginViewModel.Id, loginViewModel.Password);
                Setting.Save();
                App.bulletinData.bulletinViewModel.Writer = loginViewModel.Id;
            }

            SetUserData(loginViewModel.Id, loginViewModel.Password);
            isAutoLogin = Setting.IsAutoLogin ? true : false;
            LoginResultRecieved?.Invoke(this, success);
        }

        private void SetUserData(string id, string pw)
        {
            if (isAutoLogin)
            {
                Setting.SaveUserdata(id, pw);
            }
            else
            {
                Setting.SaveUserData(id);
            }

            Setting.IsAutoLogin = isAutoLogin;
            Setting.Save();
        }

        public async Task CheckAutoLoginAsync()
        {
            string id = Setting.GetUserId();
            isAutoLogin = Setting.IsAutoLogin;
            cbAutoLogin.IsChecked = isAutoLogin;
            loginViewModel.Id = id;
            
            string pw = Setting.GetUserPw();
            pbPw.Password = pw;

            if (isAutoLogin)
            {
                loginViewModel.Password = pw;
                App.loginData.Login();
            }
            else if (!string.IsNullOrEmpty(id))
            {
                pbPw.Focus();
            }
            else
            {
                tbId.Focus();
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && btnLogin.IsEnabled)
            {
                App.loginData.Login();
            }
        }

        private void tb_TextChanged(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = !CheckEmpty() ? true : false;
        }

        private bool CheckEmpty()
        {
            return (string.IsNullOrEmpty(tbId.Text.Trim()) || string.IsNullOrEmpty(pbPw.Password.Trim())) ? true : false;
        }

        private void CbAutologin_Checked(object sender, EventArgs e)
        {
            isAutoLogin = ((sender as CheckBox).IsChecked ?? false) ? true : false;
        }

        private void tbSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpResultReceived?.Invoke(this, e);
        }
    }

    #region PasswordBoxMonitor
    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));


        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
    #endregion

    #region PasswordHelper
    public class PasswordHelper : DependencyObject
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
    #endregion
}
