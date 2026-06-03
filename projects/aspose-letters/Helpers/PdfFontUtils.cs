using Aspose.Pdf.Text;

namespace AsposeLetters.Helpers
{
    internal class PdfFontUtils
    {
        // Line height constants
        public const double TIMES_9_LINE_HEIGHT_MM = 3.175;

        public static void SetFontTimesBold(TextFragment textFragment)
        {
            textFragment.TextState.FontSize = 9;
            textFragment.TextState.Font = FontRepository.FindFont("Times New Roman");
            textFragment.TextState.FontStyle = FontStyles.Bold;
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
            textFragment.IsInLineParagraph = true;
        }

        public static void SetFontTimesBoldTitle(TextFragment textFragment)
        {
            textFragment.TextState.FontSize = 14;
            textFragment.TextState.Font = FontRepository.FindFont("Times-BoldItalic");
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
            textFragment.IsInLineParagraph = true;
        }

        public static void SetFontArial(TextFragment textFragment)
        {
            textFragment.TextState.FontSize = 8;
            textFragment.TextState.Font = FontRepository.FindFont("Arial");
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
            textFragment.IsInLineParagraph = true;
        }

        public static void SetFontTimes(TextFragment textFragment)
        {
            textFragment.TextState.FontSize = 9;
            textFragment.TextState.Font = FontRepository.FindFont("Times New Roman");
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
            textFragment.IsInLineParagraph = true;
        }

        public static void SetFontTimesRed(TextFragment textFragment)
        {
            textFragment.TextState.FontSize = 9;
            textFragment.TextState.Font = FontRepository.FindFont("Times New Roman");
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.Red;
            textFragment.IsInLineParagraph = true;
        }

        public static void SetFontDollarLine(TextFragment textFragment)
        {
            textFragment.TextState.FontSize = 11; // target 0.16 mm line thickness to match underline TextSegment line thinkness
            textFragment.TextState.Font = FontRepository.FindFont("Times New Roman");
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
            textFragment.IsInLineParagraph = true;
        }

        public static void SetFontTextSegment(TextSegment textSegment)
        {
            textSegment.TextState.FontSize = 9;
            textSegment.TextState.Font = FontRepository.FindFont("Times New Roman");
            textSegment.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
        }

        public static void SetFontTextSegmentTimesDarkBlue(TextSegment textSegment)
        {
            textSegment.TextState.FontSize = 9;
            textSegment.TextState.Font = FontRepository.FindFont("Times New Roman");
            textSegment.TextState.ForegroundColor = Aspose.Pdf.Color.DarkBlue;
        }

        public static void SetFontTextSegmentArialDarkSlateBlue(TextSegment textSegment)
        {
            textSegment.TextState.FontSize = 9;
            textSegment.TextState.Font = FontRepository.FindFont("Arial");
            textSegment.TextState.ForegroundColor = Aspose.Pdf.Color.DarkSlateBlue;
        }

        public static void SetFontTextSegmentArialMediumSlateBlue(TextSegment textSegment)
        {
            textSegment.TextState.FontSize = 9;
            textSegment.TextState.Font = FontRepository.FindFont("Arial");
            textSegment.TextState.ForegroundColor = Aspose.Pdf.Color.MediumSlateBlue;
        }

        /// <summary>
        /// Converts double to string: Comma separated with 2 decimals (1,000.00)
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string FormatAmountAsString(double amount)
        {
            string commaNumberAsString = string.Format("{0:N2}", amount);
            return commaNumberAsString;
        }
    }
}
