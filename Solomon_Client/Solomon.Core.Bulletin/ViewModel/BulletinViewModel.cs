using Dapper;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Solomon.Core.Bulletin.Model;
using Solomon.Core.Bulletin.Service;
using Solomon.Core.Util;
using Solomon_Client.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        #region Properties
        #region BulletinVariables
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

        private ObservableCollection<BulletinModel> _specificBulletinItems;
        public ObservableCollection<BulletinModel> SpecificBulletinItems
        {
            get => _specificBulletinItems;
            set => SetProperty(ref _specificBulletinItems, value);
        }

        public List<CategoryModel> BulletinCategories { get; set; }

        private CategoryModel _selectedCategory;
        public CategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private string _bulletinPostTitle;
        public string BulletinPostTitle
        {
            get => _bulletinPostTitle;
            set
            {
                if (value.Length <= 60)
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

        private int _bulletinIdx;
        public int BulletinIdx
        {
            get => _bulletinIdx;
            set => SetProperty(ref _bulletinIdx, value);
        }

        public Visibility IsVisible { get; set; } = Visibility.Collapsed;

        private ObservableCollection<BulletinModel> _popularBulletinItems;
        public ObservableCollection<BulletinModel> PopularBulletinItems
        {
            get => _popularBulletinItems;
            set => SetProperty(ref _popularBulletinItems, value);
        }

        private List<string> _popularTopicItems;
        public List<string> PopularTopicItems
        {
            get => _popularTopicItems;
            set => SetProperty(ref _popularTopicItems, value);
        }
        #endregion

        #region CommentVariables
        private ObservableCollection<CommentModel> _bulletinCommentItems;
        public ObservableCollection<CommentModel> BulletinCommentItems
        {
            get => _bulletinCommentItems;
            set => SetProperty(ref _bulletinCommentItems, value);
        }

        private string _bulletinCommentContent;
        public string BulletinCommentContent
        {
            get => _bulletinCommentContent;
            set => SetProperty(ref _bulletinCommentContent, value);
        }

        private int _specificBulletinIdx;
        public int SpecificBulletinIdx
        {
            get => _specificBulletinIdx;
            set => SetProperty(ref _specificBulletinIdx, value);
        }

        private int _commentIdx;
        public int CommentIdx
        {
            get => _commentIdx;
            set => SetProperty(ref _commentIdx, value);
        }
        #endregion

        #region Common
        private string _writer;
        public string Writer
        {
            get => _writer;
            set => SetProperty(ref _writer, value);
        }

        private bool _isRequestEnabled = true;
        public bool IsRequestEnabled
        {
            get => _isRequestEnabled;
            set => SetProperty(ref _isRequestEnabled, value);
        }
        #endregion

        #region Chart
        public SeriesCollection GenderRatioPieCollection { get; set; }
        public SeriesCollection AgeRatioColumnCollection { get; set; }
        #endregion
        #endregion

        #region Commands
        public ICommand BulletinWriteCommand { get; set; }
        public ICommand BulletinDeleteCommand { get; set; }
        public ICommand BulletinCommentWriteCommand { get; set; }
        public ICommand SpecificBulletinCommentWriteCommand { get; set; }
        public ICommand CommentDeleteCommand { get; set; }

        #endregion
        
        #region Constructor
        public BulletinViewModel()
        {
            InitVariables();
            InitCommand();
            LoadCategories();
        }
        #endregion

        #region Initialize
        private void InitVariables()
        {
            BulletinItems = new ObservableCollection<BulletinModel>();
            TempBulletinItems = new ObservableCollection<BulletinModel>();
            SpecificBulletinItems = new ObservableCollection<BulletinModel>();
            BulletinCategories = new List<CategoryModel>();
            BulletinCommentItems = new ObservableCollection<CommentModel>();
            PopularBulletinItems = new ObservableCollection<BulletinModel>();
            PopularTopicItems = new List<string>();
            GenderRatioPieCollection = new SeriesCollection();
            AgeRatioColumnCollection = new SeriesCollection();
        }

        private void InitCommand()
        {
            BulletinWriteCommand = new DelegateCommand(OnBulletinWrite, CanRequest).ObservesCanExecute(() => IsRequestEnabled); // 게시물 작성
            BulletinDeleteCommand = new DelegateCommand(OnBulletinDelete, CanRequest).ObservesCanExecute(() => IsRequestEnabled); // 게시물 삭제
            BulletinCommentWriteCommand = new DelegateCommand(OnBulletinComment, CanRequest).ObservesCanExecute(() => IsRequestEnabled); // 전체 게시물에서 댓글 작성
            SpecificBulletinCommentWriteCommand = new DelegateCommand(OnSpecificBulletinComment, CanRequest).ObservesCanExecute(() => IsRequestEnabled); // 특정 게시물 댓글 작성
            CommentDeleteCommand = new DelegateCommand(OnCommentDelete, CanRequest).ObservesCanExecute(() => IsRequestEnabled); // 댓글 삭제
        }

        public void ClearDatas()
        {
            BulletinItems.Clear();
            TempBulletinItems.Clear();
            BulletinCommentItems.Clear();
        }

        private void LoadCategories()
        {
            #region Add BulletinCategories
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "교육&학문"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "컴퓨터통신"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "게임"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "엔터테인먼트&예술"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "생활"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "건강"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "사회,정치"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "경제"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "여행"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "스포츠&레저"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "쇼핑"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "지역&플레이스"
            });
            BulletinCategories.Add(new CategoryModel()
            {
                CategoryName = "자유"
            });
            #endregion
        }
        #endregion

        #region CommandMethod
        private async void OnBulletinWrite()
        {
            IsRequestEnabled = false;
            if (BulletinPostTitle != null && BulletinPostContent != null && Writer != null && SelectedCategory.CategoryName != null)
            {
                var resp = await bulletinService.WriteBulletin(BulletinPostTitle, BulletinPostContent, Writer, SelectedCategory.CategoryName);
                if (resp.Status == ResponseStatus.CREATED)
                {
                    BulletinPostResultReceived?.Invoke(this);
                    if (BulletinImgName != null)
                    {
                        await SaveBulletinImage(BulletinImgPath, BulletinImgName);
                    }
                    ClearWritePostDatas();
                    await LoadDataAsync();
                }
            }
            IsRequestEnabled = true;
        }

        private async void OnBulletinDelete()
        {
            IsRequestEnabled = false;
            if (Writer != null && BulletinIdx.ToString().Length > 0)
            {
                var resp = await bulletinService.DeleteBulletin(BulletinIdx, Writer);
                if (resp.Status == ResponseStatus.OK)
                {
                    await LoadDataAsync();
                }
            }
            IsRequestEnabled = true;
        }

        private async void OnBulletinComment()
        {
            IsRequestEnabled = false;
            if (BulletinCommentContent != null && Writer != null && BulletinIdx.ToString().Length > 0)
            {
                var resp = await bulletinService.WriteComment(BulletinIdx, BulletinCommentContent, Writer);
                if (resp.Status == ResponseStatus.CREATED)
                {
                    BulletinCommentContent = string.Empty;
                    await GetBulletinList();
                    await GetBulletinImageList();
                }
            }
            IsRequestEnabled = true;
        }

        private async void OnSpecificBulletinComment()
        {
            IsRequestEnabled = false;
            if (BulletinCommentContent != null && Writer != null && SpecificBulletinIdx.ToString().Length > 0)
            {
                var resp = await bulletinService.WriteComment(SpecificBulletinIdx, BulletinCommentContent, Writer);
                if (resp.Status == ResponseStatus.CREATED)
                {
                    BulletinCommentContent = string.Empty;
                    await GetCommentList(SpecificBulletinIdx);
                    await GetBulletinList();
                }
            }
            IsRequestEnabled = true;
        }

        private async void OnCommentDelete()
        {
            IsRequestEnabled = false;
            if (Writer != null && CommentIdx.ToString().Length > 0)
            {
                var resp = await bulletinService.DeleteComment(CommentIdx, Writer);
                if (resp.Status == ResponseStatus.OK)
                {
                    await GetCommentList(SpecificBulletinIdx);
                    await GetBulletinList();
                }
            }
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

            if (resp != null && resp.Status == ResponseStatus.OK && resp.Data != null)
            {
                try
                {
                    BulletinModel bulletinItem = new BulletinModel();
                    var tempBulletinItems = new ObservableCollection<BulletinModel>();

                    foreach (var item in resp.Data.Bulletins)
                    {
                        bulletinItem.BulletinIdx = item.BulletinIdx;
                        bulletinItem.Title = item.Title;
                        bulletinItem.Content = item.Content;
                        bulletinItem.Writer = item.Writer;
                        bulletinItem.WrittenTime = item.WrittenTime;
                        bulletinItem.Category = item.Category;

                        //if (Writer == item.Writer)
                        //{
                        //    bulletinItem.IsVisible = Visibility.Visible;
                        //}
                        //else
                        //{
                        //    bulletinItem.IsVisible = Visibility.Collapsed;
                        //}

                        var response = await bulletinService.GetCommentList(bulletinItem.BulletinIdx);
                        bulletinItem.CommentCount = response.Data.Comments.Count;

                        tempBulletinItems.Add((BulletinModel)bulletinItem.Clone());
                    }

                    BulletinItems = new ObservableCollection<BulletinModel>(tempBulletinItems.OrderByDescending(x => x.BulletinIdx));
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

        public void GetSpecificBulletin(int bulletinIdx)
        {
            SpecificBulletinItems.Add(BulletinItems.Where(x => x.BulletinIdx == bulletinIdx).FirstOrDefault());
        }

        public async Task GetCommentList(int bulletin_idx)
        {
            SpecificBulletinIdx = bulletin_idx;

            if (BulletinCommentItems.Count > 0)
            {
                BulletinCommentItems.Clear();
            }

            var resp = await bulletinService.GetCommentList(bulletin_idx);

            if (resp != null && resp.Status == ResponseStatus.OK && resp.Data != null)
            {
                try
                {
                    CommentModel commentItems = new CommentModel();

                    foreach (var item in resp.Data.Comments)
                    {
                        commentItems.CommentIdx = item.CommentIdx;
                        commentItems.BulletinIdx = item.BulletinIdx;
                        commentItems.Content = item.Content;
                        commentItems.Writer = item.Writer;

                        BulletinCommentItems.Add((CommentModel)commentItems.Clone());
                    }
                }
                catch (Exception e)
                {
                    Message?.Invoke(e.Message);
                    Debug.WriteLine("GET COMMENTS LIST ERROR : " + e.Message);
                }
            }
            else
            {
                Message?.Invoke(resp.Message);
            }
        }
        #endregion

        #region DataBase
        public async Task GetBulletinImageList()
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
                    // TODO : 이미지의 크기가 너무 크면 오류가 나는거 같음. 확인해봐야 할듯.
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
        #endregion

        #region Chart
        public void LoadGenderRatioDatas()
        {
            GenderRatioPieCollection.Clear();

            GenderRatioPieCollection.Add(new PieSeries()
            {
                Title = "Male",
                Values = new ChartValues<ObservableValue> { new ObservableValue(52) },
                DataLabels = true
            });
            GenderRatioPieCollection.Add(new PieSeries()
            {
                Title = "FeMale",
                Values = new ChartValues<ObservableValue> { new ObservableValue(48) },
                DataLabels = true
            });
        }

        public void LoadAgeRatioDatas()
        {
            AgeRatioColumnCollection.Clear();

            AgeRatioColumnCollection.Add(new ColumnSeries()
            {
                Title = "2015",
                Values = new ChartValues<double> { 10, 50, 39, 50 }
            });
        }
        #endregion

        internal void SetPopularBulletins()
        {
            if (PopularBulletinItems.Count > 0)
            {
                PopularBulletinItems.Clear();
            }

            var tempBulletins = BulletinItems.OrderByDescending(x => x.CommentCount).ToList();
            
            if (tempBulletins.Count > 0)
            {
                var popularBulletins = tempBulletins.GetRange(0, 5);
                for (int i = 0; i < popularBulletins.Count; i++)
                {
                    PopularBulletinItems.Add(popularBulletins[i]);
                }
            }
        }

        internal void SetPopularTopicItems()
        {
            if (PopularTopicItems.Count > 0)
            {
                PopularTopicItems.Clear();
            }

            var tempTopicItems = new Dictionary<string, int>();
            for (int i = 0; i < BulletinCategories.Count; i++)
            {
                var specificTopicItems = BulletinItems.Where(x => x.Category == BulletinCategories[i].CategoryName).ToList();
                tempTopicItems.Add(BulletinCategories[i].CategoryName, specificTopicItems.Count);   
            }

            var popularTopicItems = tempTopicItems.OrderByDescending(x => x.Value).ToList().GetRange(0, 5);
            foreach (var topic in popularTopicItems)
            {
                PopularTopicItems.Add($"#{topic.Key}");
            }
        }

        public void ClearWritePostDatas()
        {
            BulletinPostTitle = string.Empty;
            BulletinPostContent = string.Empty;
            if (SelectedCategory != null)
            {
                SelectedCategory.CategoryName = string.Empty;
            }
        }

        public async Task LoadDataAsync()
        {
            ClearDatas();
            await GetBulletinList();
            await GetBulletinImageList();
        }
    }
}
