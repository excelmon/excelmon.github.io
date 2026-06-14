using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.Helpers;

namespace PdfSharpLetters.Sections
{
    internal class LetterheadSection
    {
        // === Measurement Constants (MM) ===
        private const double LETTER_PAGE_TOP_MARGIN_MM = 6.25;
        private const double RETURN_ADDRESS_MARGIN_MM = 10;
        private const double DATE_Y_OFFSET_MM = 19;

        private readonly PdfDocument _pdfDocument;
        private readonly PdfPage _page;
        private readonly XGraphics _gfx;

        private double _yPosition;
        private readonly double _yLineSpacing;
        private readonly double _marginReturnAddress;

        public LetterheadSection(PdfDocument pdfDocument, PdfPage page, XGraphics gfx)
        {
            _pdfDocument = pdfDocument;
            _page = page;
            _gfx = gfx;
            // Top margin: 6.25 mm from the top of the page
            _yPosition = PdfMeasurementUtils.FromMMToPoints(LETTER_PAGE_TOP_MARGIN_MM);
            _yLineSpacing = PdfMeasurementUtils.FromMMToPoints(PdfFontUtils.TIMES_9_LINE_HEIGHT_MM);
            _marginReturnAddress = PdfMeasurementUtils.FromMMToPoints(RETURN_ADDRESS_MARGIN_MM);
        }

        public void AddToDocument()
        {
            // Return Address
            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginReturnAddress, _yPosition, "Aku Ren Industries");
            _yPosition += _yLineSpacing;
            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginReturnAddress, _yPosition, "555 Main Street");
            _yPosition += _yLineSpacing;
            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginReturnAddress, _yPosition, "Dallas, TX 75201");

            // Letter Title (centered)
            PdfTextUtils.SimpleTextFragmentLetterTitle(_gfx, _page.Width, "===== DEMO LETTER =====");

            // Today's date
            _yPosition += PdfMeasurementUtils.FromMMToPoints(DATE_Y_OFFSET_MM);
            PdfTextUtils.SimpleTextFragmentTimesRed(_gfx, _marginReturnAddress, _yPosition,
                DateTime.Now.ToString("MMMM d, yyyy"));
        }
    }
}
