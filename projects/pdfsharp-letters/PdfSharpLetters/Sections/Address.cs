namespace PdfSharpLetters.Sections
{
    public class Address : IAddress
    {
        public string AddressName { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public Address(string addressName, string addressLine1, string? addressLine2, string city, string state, string zipcode)
        {
            AddressName = addressName;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            ZipCode = zipcode;
        }

        public string CityStateZip()
        {
            return $"{City}, {State} {ZipCode}";
        }
    }
}
