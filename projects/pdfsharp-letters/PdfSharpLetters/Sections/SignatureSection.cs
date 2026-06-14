using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.Helpers;

namespace PdfSharpLetters.Sections
{
    internal class SignatureSection
    {
        private const double Y_LINE_SPACING_MM = 5.5;
        private const double MARGIN_LETTER_BODY_MM = 13;
        private const double MARGIN_SIGNATURE_MM = 44.70;

        private readonly PdfPage _page;
        private readonly XGraphics _gfx;
        private double _yPosition;
        private double _ySignature;
        private readonly double _yLineSpacing;
        private readonly double _marginLetterBody;
        private readonly double _marginSignature;

        public SignatureSection(PdfDocument pdfDocument, PdfPage page, XGraphics gfx, double yPosition)
        {
            _page = page;
            _gfx = gfx;
            _yPosition = yPosition;
            _ySignature = yPosition;
            _yLineSpacing = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_MM);
            _marginLetterBody = PdfMeasurementUtils.FromMMToPoints(MARGIN_LETTER_BODY_MM);
            _marginSignature = PdfMeasurementUtils.FromMMToPoints(MARGIN_SIGNATURE_MM);
        }

        public void AddToDocument()
        {
            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginLetterBody, _yPosition, "Completed by:");
            _yPosition += _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginLetterBody, _yPosition, "Company and Title:");
            _yPosition += _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginLetterBody, _yPosition, "Date:");
            _yPosition += _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginLetterBody, _yPosition, "Email:");

            // Signature values (right column, starting at original yPosition)
            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginSignature, _ySignature, "Phil Berger");
            _ySignature += _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginSignature, _ySignature, "Aku Ren Industries, Developer");
            _ySignature += _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimes(_gfx, _marginSignature, _ySignature,
                DateTime.Now.ToString("MMMM d, yyyy"));
            _ySignature += _yLineSpacing;

            PdfTextUtils.SimpleTextFragmentTimesUnderlined(_gfx, _marginSignature, _ySignature,
                "berger.phil@gmail.com");
        }
    }
}
