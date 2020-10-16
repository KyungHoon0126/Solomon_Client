using System.Drawing;
using System.Windows;

namespace Solomon.Core.CustomMessageBox
{
    internal partial class CustomMessageBoxWindow : Window
    {
        internal string Caption
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        internal string Message
        {
            get
            {
                return TextBlock_Message.Text;
            }
            set
            {
                TextBlock_Message.Text = value;
            }
        }

        internal string OkButtonText
        {
            get
            {
                return lbOk.Content.ToString();
            }
            set
            {
                lbOk.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string CancelButtonText
        {
            get
            {
                return lbCancel.Content.ToString();
            }
            set
            {
                lbCancel.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string YesButtonText
        {
            get
            {
                return lbYes.Content.ToString();
            }
            set
            {
                lbYes.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string NoButtonText
        {
            get
            {
                return lbNo.Content.ToString();
            }
            set
            {
                lbNo.Content = value.TryAddKeyboardAccellerator();
            }
        }

        public MessageBoxResult Result { get; set; }

        internal CustomMessageBoxWindow(string message)
        {
            InitializeComponent();
            Message = message;
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;
            DisplayButtons(MessageBoxButton.OK);
        }

        internal CustomMessageBoxWindow(string message, string caption)
        {
            InitializeComponent();
            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;
            DisplayButtons(MessageBoxButton.OK);
        }

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button)
        {
            InitializeComponent();
            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;

            DisplayButtons(button);
        }

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxImage image)
        {
            InitializeComponent();
            Message = message;
            Caption = caption;
            DisplayImage(image);
            DisplayButtons(MessageBoxButton.OK);
        }

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            InitializeComponent();
            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;
            DisplayButtons(button);
            DisplayImage(image);
        }

        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    // Hide all but OK, Cancel
                    btnOK.Visibility = Visibility.Visible;
                    btnOK.Focus();
                    btnCancel.Visibility = Visibility.Visible;

                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    // Hide all but Yes, No
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    btnNo.Visibility = Visibility.Visible;

                    btnOK.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNoCancel:
                    // Hide only OK
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;

                    btnOK.Visibility = Visibility.Collapsed;
                    break;
                default:
                    // Hide all but OK
                    btnOK.Visibility = Visibility.Visible;
                    btnOK.Focus();

                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void DisplayImage(MessageBoxImage image)
        {
            Icon icon;

            switch (image)
            {
                case MessageBoxImage.Exclamation:       // Enumeration value 48 - also covers "Warning"
                    icon = SystemIcons.Exclamation;
                    break;
                case MessageBoxImage.Error:             // Enumeration value 16, also covers "Hand" and "Stop"
                    icon = SystemIcons.Hand;
                    break;
                case MessageBoxImage.Information:       // Enumeration value 64 - also covers "Asterisk"
                    icon = SystemIcons.Information;
                    break;
                case MessageBoxImage.Question:
                    icon = SystemIcons.Question;
                    break;
                default:
                    icon = SystemIcons.Information;
                    break;
            }

            Image_MessageBox.Source = icon.ToImageSource();
            Image_MessageBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void ButtonOKClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void ButtonYesClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void ButtonNoClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }
    }
}
