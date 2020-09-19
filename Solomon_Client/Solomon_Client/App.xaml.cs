using Solomon.Core.Bulletin;
using Solomon.Core.Login;
using Solomon.Core.SignUp;
using Solomon_Client.Common;
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
        public static BulletinData bulletinData = new BulletinData();

        public App()
        {
            Setting.Load();
        }
    }
}
