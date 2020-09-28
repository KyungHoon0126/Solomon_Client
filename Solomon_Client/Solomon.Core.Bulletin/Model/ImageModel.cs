using Newtonsoft.Json;
using Prism.Mvvm;

namespace Solomon.Core.Bulletin.Model
{
    public class ImageModel : BindableBase
    {
        private int _imageIdx;
        [JsonProperty("image_idx")]
        public int ImageIdx
        {
            get => _imageIdx;
            set
            {
                SetProperty(ref _imageIdx, value);
            }
        }

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

        private string _imageName;
        [JsonProperty("img_name")]
        public string ImageName
        {
            get => _imageName;
            set
            {
                SetProperty(ref _imageName, value);
            }
        }

        private byte[] _imgSize;
        [JsonProperty("img_size")]
        public byte[] ImgSize
        {
            get => _imgSize;
            set
            {
                SetProperty(ref _imgSize, value);
            }
        }
    }

    public class DBImageModel
    {
        public int img_idx { get; set; }
        public int bulletin_idx { get; set; }
        public string img_name { get; set; }
        public byte[] img_size { get; set; }
    }
}
