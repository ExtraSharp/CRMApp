using System.Collections.Generic;

namespace WPFUI.ViewModels.DataEntry
{
    public class CommunicationDataViewModel : Screen
    {
        #region Private Fields
        private CommunicationModel _comm;
        private BindableCollection<CompanyModel> _companies = new();
        private BindableCollection<FullContactModel> _contacts = new();
        private CompanyModel _selectedCompany;
        private FullContactModel _selectedContact;
        private string _selectedMeans;
        private string _selectedSubject;
        private bool _followUp;
        private string _mode;
        Visibility _comboboxVisibility = Visibility.Visible;
        #endregion
        public CommunicationDataViewModel()
        {
            _mode = "add";
            ButtonText = "Save";
            _comm = new CommunicationModel();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            Companies = new BindableCollection<CompanyModel>(sql.ReadAllCompanies().OrderBy(x => x.Name));
            Date = DateTime.Now;
            Subjects = new List<string> { "Current contract", "Contract extension", "Pitch", "Other" };
            Means = new List<string> { "E-Mail", "In-Person", "LinkedIn", "Phone", "XING" };
        }
        public CommunicationDataViewModel(CommunicationModel comm)
        {
            _comm = comm;
            ButtonText = "Update";
            _mode = "edit";
            ComboboxVisibility = Visibility.Collapsed;
        }

        #region Properties
        public string ButtonText { get; set; }
        public Visibility ComboboxVisibility
        {
            get { return _comboboxVisibility; }
            set
            {
                _comboboxVisibility = value;
                NotifyOfPropertyChange(() => ComboboxVisibility);
            }
        }
        public bool FollowUp
        {
            get { return _followUp; }
            set
            {
                _followUp = value;
                NotifyOfPropertyChange(() => FollowUp);
                _comm.FollowUpNeeded = value;
            }
        }
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
                Contacts.Clear();
                if (SelectedCompany != null)
                {
                    LoadContacts();
                }                               
            }
        }
        public BindableCollection<FullContactModel> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }
        public List<string> Means { get; set; }
        public List<string> Subjects { get; set; }

        public FullContactModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                NotifyOfPropertyChange(() => SelectedContact);

                if (value != null)
                {
                    _comm.ContactId = SelectedContact.Id;
                }
            }
        }
        public string SelectedMeans
        {
            get { return _selectedMeans; }
            set
            {
                _selectedMeans = value;
                NotifyOfPropertyChange(() => SelectedMeans);

                if (value != null)
                {
                    _comm.Means = SelectedMeans;
                }
            }
        }
        public string SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                NotifyOfPropertyChange(() => SelectedSubject);

                if (value != null)
                {
                    _comm.Subject = SelectedSubject;
                }
            }
        }

        public DateTime Date
        {
            get { return Calc.TimeStampToDateTime(_comm.Date); }
            set
            {
                if (_comm.Date != (int)Calc.ConvertToTimestamp(value))
                {
                    _comm.Date = (int)Calc.ConvertToTimestamp(value);
                    NotifyOfPropertyChange(() => Date);
                }
            }
        }
        public string Details
        {
            get { return _comm.Details; }
            set
            {
                if (_comm.Details != value)
                {
                    _comm.Details = value;
                    NotifyOfPropertyChange(() => Details);
                }
            }
        }
        #endregion

        #region Methods
        private void LoadContacts()
        {
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> contactIds = sql.ReadContactIdsByCompany(SelectedCompany.Id);
            
            foreach (int id in contactIds)
            {
                FullContactModel contact = sql.ReadContactById(id);
                Contacts.Add(contact);
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
                sql.CreateCommunication(_comm);

                MessageBox.Show("Communication successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                sql.UpdateCommunication(_comm);

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
            SelectedContact = null;
            Details = String.Empty;
            SelectedCompany = null;
            Date = DateTime.Now;
            SelectedMeans = null;
            SelectedSubject = null;
            FollowUp = false;
        }
        #endregion
    }
}
