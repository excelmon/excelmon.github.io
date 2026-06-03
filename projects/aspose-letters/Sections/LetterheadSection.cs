using Aspose.Pdf;
using AsposeLetters.Helpers;

namespace AsposeLetters.Sections
{
    internal class LetterheadSection
    {
        // === Measurement Constants (MM) ===
        private const double LETTER_PAGE_TOP_MARGIN_MM = 6.25;
        private const double RETURN_ADDRESS_MARGIN_MM = 10;
        private const double DATE_Y_OFFSET_MM = 19;

        public Document PdfDocument { get; set; }
        public Page Page { get; set; }

        // === Private Fields ===
        private double _yPosition;
        private readonly double _yLineSpacing;
        private readonly double _marginReturnAddress;

        public LetterheadSection(Document pdfDocument, Page page) 
        { 
            PdfDocument = pdfDocument;
            Page = page;
            _yPosition = PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.LETTER_HEIGHT_MM - LETTER_PAGE_TOP_MARGIN_MM); // 6.25 mm from top of letter size page
            _yLineSpacing = PdfMeasurementUtils.FromMMToPoints(PdfFontUtils.TIMES_9_LINE_HEIGHT_MM);
            _marginReturnAddress = PdfMeasurementUtils.FromMMToPoints(RETURN_ADDRESS_MARGIN_MM); // Left margin for return address and date
        }

        public void AddToDocument() 
        {
            // Return Address
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginReturnAddress, _yPosition, "Aku Ren Industries");
            _yPosition -= _yLineSpacing;
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginReturnAddress, _yPosition, "555 Main Street");
            _yPosition -= _yLineSpacing;
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginReturnAddress, _yPosition, "Dallas, TX 75201");

            // Letter Title
            PdfTextUtils.SimpleTextFragmentLetterTitle(Page, "===== DEMO LETTER =====");

            // Today's date formatted
            _yPosition -= PdfMeasurementUtils.FromMMToPoints(DATE_Y_OFFSET_MM);
            PdfTextUtils.SimpleTextFragmentTimesRed(Page, _marginReturnAddress, _yPosition, DateTime.Now.ToString("MMMM d, yyyy"));
        }
    }
}
