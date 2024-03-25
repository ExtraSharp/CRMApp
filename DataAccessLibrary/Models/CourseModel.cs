namespace DataAccessLibrary.Models;

public class CourseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UeLength { get; set; }
    public string UePerSession { get; set; }
    public string TotalUe { get; set; }
    public string CompletedUe { get; set; }
    public int CompletedAsOf { get; set; }
    public DateOnly CompletedAsOfDisplay { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public string Book { get; set; }
    public int BookOrdered { get; set; }
    public int BookReimbursed { get; set; }

    public DateOnly BookOrderedDisplay { get; set; }
    public DateOnly BookReimbursedDisplay { get; set; }
    public string Notes { get; set; }
    public int ContractId { get; set; }
}