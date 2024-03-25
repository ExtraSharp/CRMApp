using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.ViewModels.DataEntry
{
    public class CourseDataViewModel : Screen
    {
        # region Private Fields
        private CourseModel _course;
        private BindableCollection<CompanyModel> _companies = new();
        private BindableCollection<ContractModel> _contracts = new();
        private CompanyModel _selectedCompany;
        private ContractModel _selectedContract;
        private string _mode;
        private bool _bookUsed;
        Visibility _comboboxVisibility = Visibility.Visible;
        Visibility _bookVisibility = Visibility.Collapsed;
        #endregion

        public CourseDataViewModel()
        {
            _mode = "add";
            ButtonText = "Save";
            _course = new CourseModel();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            Companies = new BindableCollection<CompanyModel>(sql.ReadAllCompanies().OrderBy(x => x.Name));
            CompletedAsOf = DateTime.Now;              
        }
        public CourseDataViewModel(CourseModel course)
        {
            _mode = "edit";
            ButtonText = "Update";
            _course = course;
            ComboboxVisibility = Visibility.Collapsed;
        }

        #region Properties      
        public Visibility ComboboxVisibility
        {
            get { return _comboboxVisibility; }
            set
            {
                _comboboxVisibility = value;
                NotifyOfPropertyChange(() => ComboboxVisibility);
            }
        }
        public Visibility BookVisibility
        {
            get { return _bookVisibility; }
            set
            {
                _bookVisibility = value;
                NotifyOfPropertyChange(() => BookVisibility);
            }
        }
        

        public bool BookUsed
        {
            get { return _bookUsed; }
            set 
            { 
                _bookUsed = value; 
                NotifyOfPropertyChange(() => BookUsed);
                
                if (BookVisibility == Visibility.Collapsed)
                {
                    BookVisibility = Visibility.Visible;
                }
                else
                {
                    BookVisibility = Visibility.Collapsed;
                }
                
            }
        }

        public string ButtonText { get; set; }
        public BindableCollection<CompanyModel> Companies
        {
            get { return _companies; }
            set { _companies = value; }
        }
        public CompanyModel SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                _selectedCompany = value;
                NotifyOfPropertyChange(() => SelectedCompany);
                Contracts.Clear();
                if (SelectedCompany != null)
                {
                    LoadContracts();
                }
            }
        }

        public BindableCollection<ContractModel> Contracts
        {
            get { return _contracts; }
            set { _contracts = value; }
        }

        public ContractModel SelectedContract
        {
            get { return _selectedContract; }
            set
            {
                _selectedContract = value;
                NotifyOfPropertyChange(() => SelectedContract);

                if (value != null)
                {
                    _course.ContractId = SelectedContract.Id;
                }
            }
        }

        public string Name
        {
            get { return _course.Name; }
            set
            {
                if (_course.Name != value)
                {
                    _course.Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }
        public string TotalUe
        {
            get { return _course.TotalUe; }
            set
            {
                if (_course.TotalUe != value)
                {
                    _course.TotalUe = value;
                    NotifyOfPropertyChange(() => TotalUe);
                }
            }
        }
        public string UeLength
        {
            get { return _course.UeLength; }
            set
            {
                if (_course.UeLength != value)
                {
                    _course.UeLength = value;
                    NotifyOfPropertyChange(() => UeLength);
                }
            }
        }
        public string CompletedUe
        {
            get { return _course.CompletedUe; }
            set
            {
                if (_course.CompletedUe != value)
                {
                    _course.CompletedUe = value;
                    NotifyOfPropertyChange(() => CompletedUe);
                }
            }
        }
        public string UePerSession
        {
            get { return _course.UePerSession; }
            set
            {
                if (_course.UePerSession != value)
                {
                    _course.UePerSession = value;
                    NotifyOfPropertyChange(() => UePerSession);
                }
            }
        }
        public string Book
        {
            get { return _course.Book; }
            set
            {
                if (_course.Book != value)
                {
                    _course.Book = value;
                    NotifyOfPropertyChange(() => Book);
                }
            }
        }
        public DateTime CompletedAsOf
        {
            get { return Calc.TimeStampToDateTime(_course.CompletedAsOf); }
            set
            {
                if (_course.CompletedAsOf != (int)Calc.ConvertToTimestamp(value))
                {
                    _course.CompletedAsOf = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => CompletedAsOf);
                }
            }
        }
        public DateTime BookOrdered
        {
            get { return Calc.TimeStampToDateTime(_course.BookOrdered); }
            set
            {
                if (_course.BookOrdered != (int)Calc.ConvertToTimestamp(value))
                {
                    _course.BookOrdered = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => BookOrdered);
                }
            }
        }
        public DateTime BookReimbursed
        {
            get { return Calc.TimeStampToDateTime(_course.BookReimbursed); }
            set
            {
                if (_course.BookReimbursed != (int)Calc.ConvertToTimestamp(value))
                {
                    _course.BookReimbursed = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => BookReimbursed);
                }
            }
        }
        #endregion

        #region Methods        
        private void LoadContracts()
        {
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> contractIds = sql.ReadContractIdsByCompany(SelectedCompany.Id);

            foreach (int id in contractIds)
            {
                ContractModel contract = sql.ReadContractById(id);
                Contracts.Add(contract);
            }
        }
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
                sql.CreateCourse(_course);

                MessageBox.Show("Course successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                sql.UpdateCourse(_course);

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
            SelectedContract = null;
            Name = String.Empty;
            Book = String.Empty;
            TotalUe = String.Empty;
            UeLength = String.Empty;
            UePerSession = String.Empty;
            CompletedUe = String.Empty;
            SelectedCompany = null;
        }
        #endregion
    }    
}
