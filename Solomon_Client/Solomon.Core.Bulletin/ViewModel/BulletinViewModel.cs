using Dapper;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Solomon.Core.Bulletin.Model;
using Solomon.Core.Bulletin.Service;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Solomon.Core.Bulletin.ViewModel
{
    public class BulletinViewModel : BindableBase
    {
        BulletinService bulletinService = new BulletinService();

        public delegate void OnMessageHandler(string msg);
        public event OnMessageHandler Message;

        public delegate void BulletinPostResultReceivedHandler(object sender);
        public event BulletinPostResultReceivedHandler BulletinPostResultReceived;

        #region Properties
        private ObservableCollection<BulletinModel> _tempBulletinItems;
        public ObservableCollection<BulletinModel> TempBulletinItems
        {
            get => _tempBulletinItems;
            set => SetProperty(ref _tempBulletinItems, value);
        }

        private ObservableCollection<BulletinModel> _bulletinItems;
        public ObservableCollection<BulletinModel> BulletinItems
        {
            get => _bulletinItems;
            set => SetProperty(ref _bulletinItems, value);
        }

        #region BulletinPostVariables
        //private int _bulletinIdx;
        //public int BulletinIdx
        //{
        //    get => _bulletinIdx;
        //    set => SetProperty(ref _bulletinIdx, value);
        //}

        private string _bulletinPostTitle;
        public string BulletinPostTitle
        {
            get => _bulletinPostTitle;
            set
            {
                if (value.Length <= 20)
                {
                    SetProperty(ref _bulletinPostTitle, value);
                }
                return;
            } 
        }

        private string _bulletinPostContent;
        public string BulletinPostContent
        {
            get => _bulletinPostContent;
            set
            {
                if (value.Length <= 1000)
                {
                    SetProperty(ref _bulletinPostContent, value);
                }
                return;
            }
        }

        private string _bulletinPostWriter;
        public string BulletinPostWriter
        {
            get => _bulletinPostWriter;
            set => SetProperty(ref _bulletinPostWriter, value);
        }

        private byte[] _bulletinPostImage;
        public byte[] BulletinPostImage
        {
            get => _bulletinPostImage;
            set => SetProperty(ref _bulletinPostImage, value);
        }

        private string _bulletinImgName;
        public string BulletinImgName
        {
            get => _bulletinImgName;
            set => SetProperty(ref _bulletinImgName, value);
        }

        private string _bulletinImgPath;
        public string BulletinImgPath
        {
            get => _bulletinImgPath;
            set => SetProperty(ref _bulletinImgPath, value);
        }
        #endregion

        private bool _isRequestEnabled = true;
        public bool IsRequestEnabled
        {
            get => _isRequestEnabled;
            set => SetProperty(ref _isRequestEnabled, value);
        }

        public Visibility ModalBackGround { get; set; } = Visibility.Collapsed;
        #endregion

        #region Commands
        public ICommand BulletinWriteCommand { get; set; }
        #endregion

        public BulletinViewModel()
        {
            InitVariables();
            InitCommand();
        }

        #region Initialize
        private void InitVariables()
        {
            BulletinItems = new ObservableCollection<BulletinModel>();
            TempBulletinItems = new ObservableCollection<BulletinModel>();
        }

        private void InitCommand()
        {
            BulletinWriteCommand = new DelegateCommand(OnWrite, CanRequest).ObservesCanExecute(() => IsRequestEnabled);
        }

        public void ClearDatas()
        {
            BulletinItems.Clear();
        }
        #endregion

        #region CommandMethod
        private async void OnWrite()
        {
            IsRequestEnabled = false;

            if (BulletinPostTitle != null && BulletinPostContent != null && BulletinPostWriter != null && BulletinImgName != null)
            { 
                var resp = await bulletinService.WriteBulletin(BulletinPostTitle, BulletinPostContent, BulletinPostWriter);
                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BulletinPostResultReceived?.Invoke(this);
                    await SaveBulletinImage(BulletinPostTitle, BulletinImgName, BulletinImgPath);
                    ClearWritePostDatas();
                    await GetBulletinList();
                }
            }
            else if (BulletinPostTitle != null && BulletinPostContent != null && BulletinPostWriter != null)
            {
                var resp = await bulletinService.WriteBulletin(BulletinPostTitle, BulletinPostContent, BulletinPostWriter);
                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BulletinPostResultReceived?.Invoke(this);
                    ClearWritePostDatas();
                    await GetBulletinList();
                }
            }

            ModalBackGround = Visibility.Collapsed;
            IsRequestEnabled = true;
        }

        private bool CanRequest()
        {
            return IsRequestEnabled;
        }
        #endregion

        #region CommunicateServer
        private async Task GetBulletinList()
        {
            var resp = await bulletinService.GetBulletinList();

            if (resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    var bulletin = resp.Data.Bulletins.OrderByDescending(x => x.BulletinIdx).ToList();
                    BulletinItems = new ObservableCollection<BulletinModel>(bulletin);
                }
                catch (Exception e)
                {
                    Message?.Invoke(e.Message);
                    Debug.WriteLine("GET BULLETINS LIST ERROR : " + e.Message);
                }
            }
            else
            {
                Message?.Invoke(resp.Message);
            }
        }
        #endregion

        private async Task SaveBulletinImage(string bulletinPostTitle, string imgName, string imgPath)
        {
            try
            {
                BulletinModel bulletinModel = new BulletinModel();

                using (IDbConnection db = new MySqlConnection($"SERVER = localhost; DATABASE = Bulletin; UID = root; PASSWORD =#kkh03kkh#;allow user variables=true"))
                {
                    db.Open();

                    string selectBulletinSql = $@"
SELECT
    bulletin_idx
FROM
    bulletin_tb
WHERE
    title = '{bulletinPostTitle}'
";
                    bulletinModel = await SqlMapper.QueryFirstOrDefaultAsync<BulletinModel>(db, selectBulletinSql, "");

                    ImageModel imageModel = new ImageModel();
                    imageModel.ImageName = imgName;
                    imageModel.ImgSize = ConvertImgSourceValue(imgPath);
                    imageModel.BulletinIdx = bulletinModel.BulletinIdx;

                    string insertImageSql = @"
INSERT INTO image_tb(
    img_name,
    img_size,
    bulletin_idx
)
VALUES(
    @img_name,
    @img_size,
    @bulletin_idx
);";

                    await SqlMapper.QueryAsync<ImageModel>(db, insertImageSql, imageModel);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private byte[] ConvertImgSourceValue(string imgPath)
        {
            FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
            byte[] imgByteArr = new byte[fileStream.Length];
            fileStream.Read(imgByteArr, 0, Convert.ToInt32(fileStream.Length));
            return imgByteArr;
        }

        public void ClearWritePostDatas()
        {
            BulletinPostTitle = string.Empty;
            BulletinPostContent = string.Empty;
        }

        public async Task LoadDataAsync()
        {
            ClearDatas();
            await GetBulletinList();
        }
    }
}
