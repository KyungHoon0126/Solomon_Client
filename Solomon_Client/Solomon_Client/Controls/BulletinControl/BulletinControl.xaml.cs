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

namespace Solomon_Client.Controls.BulletinControl
{
    /// <summary>
    /// Interaction logic for BulletinControl.xaml
    /// </summary>
    public partial class BulletinControl : UserControl, IPage
    {
        public const string Title = "Bulletin";

        public BulletinControl()
        {
            InitializeComponent();
        }

        public string GetTitle()
        {
            return Title;
        }

        public async void LoadDataAsync()
        {
            await App.bulletinData.LoadDataAsync();
        }
    }
}
