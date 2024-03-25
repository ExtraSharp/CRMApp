namespace DataAccessLibrary.Models;

public class ContractModel
{
    public int Id { get; set; }
    public string?  PurchaseOrderNumber { get; set; }
    public string Designation { get; set; }
    public int StartDate { get; set; }
    public int EndDate { get; set; }

    public DateOnly StartDateDisplay { get; set; }
    public DateOnly EndDateDisplay { get; set; }
    public bool IsSigned { get; set; }
    public bool IsActive { get; set; }
    public int SentToInstructor { get; set; }
    public DateOnly? SentToInstructorDisplay { get; set; }
    public string TotalHours { get; set; }
    public int CompanyId { get; set; }
}