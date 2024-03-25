namespace DataAccessLibrary;

public class SqliteCrud
{
    private readonly string _connectionString;
    private readonly SqliteDataAccess _db = new();

    public SqliteCrud(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateCompany(CompanyModel company)
    {
        string sql = "insert into Companies (Name, Street, Street2, PostalCode, City, Website) values (@Name, @Street, @Street2, @PostalCode, @City, @Website)";
        _db.SaveData(sql, new { company.Name, company.Street, company.Street2, company.PostalCode, company.City, company.Website }, _connectionString);
    }

    public void CreateContact(FullContactModel contact)
    {
        // Save basic contact info
        var sql = "insert into Contacts (FirstName, LastName, Salutation, Position, Email, PhoneNumber, Notes) values (@FirstName, @LastName, @Salutation, @Position, @Email, @PhoneNumber, @Notes)";
        _db.SaveData(sql, new { contact.FirstName, contact.LastName, contact.Salutation, contact.Position, contact.Email, contact.PhoneNumber, contact.Notes }, _connectionString);

        // Load ID of new contact
        sql = "select Id from Contacts where FirstName = @FirstName and LastName = @LastName;";
        var contactId = _db.LoadData<IdLookupModel, dynamic>(
            sql,
            new { contact.FirstName, contact.LastName },
            _connectionString).First().Id;

        sql = "select Id from Companies where Name = @Company;";
        var companyId = _db.LoadData<IdLookupModel, dynamic>(
            sql,
            new { contact.Company },
            _connectionString).First().Id;

        sql = "insert into CompanyContact (CompanyId, ContactId) values (@CompanyId, @ContactId)";
        _db.SaveData(sql, new { CompanyId = companyId, ContactId = contactId }, _connectionString);
    }

    public void CreateCommunication(CommunicationModel comm)
    {
        var sql = "insert into Communications (Subject, Means, Date, FollowUpNeeded, Details) values (@Subject, @Means, @Date, @FollowUpNeeded, @Details)";
        _db.SaveData(sql, new { comm.Subject, comm.Means, comm.Date, comm.FollowUpNeeded, comm.Details }, _connectionString);

        sql = "select Id from Communications where Date = @Date and Details = @Details;";
        var communicationId = _db.LoadData<IdLookupModel, dynamic>(sql, new { comm.Date, comm.Details }, _connectionString).First().Id;

        sql = "insert into ContactCommunication (ContactId, CommunicationId) values (@ContactId, @CommunicationId)";
        _db.SaveData(sql, new { comm.ContactId, CommunicationId = communicationId }, _connectionString);
    }
    public void CreateNews(NewsModel news)
    {
        string sql = "insert into News (Source, Priority, Date, Details) values (@Source, @Priority, @Date, @Details)";
        _db.SaveData(sql, new { news.Source, news.Priority, news.Date, news.Details }, _connectionString);

        sql = "select Id from News where Date = @Date and Details = @Details;";
        int newsId = _db.LoadData<IdLookupModel, dynamic>(sql, new { news.Date, news.Details }, _connectionString).First().Id;

        sql = "insert into CompanyNews (CompanyId, NewsId) values (@CompanyId, @NewsId)";
        _db.SaveData(sql, new { news.CompanyId, NewsId = newsId }, _connectionString);
    }

    public void CreateContract(ContractModel contract)
    {
        string sql = "insert into Contracts (PurchaseOrderNumber, StartDate, EndDate, SentToInstructor, TotalHours, IsSigned, IsActive, Designation) values " +
                     "(@PurchaseOrderNumber, @StartDate, @EndDate, @SentToInstructor, @TotalHours, @IsSigned, @IsActive, @Designation)";
        _db.SaveData(sql, new
        {
            contract.PurchaseOrderNumber,
            contract.StartDate,
            contract.EndDate,
            contract.SentToInstructor,
            contract.TotalHours,
            contract.IsSigned,
            contract.IsActive,
            contract.Designation
        }, _connectionString);

        sql = "select Id from Contracts where Designation = @Designation";
        var contractId = _db.LoadData<IdLookupModel, dynamic>(sql, new { contract.Designation }, _connectionString).First().Id;

        sql = "insert into CompanyContract (CompanyId, ContractId) values (@CompanyId, @ContractId)";
        _db.SaveData(sql, new { contract.CompanyId, ContractId = contractId }, _connectionString);
    }

    public void CreateCourse(CourseModel course)
    {
        var sql = "insert into Courses (Name, UeLength, TotalUe, UePerSession, CompletedUe, CompletedAsOf, Book, BookOrdered, BookReimbursed, Notes) values " +
                  "(@Name, @UeLength, @TotalUe, @UePerSession, @CompletedUe, @CompletedAsOf, @Book, @BookOrdered, @BookReimbursed, @Notes)";
        _db.SaveData(sql, new
        {
            course.Name,
            course.UeLength,
            course.TotalUe,
            course.UePerSession,
            course.CompletedUe,
            course.CompletedAsOf,
            course.Book,
            course.BookOrdered,
            course.BookReimbursed,
            course.Notes
        }, _connectionString);

        sql = "select Id from Courses where Name = @Name";
        int courseId = _db.LoadData<IdLookupModel, dynamic>(sql, new { course.Name }, _connectionString).First().Id;

        sql = "insert into ContractCourse (ContractId, CourseId) values (@ContractId, @CourseId)";
        _db.SaveData(sql, new { course.ContractId, CourseId = courseId }, _connectionString);

    }

    public List<CompanyModel> ReadAllCompanies()
    {
        string sql = "select Id, Name, Street, Street2, PostalCode, City, Website from Companies";

        return _db.LoadData<CompanyModel, dynamic>(sql, new { }, _connectionString);
    }

    public List<NewsModel> ReadAllNews()
    {
        string sql = "select Id, Source, Priority, Date, Details from News";

        return _db.LoadData<NewsModel, dynamic>(sql, new { }, _connectionString);
    }

    public int ReadCompanyIdByNewsId(int id)
    {
        string sql = "select CompanyId from CompanyNews where NewsId = @NewsId";

        return _db.LoadData<int, dynamic>(sql, new { NewsId = id}, _connectionString).FirstOrDefault();   
    }

    public List<int> ReadContactIdsByCompany(int id)
    {
        string sql = "select ContactId from CompanyContact where CompanyId = @CompanyId";

        return _db.LoadData<int, dynamic>(sql, new { CompanyId = id }, _connectionString);
    }
    public List<int> ReadCourseIdsByContract(int id)
    {
        string sql = "select CourseId from ContractCourse where ContractId = @ContractId";

        return _db.LoadData<int, dynamic>(sql, new { ContractId = id }, _connectionString);
    }
    public List<int> ReadContractIdsByCompany(int id)
    {
        string sql = "select ContractId from CompanyContract where CompanyId = @CompanyId";

        return _db.LoadData<int, dynamic>(sql, new { CompanyId = id }, _connectionString);
    }
    public List<int> ReadNewsIdsByCompany(int id)
    {
        string sql = "select NewsId from CompanyNews where CompanyId = @CompanyId";

        return _db.LoadData<int, dynamic>(sql, new { CompanyId = id }, _connectionString);
    }
    public List<int> ReadCommuncationIdByContact(int id)
    {
        string sql = "select CommunicationId from ContactCommunication where ContactId = @ContactId";

        return _db.LoadData<int, dynamic>(sql, new { ContactId = id }, _connectionString);
    }
    public string ReadCommpanyNameById(int id)
    {
        string sql = "select Name from Companies where Id = @Id";

        return _db.LoadData<string, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();
    }

    public CommunicationModel ReadCommunicationById(int id)
    {
        var sql = "select Id, Subject, Means, Date, FollowUpNeeded, Details from Communications where Id = @Id";

        return _db.LoadData<CommunicationModel, dynamic>(sql, new { id }, _connectionString).FirstOrDefault();
    }

    public FullContactModel ReadContactById(int id)
    {
        var sql = "select Id, FirstName, LastName, Position, Email, PhoneNumber from Contacts where Id = @Id";

        return _db.LoadData<FullContactModel, dynamic>(sql, new { id }, _connectionString).FirstOrDefault();
    }
    public ContractModel ReadContractById(int id)
    {
        var sql = "select Id, StartDate, EndDate, PurchaseOrderNumber, Designation, TotalHours, IsActive, IsSigned, SentToInstructor from Contracts where Id = @Id";

        return _db.LoadData<ContractModel, dynamic>(sql, new { id }, _connectionString).FirstOrDefault();
    }

    public CourseModel ReadCourseById(int id)
    {
        var sql = "select Id, Name, TotalUe, CompletedUe, CompletedAsOf, UePerSession, UeLength from Courses where Id = @Id";

        return _db.LoadData<CourseModel, dynamic>(sql, new { id }, _connectionString).FirstOrDefault();
    }
    public NewsModel ReadNewsById(int id)
    {
        var sql = "select Id, Source, Priority, Details, Date from News where Id = @Id";

        return _db.LoadData<NewsModel, dynamic>(sql, new { id }, _connectionString).FirstOrDefault();
    }

    public List<BasicCompanyModel> LoadCompanies()
    {
        var sql = "select Id, Name from Companies";

        return _db.LoadData<BasicCompanyModel, dynamic>(sql, new { }, _connectionString);
    }

    public void UpdateCompany(CompanyModel company)
    {
        var sql = "update Companies set Name = @Name, Street = @Street, Street2 = @Street2, PostalCode = @PostalCode, City = @City, Website = @Website, Notes = @Notes where Id = @Id";

        _db.SaveData(sql, company, _connectionString);
    }
    public void UpdateCourse(CourseModel course)
    {
        string sql = "update Courses set Name = @Name, UeLength = @UeLength, TotalUe = @TotalUe, UePerSession = @UePerSession, CompletedUe = @CompletedUe, CompletedAsOf = @CompletedAsOf, Book = @Book, BookOrdered = @BookOrdered, BookReimbursed = @BookReimbursed, Notes = @Notes where Id = @Id";

        _db.SaveData(sql, course, _connectionString);
    }
    public void UpdateContract(ContractModel contract)
    {
        string sql = "update Contracts set PurchaseOrderNumber = @PurchaseOrderNumber, StartDate = @StartDate, EndDate = @EndDate, SentToInstructor = @SentToInstructor, TotalHours = @TotalHours, IsSigned = @IsSigned, IsActive = @IsActive, Designation = @Designation where Id = @Id";

        _db.SaveData(sql, contract, _connectionString);
    }
    public void UpdateNews(NewsModel news)
    {
        string sql = "update News set Source = @Source, Priority = @Priority, Date = @Date, Details = @Details where Id = @Id";

        _db.SaveData(sql, news, _connectionString);
    }
    public void UpdateCommunication(CommunicationModel comm)
    {
        string sql = "update Communications set Date = @Date, Details = @Details, FollowUpNeeded = @FollowUpNeeded where Id = @Id";

        _db.SaveData(sql, comm, _connectionString);
    }
}