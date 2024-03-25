namespace WPFUI.ViewModels
{
    public class DataEntryViewModel : Conductor<object>
    {
        public void AddCompany()
        {
            ActivateItemAsync(new CompanyDataViewModel());
        }

        public void AddContact()
        {
            ActivateItemAsync(new ContactDataViewModel());
        }
        public void AddNews()
        {
            ActivateItemAsync(new NewsDataViewModel());
        }
        public void AddComm()
        {
            ActivateItemAsync(new CommunicationDataViewModel());
        }
        public void AddContract()
        {
            ActivateItemAsync(new ContractDataViewModel());
        }
        public void AddCourse()
        {
            ActivateItemAsync(new CourseDataViewModel());
        }
    }
}
