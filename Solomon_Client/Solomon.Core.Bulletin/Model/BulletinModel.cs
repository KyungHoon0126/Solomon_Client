using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Windows.Media.Imaging;

namespace Solomon.Core.Bulletin.Model
{
    public class BulletinModel : BindableBase, ICloneable
    {
        private int _bulletinIdx;
        [JsonProperty("bulletin_idx")]
        public int BulletinIdx
        {
            get => _bulletinIdx;
            set
            {
                SetProperty(ref _bulletinIdx, value);
            }
        }

        private string _title;
        [JsonProperty("title")]
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
            }
        }

        private string _content;
        [JsonProperty("content")]
        public string Content
        {
            get => _content;
            set
            {
                SetProperty(ref _content, value);
            }
        }

        private string _writer;
        [JsonProperty("writer")]
        public string Writer
        {
            get => _writer;
            set
            {
                SetProperty(ref _writer, value);
            }
        }

        private DateTime _writtenTime;
        [JsonProperty("written_time")]
        public DateTime WrittenTime
        {
            get => _writtenTime;
            set
            {
                SetProperty(ref _writtenTime, value);
            }
        }

        [JsonIgnore]
        private BitmapImage _bulletinImage;
        public BitmapImage BulletinImage
        {
            get => _bulletinImage;
            set
            {
                SetProperty(ref _bulletinImage, value);
            }
        }

        public object Clone()
        {
            return new BulletinModel
            {
                BulletinIdx = this.BulletinIdx,
                Title = this.Title,
                Content = this.Content,
                Writer = this.Writer,
                WrittenTime = this.WrittenTime,
                BulletinImage = this.BulletinImage
            };
        }
    }
}
