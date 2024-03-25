namespace DataAccessLibrary.Models;

public class CompanyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string Street2 { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Website { get; set; }
    public string Notes { get; set; }
    public bool HasFollowUps { get; set; }
    public string FullAddress
    {
        get { return $"{ Street }, { PostalCode } { City }"; }
    }
}