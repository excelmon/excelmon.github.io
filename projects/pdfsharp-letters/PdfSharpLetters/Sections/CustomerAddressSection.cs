using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.Helpers;

namespace PdfSharpLetters.Sections
{
    internal class CustomerAddressSection
    {
        private const double MARGIN_CUSTOMER_ADDRESS_MM = 133;
        private const double Y_POSITION_CUSTOMER_ADDRESS_MM = 83; // 83 mm from the TOP of the page
        private const double Y_LINE_SPACING_ADDRESS_MM = 3.75;

        private readonly PdfPage _page;
        private readonly XGraphics _gfx;
        private readonly Address _address;
        private double _yPosition;
        private readonly double _ySpacingAddress;
        private readonly double _marginCustomer;

        public CustomerAddressSection(PdfDocument pdfDocument, PdfPage page, XGraphics gfx, Address address)
        {
            _page = page;
            _gfx = gfx;
            _address = address;
            _yPosition = PdfMeasurementUtils.FromMMToPoints(Y_POSITION_CUSTOMER_ADDRESS_MM);
            _ySpacingAddress = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_ADDRESS_MM);
            _marginCustomer = PdfMeasurementUtils.FromMMToPoints(MARGIN_CUSTOMER_ADDRESS_MM);
        }

        public void AddToDocument()
        {
            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginCustomer, _yPosition, "In Care of:");
            _yPosition += _ySpacingAddress;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginCustomer, _yPosition, _address.AddressName);
            _yPosition += _ySpacingAddress;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginCustomer, _yPosition, _address.AddressLine1);
            _yPosition += _ySpacingAddress;

            if (!string.IsNullOrEmpty(_address.AddressLine2))
            {
                PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginCustomer, _yPosition, _address.AddressLine2);
                _yPosition += _ySpacingAddress;
            }

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginCustomer, _yPosition, _address.CityStateZip());
        }
    }
}
