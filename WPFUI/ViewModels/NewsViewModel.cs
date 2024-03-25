using System.Collections.Generic;
using System.Dynamic;

namespace WPFUI.ViewModels
{
    public class NewsViewModel : Screen
    {
        private BindableCollection<NewsModel> _news = new();
        private NewsModel _selectedNews;
        private string _selectedPriority;
        private IWindowManager _manager = new WindowManager();

        public NewsViewModel()
        {
            PopulateList();
            Priorities = new List<string> { "none", "low", "medium", "high" };
        }

        public BindableCollection<NewsModel> News
        {
            get { return _news; }
            set { _news = value; }
        }

        public List<string> Priorities { get; set; }

        public string SelectedPriority
        {
            get { return _selectedPriority; }
            set
            {
                _selectedPriority = value;
                NotifyOfPropertyChange(() => SelectedPriority);
                FilterCompanies();
            }
        }

        public NewsModel SelectedNews
        {
            get { return _selectedNews; }
            set
            {
                _selectedNews = value;
                NotifyOfPropertyChange(() => _selectedNews);
            }
        }

        private void PopulateList()
        {
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            _news = new BindableCollection<NewsModel>(sql.ReadAllNews().OrderByDescending(x => x.Date));

            foreach (var newsItem in _news)
            {
                newsItem.CompanyId = sql.ReadCompanyIdByNewsId(newsItem.Id);
                newsItem.Company = sql.ReadCommpanyNameById(newsItem.CompanyId);
                newsItem.DateDisplay = Calc.TimeStampToDateOnly(newsItem.Date);
            }
        }

        private void FilterCompanies()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(News);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(SelectedPriority))
            {
                return true;
            }
            else
            {
                return (item as NewsModel).Priority.IndexOf(SelectedPriority, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        public void ClearFilter()
        {
            SelectedPriority = null;
            //PopulateList();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(News);
            
        }
        public void EditNews()
        {
            dynamic settings = new ExpandoObject();
            settings.Title = "Edit News";
            settings.ResizeMode = ResizeMode.NoResize;
            _manager.ShowWindowAsync(new NewsDataViewModel(_selectedNews), null, settings);
        }
    }
}
