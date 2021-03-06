﻿using Solomon_Client.Common;
using Solomon_Client.Themes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solomon_Client.Controls.OptionControl
{
    /// <summary>
    /// Interaction logic for OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl, IPage
    {
        public const string Title = "Option";

        public OptionControl()
        {
            InitializeComponent();
            OnOptionControlLoad();
        }

        public string GetTitle()
        {
            return Title;
        }

        private void OnOptionControlLoad()
        {
            tscSetAutoLogin.Toggled = Setting.IsAutoLogin;
        }

        public void OptionSynchronization()
        {
            OnOptionControlLoad();
        }

        private void tscSetAutoLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Setting.IsAutoLogin = tscSetAutoLogin.Toggled;
            if (Setting.IsAutoLogin == true)
            {
                Setting.SaveUserdata(App.loginData.loginViewModel.Id, App.loginData.loginViewModel.Password);
            }
            else
            {
                Setting.SaveUserData(App.loginData.loginViewModel.Id);
            }
            Setting.Save();
        }

        private void tscSetDarkMode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //mainWindow.Background = new SolidColorBrush(Color.FromRgb(26, 26, 26));
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0: ThemesController.SetTheme(ThemesController.ThemeTypes.Light); break;
                case 1: ThemesController.SetTheme(ThemesController.ThemeTypes.ColourfulLight); break;
                case 2: ThemesController.SetTheme(ThemesController.ThemeTypes.Dark); break;
                case 3: ThemesController.SetTheme(ThemesController.ThemeTypes.ColourfulDark); break;
            }
            e.Handled = true;
        }
    }
}
