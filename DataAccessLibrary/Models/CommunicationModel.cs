namespace DataAccessLibrary.Models;

public class CommunicationModel
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Means { get; set; }
    public string Details { get; set; }
    public int Date { get; set; }
    public DateOnly DateDisplay { get; set; }
    public bool FollowUpNeeded { get; set; }
    public int ContactId { get; set; }
}