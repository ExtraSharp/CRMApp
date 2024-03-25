using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.ViewModels.DataEntry
{
    public class NewsDataViewModel : Screen
    {
        #region Private Fields
        private NewsModel _news;
        private BindableCollection<CompanyModel> _companies = new();
        private CompanyModel _selectedCompany;
        private string _selectedPriority;
        private string _mode;
        Visibility _companyVisibility = Visibility.Visible;
        #endregion

        public NewsDataViewModel()
        {
            _mode = "add";
            ButtonText = "Save";
            _news = new NewsModel();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            Companies = new BindableCollection<CompanyModel>(sql.ReadAllCompanies().OrderBy(x => x.Name));
            Date = DateTime.Now;
            Priorities = new List<string> { "none", "low", "medium", "high" };
        }
        public NewsDataViewModel(NewsModel news)
        {
            _news = news;
            ButtonText = "Update";
            _mode = "edit";
            Priorities = new List<string> { "none", "low", "medium", "high" };
            SelectedPriority = news.Priority;
            CompanyVisibility = Visibility.Collapsed;
        }

        #region Properties
        public Visibility CompanyVisibility
        {
            get { return _companyVisibility; }
            set
            {
                _companyVisibility = value;
                NotifyOfPropertyChange(() => CompanyVisibility);
            }
        }
        public string ButtonText { get; set; }
        public CompanyModel SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                _selectedCompany = value;
                NotifyOfPropertyChange(() => SelectedCompany);
                
                if (value != null)
                {
                    _news.CompanyId = SelectedCompany.Id;
                }
            }
        }

        public List<string> Priorities { get; set; }

        public string SelectedPriority
        {
            get { return _selectedPriority; }
            set
            {
                _selectedPriority = value;
                NotifyOfPropertyChange(() => SelectedPriority);

                if (value != null)
                {
                    _news.Priority = SelectedPriority;
                }
            }
        }

        public BindableCollection<CompanyModel> Companies
        {
            get { return _companies; }
            set { _companies = value; }
        }

        public string Source
        {
            get { return _news.Source; }
            set
            {
                if (_news.Source != value)
                {
                    _news.Source = value;
                    NotifyOfPropertyChange(() => Source);
                }
            }
        }

        public DateTime Date
        {
            get { return Calc.TimeStampToDateTime(_news.Date); }
            set
            {
                if (_news.Date != (int)Calc.ConvertToTimestamp(value))
                {
                    _news.Date = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => Date);
                }
            }
        }

        public string Details
        {
            get { return _news.Details; }
            set
            {
                if (_news.Details != value)
                {
                    _news.Details = value;
                    NotifyOfPropertyChange(() => Details);
                }
            }
        }
        #endregion

        #region Methods
        public void Save()
        {
            if (_mode == "add")
            {
                Add();
            }
            else if (_mode == "edit")
            {
                Update();
            }
            
        }
        public void Add()
        {
            try
            {
                SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
                sql.CreateNews(_news);

                MessageBox.Show("News successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Update()
        {
            try
            {
                SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
                sql.UpdateNews(_news);

                MessageBox.Show("Changes successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.TryCloseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFields()
        {
            Source = String.Empty;
            Details = String.Empty;
            SelectedCompany = null;
            Date = DateTime.Now;
            SelectedPriority = null;
        }
        #endregion
    }
}
