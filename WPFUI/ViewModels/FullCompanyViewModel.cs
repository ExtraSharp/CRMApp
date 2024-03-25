namespace WPFUI.ViewModels
{
    public class FullCompanyViewModel : Screen
    {
        #region Private Fields
        private CompanyModel _company;
        private BindableCollection<CommunicationModel> _comms = new();
        private BindableCollection<FullContactModel> _contacts = new();
        private BindableCollection<ContractModel> _contracts = new();
        private BindableCollection<NewsModel> _news = new();
        private BindableCollection<CourseModel> _courses = new();
        private CommunicationModel _selectedCommunication;
        private CourseModel _selectedCourse;
        private ContractModel _selectedContract;
        private NewsModel _selectedNews;
        private IWindowManager _manager = new WindowManager();
        #endregion
        public FullCompanyViewModel(CompanyModel company)
        {
            _company = company;
            Name = _company.Name;
            UpdateContacts();
            UpdateContracts();
            UpdateComms();
            UpdateNews();
            UpdateCourses();
        }

        private bool _onlyActive;

        public bool OnlyActive
        {
            get { return _onlyActive; }
            set 
            { 
                _onlyActive = value; 
                NotifyOfPropertyChange(() => OnlyActive);
                UpdateContracts();
            }
        }

        public string Name { get; set; }
        public BindableCollection<FullContactModel> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }
        public BindableCollection<NewsModel> News
        {
            get { return _news; }
            set { _news = value; }
        }
        public BindableCollection<ContractModel> Contracts
        {
            get { return _contracts; }
            set 
            { 
                _contracts = value;
                NotifyOfPropertyChange(() => Contracts);
            }
        }
        public BindableCollection<CommunicationModel> Comms
        {
            get { return _comms; }
            set { _comms = value; }
        }
        public BindableCollection<CourseModel> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        public CommunicationModel SelectedCommunication
        { 
            get { return _selectedCommunication; }
            set
            {
                _selectedCommunication = value;
                NotifyOfPropertyChange(() => SelectedCommunication);
            }
        }
        public CourseModel SelectedCourse
        {
            get { return _selectedCourse; }
            set
            {
                _selectedCourse = value;
                NotifyOfPropertyChange(() => SelectedCourse);
            }
        }
        public ContractModel SelectedContract
        {
            get { return _selectedContract; }
            set
            {
                _selectedContract = value;
                NotifyOfPropertyChange(() => SelectedContract);
            }
        }
        public NewsModel SelectedNews
        {
            get { return _selectedNews; }
            set
            {
                _selectedNews = value;
                NotifyOfPropertyChange(() => SelectedNews);
            }
        }

        private void UpdateContacts()
        {
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> contactIds =sql.ReadContactIdsByCompany(_company.Id);

            foreach (int id in contactIds)
            {
                FullContactModel contact = sql.ReadContactById(id);
                Contacts.Add(contact);
            }
        }

        private void UpdateNews()
        {
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> newsIds = sql.ReadNewsIdsByCompany(_company.Id);
            List<NewsModel> unsorted = new();
            foreach (int id in newsIds)
            {
                NewsModel news = sql.ReadNewsById(id);
                news.DateDisplay = Calc.TimeStampToDateOnly(news.Date);
                unsorted.Add(news);
            }

            News = new BindableCollection<NewsModel>(unsorted.OrderByDescending(x => x.Date));

        }        

        private void UpdateCourses()
        {
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> contractIds = sql.ReadContractIdsByCompany(_company.Id);
            List<CourseModel> unsorted = new();

            foreach (var contract in Contracts)
            {
                List<int> courseIds = sql.ReadCourseIdsByContract(contract.Id);
                
                foreach (var courseId in courseIds)
                {
                    CourseModel course = sql.ReadCourseById(courseId);
                    course.CompletedAsOfDisplay = Calc.TimeStampToDateOnly(course.CompletedAsOf);
                    unsorted.Add(course);                   
                }
            }

            Courses = new BindableCollection<CourseModel>(unsorted.OrderByDescending(x => x.CompletedAsOf));
        }

        private void UpdateContracts()
        {
            Contracts.Clear();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> contractIds = sql.ReadContractIdsByCompany(_company.Id);
            List<ContractModel> unsorted = new();

            foreach (int id in contractIds)
            {
                ContractModel contract = sql.ReadContractById(id);
                contract.StartDateDisplay = Calc.TimeStampToDateOnly(contract.StartDate);
                contract.EndDateDisplay = Calc.TimeStampToDateOnly(contract.EndDate);
                contract.SentToInstructorDisplay = Calc.TimeStampToDateOnly(contract.SentToInstructor);
                if (OnlyActive == false)
                {
                    unsorted.Add(contract);
                }
                else
                {
                    if (contract.IsActive == true)
                    {
                        unsorted.Add(contract);
                    }
                }                
            }
            Contracts = new BindableCollection<ContractModel>(unsorted.OrderByDescending(x => x.StartDate));
        }

        private void UpdateComms()
        {
            Comms.Clear();
            SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
            List<int> contactIds = sql.ReadContactIdsByCompany(_company.Id);
            List<CommunicationModel> unsorted = new();
            foreach (var contact in Contacts)
            {
                List<int> commIds = sql.ReadCommuncationIdByContact(contact.Id);

                foreach (var commId in commIds)
                {
                    CommunicationModel comm = sql.ReadCommunicationById(commId);
                    comm.DateDisplay = Calc.TimeStampToDateOnly(comm.Date);
                    unsorted.Add(comm);
                }
            }

            Comms = new BindableCollection<CommunicationModel>(unsorted.OrderByDescending(x => x.Date));
        }

        public void EditCourse()
        {
            dynamic settings = new ExpandoObject();
            settings.Title = "Edit Course";
            settings.ResizeMode = ResizeMode.NoResize;
            _manager.ShowWindowAsync(new CourseDataViewModel(_selectedCourse), null, settings);
        }
        public void EditNews()
        {
            dynamic settings = new ExpandoObject();
            settings.Title = "Edit News";
            settings.ResizeMode = ResizeMode.NoResize;
            _manager.ShowWindowAsync(new NewsDataViewModel(_selectedNews), null, settings);
        }
        public void EditCommunication()
        {
            dynamic settings = new ExpandoObject();
            settings.Title = "Edit News";
            settings.ResizeMode = ResizeMode.NoResize;
            _manager.ShowWindowAsync(new CommunicationDataViewModel(_selectedCommunication), null, settings);
        }
        public void EditContract()
        {
            dynamic settings = new ExpandoObject();
            settings.Title = "Edit Contract";
            settings.ResizeMode = ResizeMode.NoResize;
            _manager.ShowWindowAsync(new ContractDataViewModel(_selectedContract), null, settings);
        }
    }
}
