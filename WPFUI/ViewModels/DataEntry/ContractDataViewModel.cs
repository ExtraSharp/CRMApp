using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFUI.ViewModels.DataEntry
{
    public class ContractDataViewModel : Screen
    {
        #region Private Fields
        private ContractModel _contract;
        private BindableCollection<CompanyModel> _companies = new();
        private CompanyModel _selectedCompany;
        private string _designation;
        private bool _isSigned;
        private bool _isActive;
        private string _mode;
        Visibility _companyVisibility = Visibility.Visible;

        #endregion
        public ContractDataViewModel()
        {
            _mode = "add";
            ButtonText = "Save";
            _contract = new ContractModel();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            Companies = new BindableCollection<CompanyModel>(sql.ReadAllCompanies().OrderBy(x => x.Name));
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SentToInstructor = DateTime.Now;
        }
        public ContractDataViewModel(ContractModel contract)
{
            _mode = "edit";
            ButtonText = "Update";
            _contract = contract;
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
                UpdateDesignation();
                if (SelectedCompany != null)
                {
                    _contract.CompanyId = SelectedCompany.Id;
                }                
            }
        }
        
        public bool IsSigned
        {
            get
            {
                return _isSigned;
            }
            set
            {
                _isSigned = value;
                NotifyOfPropertyChange(() => IsSigned);
                _contract.IsSigned = value;
            }
        }
        
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                NotifyOfPropertyChange(() => IsActive);
                _contract.IsActive = value;
            }
        }
        public string PurchaseOrderNumber
        {
            get { return _contract.PurchaseOrderNumber; }
            set
            {
                if (_contract.PurchaseOrderNumber != value)
                {
                    _contract.PurchaseOrderNumber = value;
                    NotifyOfPropertyChange(() => PurchaseOrderNumber);
                }
            }
        }
        public string TotalHours
        {
            get { return _contract.TotalHours; }
            set
            {
                if (_contract.TotalHours != value)
                {
                    _contract.TotalHours = value;
                    NotifyOfPropertyChange(() => TotalHours);
                }
            }
        }

        public string Designation { get; set; }
        public DateTime StartDate
        {
            get { return Calc.TimeStampToDateTime(_contract.StartDate); }
            set
            {
                if (_contract.StartDate != (int)Calc.ConvertToTimestamp(value))
                {
                    _contract.StartDate = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => StartDate);
                    UpdateDesignation();
                }
            }
        }
        public DateTime EndDate
        {
            get { return Calc.TimeStampToDateTime(_contract.EndDate); }
            set
            {
                if (_contract.EndDate != (int)Calc.ConvertToTimestamp(value))
                {
                    _contract.EndDate = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => EndDate);
                }
            }
        }
        public DateTime SentToInstructor
        {
            get { return Calc.TimeStampToDateTime(_contract.SentToInstructor); }
            set
            {
                if (_contract.SentToInstructor != (int)Calc.ConvertToTimestamp(value))
                {
                    _contract.SentToInstructor = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => SentToInstructor);
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
                sql.CreateContract(_contract);

                MessageBox.Show("Contract successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                sql.UpdateContract(_contract);

                MessageBox.Show("Changes successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.TryCloseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateDesignation()
        {
            if ((SelectedCompany != null) && (StartDate != null))
            {
                Designation = $"{ SelectedCompany.Name }_{ StartDate.ToString("yyyyMMdd")}";
                _contract.Designation = Designation;
                NotifyOfPropertyChange(() => Designation);
            }
        }
        private void ResetFields()
        {
            PurchaseOrderNumber = String.Empty;
            TotalHours = String.Empty;
            SelectedCompany = null;
            StartDate = DateTime.Now;
            IsSigned = false;
            IsActive = false;
            Designation = String.Empty;
        }
        #endregion
    }
}
