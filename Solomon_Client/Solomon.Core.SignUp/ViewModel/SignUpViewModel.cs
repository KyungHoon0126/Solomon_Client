using Prism.Commands;
using Prism.Mvvm;
using Solomon.Core.SignUp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solomon.Core.SignUp.ViewModel
{
    public class SignUpViewModel : BindableBase
    {
        SignUpService signUpService = new SignUpService();

        #region Properties
        private string inputId;
        public string InputId
        {
            get => inputId;
            set
            {
                SetProperty(ref inputId, value);
            }
        }

        private string inputPw;
        public string InputPw
        {
            get => inputPw;
            set
            {
                SetProperty(ref inputPw, value);
            }
        }

        private string inputPwAgain;
        public string InputPwAgain
        {
            get => inputPwAgain;
            set
            {
                SetProperty(ref inputPwAgain, value);
            }
        }

        private string inputName;
        public string InputName
        {
            get => inputName;
            set
            {
                SetProperty(ref inputName, value);
            }
        }

        private string inputEmail;
        public string InputEmail
        {
            get => inputEmail;
            set
            {
                SetProperty(ref inputEmail, value);
            }
        }

        private string serverAddress;
        public string ServerAddress
        {
            get => serverAddress;
            set
            {
                SetProperty(ref serverAddress, value);
            }
        }

        private bool isEnable = true;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                SetProperty(ref isEnable, value);
            }
        }
        #endregion

        #region Command
        public ICommand SignUpCommand { get; set; }
        #endregion

        public SignUpViewModel()
        {
            SignUpCommand = new DelegateCommand(OnSignUp, CanSignUp).ObservesProperty(() => InputEmail);
        }

        private bool CanSignUp()
        {
            return (InputEmail != null) && (InputEmail != null) && (InputEmail != null) && (InputEmail != null);
        }

        private void OnSignUp()
        {

        }

        private void SignUp()
        {
            IsEnable = false;



            IsEnable = true;
        }
    }
}
