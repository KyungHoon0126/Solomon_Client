using Prism.Mvvm;

namespace Solomon.Core.Bulletin.Model
{
    public class CategoryModel : BindableBase
    {
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }
    }
}
