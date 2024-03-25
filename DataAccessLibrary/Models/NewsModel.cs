namespace DataAccessLibrary.Models;

public class NewsModel
{
    public int Id { get; set; }
    public string Source { get; set; }
    public string Priority { get; set; }
    public string Details { get; set; }
    public DateOnly DateDisplay { get; set; }
    public int Date { get; set; }
    public int CompanyId { get; set; }
    public string Company { get; set; }
}