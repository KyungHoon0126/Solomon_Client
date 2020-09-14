using Solomon.Core.Login.ViewModel;

namespace Solomon.Core.Login
{
    public class LoginData
    {
        public LoginViewModel loginViewModel = new LoginViewModel();

        public void Login()
        {
            loginViewModel.OnLogin();
        }
    }
}
