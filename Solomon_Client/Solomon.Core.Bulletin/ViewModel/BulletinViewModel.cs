﻿using Dapper;
using Prism.Commands;
using Prism.Mvvm;
using Solomon.Core.Bulletin.Model;
using Solomon.Core.Bulletin.Service;
using Solomon_Client.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Solomon.Core.Bulletin.ViewModel
{
    public class BulletinViewModel : BindableBase
    {
        BulletinService bulletinService = new BulletinService();
        MySqlDBConnectionManager mySqlDBConnectionManager = new MySqlDBConnectionManager();

        public delegate void OnMessageHandler(string msg);
        public event OnMessageHandler Message;

        public delegate void BulletinPostResultReceivedHandler(object sender);
        public event BulletinPostResultReceivedHandler BulletinPostResultReceived;

        string sortSql = $@"
ALTER 
    TABLE bulletin.image_tb AUTO_INCREMENT = 1;
SET 
    @COUNT = 0;
UPDATE
    bulletin.image_tb SET img_idx = @COUNT:= @COUNT + 1
;";

        #region Properties
        private ObservableCollection<BulletinModel> _bulletinItems;
        public ObservableCollection<BulletinModel> BulletinItems
        {
            get => _bulletinItems;
            set => SetProperty(ref _bulletinItems, value);
        }

        private ObservableCollection<BulletinModel> _tempBulletinItems;
        public ObservableCollection<BulletinModel> TempBulletinItems
        {
            get => _tempBulletinItems;
            set => SetProperty(ref _tempBulletinItems, value);
        }

        #region BulletinPostVariables
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

        #region Constructor
        public BulletinViewModel()
        {
            InitVariables();
            InitCommand();
        }
        #endregion

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

            if (BulletinPostTitle != null && BulletinPostContent != null && BulletinPostWriter != null)
            { 
                var resp = await bulletinService.WriteBulletin(BulletinPostTitle, BulletinPostContent, BulletinPostWriter);
                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BulletinPostResultReceived?.Invoke(this);
                    if (BulletinImgName != null)
                    {
                        await SaveBulletinImage(BulletinImgPath, BulletinImgName);
                    }
                    //ClearWritePostDatas();
                    //await GetBulletinList();
                    await LoadDataAsync();
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
                    BulletinItems = new ObservableCollection<BulletinModel>(resp.Data.Bulletins.OrderByDescending(x => x.BulletinIdx));
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

        #region DataBase
        private async Task GetBulletinImageList()
        {
            try
            {
                List<DBImageModel> images = new List<DBImageModel>();

                using (IDbConnection db = mySqlDBConnectionManager.GetConnection())
                {
                    string selectSql = @"
SELECT
    *
FROM
    image_tb
";
                    images = (await SqlMapper.QueryAsync<DBImageModel>(db, selectSql, "")).ToList();

                    
                    for (int i = 0; i < images.Count; i++)
                    {
                        for (int j = 0; j < BulletinItems.Count; j++)
                        {
                            if (images[i].bulletin_idx == BulletinItems[j].BulletinIdx)
                            {
                                TempBulletinItems.Add(BulletinItems[j]);
                            }
                        }
                    }

                    for (int i = 0; i < images.Count; i++)
                    {
                        for (int j = 0; j < TempBulletinItems.Count; j++)
                        {
                            if (BulletinItems[j].BulletinIdx == images[i].bulletin_idx)
                            {
                                byte[] blob = images[i].img_size;
                                MemoryStream stream = new MemoryStream();
                                stream.Write(blob, 0, blob.Length);
                                stream.Position = 0;
                                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                                BitmapImage bi = new BitmapImage();
                                bi.BeginInit();

                                MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                                ms.Seek(0, SeekOrigin.Begin);
                                bi.StreamSource = ms;
                                bi.EndInit();
                                BulletinItems[j].BulletinImage = bi;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task SaveBulletinImage(string imgPath, string imgName)
        {
            try
            {
                BulletinModel bulletinModel = new BulletinModel();

                using (IDbConnection db = mySqlDBConnectionManager.GetConnection())
                {
                    // 1. 전체 게시글 조회.
                    var resp = await bulletinService.GetBulletinList();

                    // 2. 작성한 게시물의 Idx 조회.
                    var item = resp.Data.Bulletins.Where(x => x.Title == BulletinPostTitle).FirstOrDefault();

                    // 3. 이미지를 저장하고자 하는 게시글의 Idx 값에 이미지 저장.
                    ImageModel imageModel = new ImageModel();
                    imageModel.ImageName = imgName;
                    imageModel.ImgSize = ConvertImgSourceValue(imgPath);
                    imageModel.BulletinIdx = item.BulletinIdx;

                    string insertImageSql = @"
INSERT INTO image_tb(
    img_name,
    img_size,
    bulletin_idx
)
VALUES(
    @ImageName,
    @ImgSize,
    @BulletinIdx
);";
                    await SqlMapper.QueryAsync<ImageModel>(db, insertImageSql, imageModel);

                    await SqlMapper.QueryAsync(db, sortSql);
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
        #endregion

        public void ClearWritePostDatas()
        {
            BulletinPostTitle = string.Empty;
            BulletinPostContent = string.Empty;
        }

        public async Task LoadDataAsync()
        {
            ClearDatas();
            await GetBulletinList();
            await GetBulletinImageList();
        }
    }
}
