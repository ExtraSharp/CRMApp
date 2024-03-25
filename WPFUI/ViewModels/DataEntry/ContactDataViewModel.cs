namespace WPFUI.ViewModels.DataEntry
{
    public class ContactDataViewModel : Screen
    {
        #region Private Fields
        private FullContactModel _contact;
        private BindableCollection<CompanyModel> _companies = new();
        private CompanyModel _selectedCompany;
        #endregion

        public ContactDataViewModel()
        {
            _contact = new FullContactModel();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            Companies = new BindableCollection<CompanyModel>(sql.ReadAllCompanies().OrderBy(x => x.Name));
        }

        #region Properties
        public CompanyModel SelectedCompany
        {
            get { return _selectedCompany; }
            set 
            {
                _selectedCompany = value;
                NotifyOfPropertyChange(() => SelectedCompany);
                if (value != null)
                {
                    _contact.Company = SelectedCompany.Name;
                }
            }
        }

        public BindableCollection<CompanyModel> Companies
        {
            get { return _companies; }
            set { _companies = value; } 
        }

        public string FirstName
        {
            get { return _contact.FirstName; }
            set
            {
                if (_contact.FirstName != value)
                {
                    _contact.FirstName = value;
                    NotifyOfPropertyChange(() => FirstName);
                }
            }
        }
        public string LastName
        {
            get { return _contact.LastName; }
            set
            {
                if (_contact.LastName != value)
                {
                    _contact.LastName = value;
                    NotifyOfPropertyChange(() => LastName);
                }
            }
        }
        public string Salutation
        {
            get { return _contact.Salutation; }
            set
            {
                if (_contact.Salutation != value)
                {
                    _contact.Salutation = value;
                    NotifyOfPropertyChange(() => Salutation);
                }
            }
        }
        public string Email
        {
            get { return _contact.Email; }
            set
            {
                if (_contact.Email != value)
                {
                    _contact.Email = value;
                    NotifyOfPropertyChange(() => Email);
                }
            }
        }
        public string PhoneNumber
        {
            get { return _contact.PhoneNumber; }
            set
            {
                if (_contact.PhoneNumber != value)
                {
                    _contact.PhoneNumber = value;
                    NotifyOfPropertyChange(() => PhoneNumber);
                }
            }
        }
        public string Notes
        {
            get { return _contact.Notes; }
            set
            {
                if (_contact.Notes != value)
                {
                    _contact.Notes = value;
                    NotifyOfPropertyChange(() => Notes);
                }
            }
        }
        public string Position
        {
            get { return _contact.Position; }
            set
            {
                if (_contact.Position != value)
                {
                    _contact.Position = value;
                    NotifyOfPropertyChange(() => Position);
                }
            }
        }
        #endregion

        #region Methods
        public void Save()
        {
            try
            {
                SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
                sql.CreateContact(_contact);

                MessageBox.Show("Contact successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFields()
        {
            Salutation = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            Email = String.Empty;
            PhoneNumber = String.Empty;
            Notes = String.Empty;
            Position = String.Empty;
            SelectedCompany = null;
        }
        #endregion
    }
}
