namespace DataAccessLibrary.Models;

public class FullContactModel
{
    public int Id { get; set; }
    public string Salutation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{LastName}, {FirstName}";
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Notes { get; set; }
    public string Position { get; set; }
    public string Company { get; set; }
}