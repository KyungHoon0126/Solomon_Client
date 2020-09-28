﻿using Solomon_Client.Common;
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

        public delegate void OnSignUpRecievedHandler(object sender, RoutedEventArgs e);
        public event OnSignUpRecievedHandler SignUpReceived;

        public delegate void OnLoginResultRecievedHandler(object sender, bool success);
        public event OnLoginResultRecievedHandler LoginResultRecieved;

        public LoginControl()
        {
            InitializeComponent();
            this.Loaded += LoginControl_Loaded;
        }

        private void LoginControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.loginData.loginViewModel.ServerAddress = "http://localhost:8080";
            //CheckAutoLoginAsync();
            App.loginData.loginViewModel.OnLoginResultRecieved += LoginViewModel_OnLoginResultRecieved;
            this.DataContext = App.loginData.loginViewModel;
        }

        private void LoginService_Message(string msg)
        {
            MessageBox.Show(msg);
            return;
        }

        private void LoginViewModel_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                Setting.SaveUserdata(App.loginData.loginViewModel.Id, App.loginData.loginViewModel.Password);
                Setting.Save();
                App.bulletinData.bulletinViewModel.BulletinPostWriter = App.loginData.loginViewModel.Id;
            }

            SetUserData(App.loginData.loginViewModel.Id, App.loginData.loginViewModel.Password);

            if (isAutoLogin == true)
            {
                Setting.IsAutoLogin = true;
            }

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
            App.loginData.loginViewModel.Id = id;
            
            string pw = Setting.GetUserPw();
            pbPw.Password = pw;

            if (isAutoLogin)
            {
                App.loginData.loginViewModel.Password = pw;
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

        private void UserControl_KeyDown(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Return && btnLogin.IsEnabled)
            {
                App.loginData.Login();
            }
        }

        private void tb_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty())
            {
                btnLogin.IsEnabled = true;
            }
            else
            {
                btnLogin.IsEnabled = false;
            }
        }

        private bool CheckEmpty()
        {
            string id = tbId.Text.Trim();
            string pw = pbPw.Password.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
            {
                return true;
            }
            return false;
        }

        private void CbAutologin_Checked(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.IsChecked ?? false)
            {
                isAutoLogin = true;
            }
            else
            {
                isAutoLogin = false;
            }
        }

        private void tbSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpReceived?.Invoke(this, e);
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
