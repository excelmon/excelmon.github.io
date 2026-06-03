namespace AsposeLetters.Sections
{
    /// <summary>
    /// Represents a mailing address with standard components.
    /// </summary>
    public interface IAddress
    {
        string AddressName { get; set; }
        string AddressLine1 { get; set; }
        string? AddressLine2 { get; set; }
        string City { get; set; }
        string State {  get; set; }
        string ZipCode { get; set; }

        string CityStateZip();
    }
}
