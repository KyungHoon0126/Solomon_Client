using Solomon.Core.Login;
using Solomon.Core.SignUp;
using Solomon_Client.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Solomon_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LoginData loginData = new LoginData();
        public static SignUpData signUpData = new SignUpData();

        public App()
        {
            Setting.Load();
        }
    }
}
