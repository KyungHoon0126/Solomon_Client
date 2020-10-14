using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solomon_Client.Controls.OptionControl
{
    /// <summary>
    /// Interaction logic for ToggleSwitchControl.xaml
    /// </summary>
    public partial class ToggleSwitchControl : UserControl
    {
        Thickness LeftSide = new Thickness(-57, 0, 0, 0);
        Thickness RightSide = new Thickness(0, 0, -57, 0);
        SolidColorBrush Off = new SolidColorBrush(Colors.White); //new SolidColorBrush(Color.FromRgb(160, 160, 160));
        SolidColorBrush On = new SolidColorBrush(Colors.RoyalBlue); //new SolidColorBrush(Color.FromRgb(130, 190, 125));

        private bool _toggled = false;
        public bool Toggled
        {
            get => _toggled;
            set
            {
                _toggled = value;
                SetToggledSwitch();
            }
        }

        public ToggleSwitchControl()
        {
            InitializeComponent();
        }

        private void SetToggledSwitch()
        {
            if (_toggled)
            {
                bdBack.Background = On;
                bdBack.BorderThickness = new Thickness(0);
                epDot.Margin = RightSide;
                epDot.Fill = new SolidColorBrush(Colors.White);
                tbStatus.Text = "ON";
            }
            else
            {
                bdBack.Background = Off;
                bdBack.BorderThickness = new Thickness(4);
                epDot.Margin = LeftSide;
                epDot.Fill = new SolidColorBrush(Colors.Black);
                tbStatus.Text = "OFF";
            }
        }

        private void bdBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Toggled = !Toggled;
        }

        private void epDot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Toggled = !Toggled;
        }
    }
}
