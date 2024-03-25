namespace WPFUI.ViewModels
{
    public class CompanyViewModel : Screen
    {
        private CompanyModel _company;
        private IWindowManager _manager = new WindowManager();
        public CompanyViewModel(CompanyModel company)
        {
            _company = company;
            Name = _company.Name;
            Address = _company.FullAddress;
            Website = _company.Website;
        }

        public string Name { get; }
        public string Address { get; }
        public string Website { get; set; }

        public void Edit()
        {
            dynamic settings = new ExpandoObject();
            settings.Title = "Edit Company";
            settings.ResizeMode = ResizeMode.NoResize;
            _manager.ShowWindowAsync(new CompanyDataViewModel(_company), null, settings);            
        }

        public void FullView()
        {
            _manager.ShowWindowAsync(new FullCompanyViewModel(_company));
        }
    }
}
