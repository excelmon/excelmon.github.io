using Aspose.Pdf;
using AsposeLetters.Helpers;

namespace AsposeLetters.Sections
{
    internal class CustomerAddressSection
    {
        // === Measurement Constants (MM) ===
        private const double MARGIN_CUSTOMER_ADDRESS_MM = 133;
        private const double Y_POSITION_CUSTOMER_ADDRESS_MM = 196.4; // (page size 279.4 mm - 83 mm)
        private const double Y_LINE_SPACING_ADDRESS_MM = 3.75;
        

        public Document PdfDocument { get; set; }
        public Page Page { get; set; }
        public Address Address { get; set; }

        private double _yPosition;
        private readonly double _ySpacingAddress;
        private readonly double _marginCustomer;

        public CustomerAddressSection(Document pdfDocument, Page page, Address address)
        {
            PdfDocument = pdfDocument;
            Page = page;
            Address = address;
            _yPosition = PdfMeasurementUtils.FromMMToPoints(Y_POSITION_CUSTOMER_ADDRESS_MM);
            _ySpacingAddress = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_ADDRESS_MM);
            _marginCustomer = PdfMeasurementUtils.FromMMToPoints(MARGIN_CUSTOMER_ADDRESS_MM);
        }

        public void AddToDocument() 
        {
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginCustomer, _yPosition, "In Care of:");
            _yPosition -= _ySpacingAddress;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginCustomer, _yPosition, Address.AddressName);
            _yPosition -= _ySpacingAddress;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginCustomer, _yPosition, Address.AddressLine1);
            _yPosition -= _ySpacingAddress;

            if (Address.AddressLine2 != "" && Address.AddressLine2 != null)
            {
                PdfTextUtils.SimpleTextFragmentTimes(Page, _marginCustomer, _yPosition, Address.AddressLine2);
                _yPosition -= _ySpacingAddress;
            }

            string cityStateZip = Address.CityStateZip();
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginCustomer, _yPosition, cityStateZip);
            _yPosition -= _ySpacingAddress;

        }
    }
}
