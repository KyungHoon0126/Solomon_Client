using Microsoft.Win32;
using Solomon.Core.Bulletin.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Solomon_Client.Views
{
    /// <summary>
    /// Interaction logic for BulletinPostWindow.xaml
    /// </summary>
    public partial class BulletinPostWindow : Window
    {
        public delegate void OnModalBackgroundVisibility();
        public event OnModalBackgroundVisibility ModalBackGroundVisibility;

        public BulletinPostWindow()
        {
            InitializeComponent();
            Loaded += BulletinPostWindow_Loaded;
        }

        private void BulletinPostWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bulletinData.bulletinViewModel;
            App.bulletinData.bulletinViewModel.BulletinPostResultReceived += BulletinViewModel_BulletinPostResultReceived;
            btnWrite.IsEnabled = false;
        }

        private void BulletinViewModel_BulletinPostResultReceived(object sender)
        {
            this.Close();
            ModalBackGroundVisibility?.Invoke();
        }

        private void btnClosePostWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            App.bulletinData.bulletinViewModel.ClearWritePostDatas();
            ModalBackGroundVisibility?.Invoke();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CheckEmpty())
            {
                btnWrite.IsEnabled = true;
            }
            else
            {
                btnWrite.IsEnabled = false;
            }
        }

        private bool CheckEmpty()
        {
            string id = tbTitle.Text.Trim();
            string pw = tbContent.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
            {
                return true;
            }
            return false;
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "ImageFile(*.jpg;*.bmp;*.gif;*png)|*.jpg;*.bmp;*gif*png";
                fldlg.ShowDialog();
                {
                    App.bulletinData.bulletinViewModel.BulletinImgPath = fldlg.FileName;
                    App.bulletinData.bulletinViewModel.BulletinImgName = fldlg.SafeFileName;
                    
                    ImageSourceConverter isc = new ImageSourceConverter();
                    uploadImg.SetValue(Image.SourceProperty, isc.ConvertFromString(fldlg.FileName));
                }
                fldlg = null;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message.ToString());
            }
        }
    }


    public class WatermarkBehavior : Behavior<ComboBox>
    {
        private WaterMarkAdorner adorner;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WatermarkBehavior), new PropertyMetadata("Watermark"));


        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(WatermarkBehavior), new PropertyMetadata(12.0));


        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(WatermarkBehavior), new PropertyMetadata(Brushes.Black));



        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(string), typeof(WatermarkBehavior), new PropertyMetadata("Segoe UI"));



        protected override void OnAttached()
        {
            adorner = new WaterMarkAdorner(this.AssociatedObject, this.Text, this.FontSize, this.FontFamily, this.Foreground);

            this.AssociatedObject.Loaded += this.OnLoaded;
            this.AssociatedObject.GotFocus += this.OnFocus;
            this.AssociatedObject.LostFocus += this.OnLostFocus;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.AssociatedObject.IsFocused)
            {
                if (String.IsNullOrEmpty(this.AssociatedObject.Text))
                {
                    var layer = AdornerLayer.GetAdornerLayer(this.AssociatedObject);
                    layer.Add(adorner);
                }
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.AssociatedObject.Text))
            {
                try
                {
                    var layer = AdornerLayer.GetAdornerLayer(this.AssociatedObject);
                    layer.Add(adorner);
                }
                catch { }
            }
        }

        private void OnFocus(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(this.AssociatedObject);
            layer.Remove(adorner);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        public class WaterMarkAdorner : Adorner
        {
            private string text;
            private double fontSize;
            private string fontFamily;
            private Brush foreground;

            public WaterMarkAdorner(UIElement element, string text, double fontsize, string font, Brush foreground)
                : base(element)
            {
                this.IsHitTestVisible = false;
                this.Opacity = 0.6;
                this.text = text;
                this.fontSize = fontsize;
                this.fontFamily = font;
                this.foreground = foreground;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);
                var text = new FormattedText(
                        this.text,
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(fontFamily),
                        fontSize,
                        foreground);

                drawingContext.DrawText(text, new Point(3, 3));
            }
        }
    }
}
