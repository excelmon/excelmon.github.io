using Aspose.Pdf;
using AsposeLetters.Helpers;

namespace AsposeLetters.Sections
{
    internal class SignatureSection
    {
        // === Measurement Constants (MM) ===
        private const double Y_LINE_SPACING_MM = 5.5;
        private const double MARGIN_LETTER_BODY_MM = 13;
        private const double MARGIN_SIGNATURE_MM = 44.70;

        public Document PdfDocument { get; set; }
        public Page Page { get; set; }
        public double YPosition { get; set; }
        private readonly double _yLineSpacing;
        private readonly double _marginLetterBody;
        private readonly double _marginSignature;
        private double _ySignature;

        public SignatureSection(Document pdfDocument, Page page, double yPosition) 
        { 
            PdfDocument = pdfDocument;
            Page = page;
            YPosition = yPosition;
            _yLineSpacing = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_MM);
            _marginLetterBody = PdfMeasurementUtils.FromMMToPoints(MARGIN_LETTER_BODY_MM);
            _marginSignature = PdfMeasurementUtils.FromMMToPoints(MARGIN_SIGNATURE_MM);
            _ySignature = yPosition; // keep original y position for signature values
        }

        public void AddToDocument() 
        {
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginLetterBody, YPosition, "Completed by:");
            YPosition -= _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginLetterBody, YPosition, "Company and Title:");
            YPosition -= _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginLetterBody, YPosition, "Date:");
            YPosition -= _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginLetterBody, YPosition, "Email:");

            // Signature values
            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginSignature, _ySignature, "Phil Berger");
            _ySignature -= _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginSignature, _ySignature, "Aku Ren Industries, Developer");
            _ySignature -= _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(Page, _marginSignature, _ySignature, DateTime.Now.ToString("MMMM d, yyyy"));
            _ySignature -= _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimesUnderlined(Page, _marginSignature, _ySignature, "berger.phil@gmail.com");
        }
    }
}
