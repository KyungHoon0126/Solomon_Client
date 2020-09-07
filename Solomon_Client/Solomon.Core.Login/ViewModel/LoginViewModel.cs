using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solomon.Core.Login.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        #region Property
        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set => SetProperty(ref _serverAddress, value.Trim());
        }

        private string _desc;
        public string Desc
        {
            get => _desc;
            set => SetProperty(ref _desc, value);
        }

        private bool _btnLoginEnabled;
        public bool BtnLoginEnabled
        {
            get => _btnLoginEnabled;
            set => SetProperty(ref _btnLoginEnabled, value);
        }

        private bool _progressRingActivated;
        public bool ProgressRingActivated
        {
            get => _progressRingActivated;
            set => SetProperty(ref _progressRingActivated, value);
        }
        #endregion
    }
}
