﻿using Prism.Commands;
using Prism.Mvvm;
using Solomon.Core.Login.Service;
using Solomon.Network.Data;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Input;

namespace Solomon.Core.Login.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        private LoginService loginService;

        public delegate void OnLoginResultRecievedHandler(object sender, bool success);
        public event OnLoginResultRecievedHandler OnLoginResultRecieved;

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

        #region Command
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Constructor
        public LoginViewModel(LoginService loginService)
        {
            this.loginService = loginService;
            LoginCommand = new DelegateCommand(OnLogin);
            //_btnLoginEnabled = true;
        }
        #endregion

        internal async void OnLogin()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                SendOnLoginResultRecievedEvent(false);

                Desc = "네트워크 상태를 확인해주세요!!";
                return;
            }

            BtnLoginEnabled = false;
            ProgressRingActivated = true;

            Debug.WriteLine($"{_btnLoginEnabled} OnLogin");

            Response<TokenInfo> loginArgs = null;
            
            try
            {
                loginService.SettingHttpRequest(ServerAddress);
                loginArgs = await loginService.Login(Id, Password);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                loginArgs = null;
            }
            
            Debug.WriteLine(loginArgs.Status);

            if (loginArgs == null || loginArgs.Status != (int)HttpStatusCode.OK)  
            {
                SendOnLoginResultRecievedEvent(false);
                Desc = "로그인에 실패하였습니다.";
            }
            else
            {
                SendOnLoginResultRecievedEvent(true);
                Desc = "로그인에 성공하였습니다.";
            }

            BtnLoginEnabled = true;
            ProgressRingActivated = false;
        }

        private void SendOnLoginResultRecievedEvent(bool success)
        {
            OnLoginResultRecieved?.Invoke(this, success);
        }
    }
}
