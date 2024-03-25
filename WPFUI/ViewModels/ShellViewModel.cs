namespace WPFUI.ViewModels;
public class ShellViewModel : Conductor<object>
{
    private CompanyModel _selectedCompany;
    private string _searchText;
    private readonly IWindowManager _manager = new WindowManager();

    public ShellViewModel()
    {
        PopulateList();
    }
        
    public BindableCollection<CompanyModel> Companies { get; set; } = new();

    public CompanyModel SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            _selectedCompany = value;
            NotifyOfPropertyChange(() => SelectedCompany);
            LoadCompany();
        }
    }
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            NotifyOfPropertyChange(() => SearchText);
            FilterCompanies();
        }
    }
    private void PopulateList()
    {
        var sql = new SqliteCrud(GlobalConfig.GetConnectionString());
        Companies.Clear();
        Companies = new BindableCollection<CompanyModel>(sql.ReadAllCompanies().OrderBy(x => x.Name));

        foreach (var company in Companies)
        {
            var contactIds = sql.ReadContactIdsByCompany(company.Id);

            foreach (var commIds in contactIds.Select(contactId => sql.ReadCommuncationIdByContact(contactId)))
            {
                foreach (var comm in commIds.Select(commId => sql.ReadCommunicationById(commId)).Where(comm => comm.FollowUpNeeded == true))
                {
                    company.HasFollowUps = true;
                    break;
                }

                break;
            }
        }
        NotifyOfPropertyChange(() => Companies);
    }

    private void FilterCompanies()
    {
        var view = (CollectionView)CollectionViewSource.GetDefaultView(Companies);
        view.Filter = UserFilter;
    }

    private bool UserFilter(object item)
    {
        if (String.IsNullOrEmpty(SearchText))
        {
            return true;
        }
        else
        {
            return (item as CompanyModel).Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    public void AddData()
    {
        _manager.ShowWindowAsync(new DataEntryViewModel(), null, null);
    }

    public void OpenNewsCentre()
    {
        _manager.ShowWindowAsync(new NewsViewModel(), null, null);
    }

    private void LoadCompany()
    {
        if (SelectedCompany != null)
        {
            ActivateItemAsync(new CompanyViewModel(SelectedCompany));
        }
    }
    public void RefreshList()
    {
        PopulateList();
    }

    public void ShowCompany()
    {
        _manager.ShowWindowAsync(new FullCompanyViewModel(SelectedCompany), null, null);
    }
}