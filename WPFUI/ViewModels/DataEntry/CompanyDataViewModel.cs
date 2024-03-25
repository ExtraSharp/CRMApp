using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.ViewModels.DataEntry
{
    public class CompanyDataViewModel : Screen
    {
        private CompanyModel _company;
        private string _mode;
        public CompanyDataViewModel()
        {
            _company = new CompanyModel();
            _mode = "add";
            ButtonText = "Save";
        }
        public CompanyDataViewModel(CompanyModel company)
        {
            _company = company;           
            _mode = "edit";
            ButtonText = "Update";
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
                sql.CreateCompany(_company);

                MessageBox.Show("Company successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                sql.UpdateCompany(_company);

                MessageBox.Show("Changes successfully saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.TryCloseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string ButtonText { get; set; }

        public string Name
        {
            get { return _company.Name; }
            set
            {
                if (_company.Name != value)
                {
                    _company.Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }
        public string Street
        {
            get { return _company.Street; }
            set
            {
                if (_company.Street != value)
                {
                    _company.Street = value;
                    NotifyOfPropertyChange(() => Street);
                }
            }
        }
        public string Street2
        {
            get { return _company.Street2; }
            set
            {
                if (_company.Street2 != value)
                {
                    _company.Street2 = value;
                    NotifyOfPropertyChange(() => Street2);
                }
            }
        }
        public string City
        {
            get { return _company.City; }
            set
            {
                if (_company.City != value)
                {
                    _company.City = value;
                    NotifyOfPropertyChange(() => City);
                }
            }
        }
        public string PostalCode
        {
            get { return _company.PostalCode; }
            set
            {
                if (_company.PostalCode != value)
                {
                    _company.PostalCode = value;
                    NotifyOfPropertyChange(() => PostalCode);
                }
            }
        }
        public string Website
        {
            get { return _company.Website; }
            set
            {
                if (_company.Website != value)
                {
                    _company.Website = value;
                    NotifyOfPropertyChange(() => Website);
                }
            }
        }
        public string Notes
        {
            get { return _company.Notes; }
            set
            {
                if (_company.Notes != value)
                {
                    _company.Notes = value;
                    NotifyOfPropertyChange(() => Notes);
                }
            }
        }
        private void ResetFields()
        {
            Name = string.Empty;
            Street = string.Empty;
            Street2 = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
            Website = string.Empty; 
            Notes = string.Empty;   
        }
    }
}
