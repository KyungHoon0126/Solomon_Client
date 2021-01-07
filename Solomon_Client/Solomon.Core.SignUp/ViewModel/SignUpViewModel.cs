using System;
using System.Net;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Prism.Commands;
using Prism.Mvvm;
using Solomon.Network.Data;
using Solomon.Core.SignUp.Common;
using Solomon.Core.SignUp.Service;

namespace Solomon.Core.SignUp.ViewModel
{
    public class SignUpViewModel : BindableBase
    {
        SignUpService signUpService = new SignUpService();

        private bool IsValidPasswordLength = false;
        private bool IsValidEmail = false;

        public delegate void OnSignUpResultReceivedHandler(Response<Nothing> signUpArgs);
        public event OnSignUpResultReceivedHandler SignUpResultRecieved;

        #region Properties
        private string _inputId;
        public string InputId
        {
            get => _inputId;
            set => SetProperty(ref _inputId, value);
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

                if (IsValidPasswordLength)
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
                IsValidPasswordLength = true;

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
            set => SetProperty(ref _inputName, value);
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

        private bool _isEnable = true;
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }

        private string _pwDesc;
        public string PwDesc
        {
            get => _pwDesc;
            set => SetProperty(ref _pwDesc, value);
        }

        private Brush _pwDescForeground;
        public Brush PwDescForeground
        {
            get => _pwDescForeground;
            set => SetProperty(ref _pwDescForeground, value);
        }

        private string _eamilDesc;
        public string EmailDesc
        {
            get => _eamilDesc;
            set => SetProperty(ref _eamilDesc, value);
        }

        private Brush _emailDescForeground;
        public Brush EmailDescForeground
        {
            get => _emailDescForeground;
            set => SetProperty(ref _emailDescForeground, value);
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

        #region Constructor
        public SignUpViewModel()
        {
            SignUpCommand = new DelegateCommand(OnSignUp, CanSignUp).ObservesProperty(() => InputEmail);
        }
        #endregion

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

        private Brush ConvertHexToRgb(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        #region Command Method
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
        #endregion

        private bool IsValidSignUpInformation()
        {
            return (InputId != null && InputPw != null && InputPwAgain != null && InputName != null && InputEmail != null && BirthYear.ToString().Length > 0 && Gender != null);
        }

        private async void SignUp()
        {
            Response<Nothing> signUpArgs = null;

            if (IsValidEmail && IsValidSignUpInformation())
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
                        IsValidEmail = false;
                        SetDescProperty("Email", "Duplicate Email.", Brushes.Red);
                    }
                    else
                    {
                        IsValidEmail = true;
                        SetDescProperty("Email", "This email address is available.", ConvertHexToRgb("#1d65ff"));
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        internal void InitVariables()
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
