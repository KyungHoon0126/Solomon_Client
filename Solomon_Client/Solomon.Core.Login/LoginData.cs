using Solomon.Core.Login.Service;
using Solomon.Core.Login.ViewModel;

namespace Solomon.Core.Login
{
    public class LoginData
    {
        public LoginViewModel loginViewModel;

        public LoginData(LoginService loginService)
        {
            loginViewModel = new LoginViewModel(loginService);
        }

        public void Login()
        {
            loginViewModel.OnLogin();
        }
    }
}
