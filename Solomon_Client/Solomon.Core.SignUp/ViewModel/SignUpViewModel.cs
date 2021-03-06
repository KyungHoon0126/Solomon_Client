﻿using Prism.Commands;
using Prism.Mvvm;
using Solomon.Core.SignUp.Common;
using Solomon.Core.SignUp.Service;
using Solomon.Network.Data;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;

namespace Solomon.Core.SignUp.ViewModel
{
    public class SignUpViewModel : BindableBase
    {
        SignUpService signUpService = new SignUpService();

        private bool CheckPasswordLength = false;
        private bool CheckEmailOverLap = false;

        #region Delegate & Event
        public delegate void OnSignUpResultReceivedHandler(Response<Nothing> signUpArgs);
        public event OnSignUpResultReceivedHandler SignUpResultRecieved;
        #endregion

        #region Properties
        private string _inputId;
        public string InputId
        {
            get => _inputId;
            set
            {
                SetProperty(ref _inputId, value);
            }
        }

        private string _inputPw;
        public string InputPw
        {
            get => _inputPw;
            set
            {
                SetProperty(ref _inputPw, value);
                PasswordValidation(value);
            }
        }

        private string _inputPwAgain;
        public string InputPwAgain
        {
            get => _inputPwAgain;
            set
            {
                SetProperty(ref _inputPwAgain, value);
                PasswordValidation(value);

                if (CheckPasswordLength)
                {
                    if (InputPwAgain == InputPw)
                    {
                        SetDescProperty("Pw", "This password is available.", ConvertHexToRgb("#1d65ff"));
                    }
                    else
                    {
                        SetDescProperty("Pw", "The passowrd does not match.", Brushes.Red);
                    }
                }
            }
        }

        private void PasswordValidation(string value)
        {
            if (value.Length >= 8 && value.Length <= 20)
            {
                PwDesc = string.Empty;
                CheckPasswordLength = true;

                if (InputPw != InputPwAgain)
                {
                    SetDescProperty("Pw", "The passowrd does not match.", Brushes.Red);
                }
            }
            else
            {
                SetDescProperty("Pw", "Please verify password length.", Brushes.Red);
            }
        }

        private string _inputName;
        public string InputName
        {
            get => _inputName;
            set
            {
                SetProperty(ref _inputName, value);
            }
        }

        private string _inputEmail;
        public string InputEmail
        {
            get => _inputEmail;
            set
            {
                SetProperty(ref _inputEmail, value);
                CheckEmailOverlap();
            }
        }

        //private string _serverAddress;
        //public string ServerAddress
        //{
        //    get => _serverAddress;
        //    set
        //    {
        //        SetProperty(ref _serverAddress, value);
        //    }
        //}

        private bool _isEnable = true;
        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                SetProperty(ref _isEnable, value);
            }
        }

        // 패스워드 Description
        private string _pwDesc;
        public string PwDesc
        {
            get => _pwDesc;
            set
            {
                SetProperty(ref _pwDesc, value);
            }
        }

        private Brush _pwDescForeground;
        public Brush PwDescForeground
        {
            get => _pwDescForeground;
            set
            {
                SetProperty(ref _pwDescForeground, value);
            }
        }

        // 이메일 Description
        private string _eamilDesc;
        public string EmailDesc
        {
            get => _eamilDesc;
            set
            {
                SetProperty(ref _eamilDesc, value);
            }
        }

        private Brush _emailDescForeground;
        public Brush EmailDescForeground
        {
            get => _emailDescForeground;
            set
            {
                SetProperty(ref _emailDescForeground, value);
            }
        }

        private string _birthYear;
        public string BirthYear
        {
            get => _birthYear;
            set => SetProperty(ref _birthYear, value);
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        #endregion

        #region Command
        public ICommand SignUpCommand { get; set; }
        #endregion

        public SignUpViewModel()
        {
            SignUpCommand = new DelegateCommand(OnSignUp, CanSignUp).ObservesProperty(() => InputEmail);
        }

        private Brush ConvertHexToRgb(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        private bool CanSignUp()
        {
            return (InputEmail != null) && (InputEmail != null) && (InputEmail != null) && (InputEmail != null) && (Gender != null);
        }

        private void OnSignUp()
        {
            IsEnable = false;
            SignUp();
            IsEnable = true;
        }

        private bool IsValidSignUpInformation()
        {
            return (InputId != null && InputPw != null && InputPwAgain != null && InputName != null && InputEmail != null && BirthYear.ToString().Length > 0 && Gender != null);
        }

        private async void SignUp()
        {
            Response<Nothing> signUpArgs = null;

            if (CheckEmailOverLap && IsValidSignUpInformation())
            {
                try
                {
                    signUpService.SettingHttpRequest(ComDef.SERVER_ADDRESS);
                    signUpArgs = await signUpService.SignUp(InputId, InputPw, InputName, InputEmail, int.Parse(BirthYear), Gender);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("SIGN UP ERROR : " + e.Message);
                }
            
                SignUpResultRecieved?.Invoke(signUpArgs);
            }
        }

        private void SetDescProperty(string property, string desc, Brush color)
        {
            if (property == "Pw")
            {
                PwDesc = desc;
                PwDescForeground = color;
            }
            else
            {
                EmailDesc = desc;
                EmailDescForeground = color;
            }
        }

        private bool IsValidEmailAddress(string email)
        {
            Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            Match match = regex.Match(email);

            if (match.Success)
            {
                return true;
            }
            else
            {
                SetDescProperty("Email", "Invalid Email format.", Brushes.Red);
                return false;
            }
        }

        private async void CheckEmailOverlap()
        {
            if (IsValidEmailAddress(InputEmail))
            {
                try
                {
                    signUpService.SettingHttpRequest(ComDef.SERVER_ADDRESS);
                    var resp = await signUpService.CheckEmailOverlap(InputEmail);
                    if (resp.Status == (int)HttpStatusCode.Conflict)
                    {
                        CheckEmailOverLap = false;
                        SetDescProperty("Email", "Duplicate Email.", Brushes.Red);
                    }
                    else
                    {
                        CheckEmailOverLap = true;
                        SetDescProperty("Email", "This email address is available.", ConvertHexToRgb("#1d65ff"));
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void InitVariables()
        {
            InputId = string.Empty;
            InputPw = string.Empty;
            InputPwAgain = string.Empty;
            InputName = string.Empty;
            InputEmail = string.Empty;
            PwDesc = string.Empty;
            EmailDesc = string.Empty;
            BirthYear = "";
            Gender = string.Empty;
        }
    }
}
