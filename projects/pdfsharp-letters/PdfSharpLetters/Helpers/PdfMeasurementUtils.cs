using PdfSharp.Drawing;

namespace PdfSharpLetters.Helpers
{
    internal class PdfMeasurementUtils
    {
        // Precomputed character widths for Times New Roman, size 9 (in mm).
        // Used by CalculateTextRectangleHeight for word-wrap line counting.
        // These were measured empirically in the original Aspose project and remain valid
        // as a fallback. For live measurement use gfx.MeasureString() where available.
        public static readonly Dictionary<char, double> CharWidthsTimesSize9 = new Dictionary<char, double>()
        {
            { 'A', 2.29 }, { 'B', 2.12 }, { 'C', 2.12 }, { 'D', 2.29 }, { 'E', 1.94 },
            { 'F', 1.77 }, { 'G', 2.29 }, { 'H', 2.29 }, { 'I', 1.06 }, { 'J', 1.24 },
            { 'K', 2.29 }, { 'L', 1.94 }, { 'M', 2.82 }, { 'N', 2.29 }, { 'O', 2.29 },
            { 'P', 1.77 }, { 'Q', 2.29 }, { 'R', 2.12 }, { 'S', 1.77 }, { 'T', 1.94 },
            { 'U', 2.29 }, { 'V', 2.29 }, { 'W', 3.00 }, { 'X', 2.29 }, { 'Y', 2.29 },
            { 'Z', 1.94 }, { 'a', 1.41 }, { 'b', 1.59 }, { 'c', 1.41 }, { 'd', 1.59 },
            { 'e', 1.41 }, { 'f', 1.06 }, { 'g', 1.59 }, { 'h', 1.59 }, { 'i', 0.88 },
            { 'j', 0.88 }, { 'k', 1.59 }, { 'l', 0.88 }, { 'm', 2.47 }, { 'n', 1.59 },
            { 'o', 1.59 }, { 'p', 1.59 }, { 'q', 1.59 }, { 'r', 1.06 }, { 's', 1.24 },
            { 't', 0.88 }, { 'u', 1.59 }, { 'v', 1.59 }, { 'w', 2.29 }, { 'x', 1.59 },
            { 'y', 1.59 }, { 'z', 1.41 }, { '.', 0.79 }, { ',', 0.79 }, { ';', 0.88 },
            { '?', 1.41 }, { ':', 0.88 }, { '#', 1.59 }, { '%', 2.64 }, { '$', 1.59 },
            { '&', 2.47 }, { '\'', 0.57 }, { '(', 1.06 }, { ')', 1.06 }, { '-', 1.06 },
            { '_', 1.59 }, { '+', 1.79 }, { '*', 1.59 }, { '[', 1.06 }, { ']', 1.06 },
            { ' ', 0.79 }, { '1', 1.59 }, { '2', 1.59 }, { '3', 1.59 }, { '4', 1.59 },
            { '5', 1.59 }, { '6', 1.59 }, { '7', 1.59 }, { '8', 1.59 }, { '9', 1.59 },
            { '0', 1.59 }
        };

        /// <summary>
        /// Convert millimeters to PDF points (1 point = 1/72 inch).
        /// </summary>
        public static double FromMMToPoints(double mm)
        {
            return mm / 0.352778;
        }

        /// <summary>
        /// Convert PDF points to millimeters.
        /// </summary>
        public static double FromPointsToMm(double point)
        {
            return point * 0.352778;
        }

        /// <summary>
        /// Measures the rendered width of a string using live PdfSharp font metrics.
        /// Prefer this over the static char-width dictionary when an XGraphics context is available.
        /// </summary>
        public static double MeasureStringWidth(XGraphics gfx, string text, XFont font)
        {
            return gfx.MeasureString(text, font).Width;
        }

        /// <summary>
        /// Calculates the X position that centers a string of the given width on a page.
        /// </summary>
        public static double CalculateCenteredXPosition(double pageWidth, double textWidth)
        {
            return (pageWidth / 2) - (textWidth / 2);
        }

        /// <summary>
        /// Splits text into words and counts required lines given an available line width (in mm).
        /// Uses the static CharWidthsTimesSize9 dictionary — accurate for Times New Roman 9pt body text.
        /// </summary>
        public static double CalculateTextRectangleHeight(string text, double totalLineWidthMm)
        {
            Console.WriteLine("calculating rectangle height");
            string[] words = text.Split(' ');
            int lineCount = 1;
            double remainingLineWidth = totalLineWidthMm;

            foreach (string word in words)
            {
                var result = CalculateLineBreak(word, remainingLineWidth, lineCount);
                remainingLineWidth = result.remainingWidth;
                lineCount = result.lineCount;
                if (result.lineReturn)
                {
                    remainingLineWidth = totalLineWidthMm - result.wordWidth;
                }
            }

            Console.WriteLine($"final line count: {lineCount}\n");
            double lineHeightPoints = FromMMToPoints(PdfFontUtils.TIMES_9_LINE_HEIGHT_MM);
            return lineHeightPoints * lineCount;
        }

        private static (double remainingWidth, int lineCount, bool lineReturn, double wordWidth) CalculateLineBreak(
            string word, double availableLineWidth, int trackLineCount)
        {
            const double whiteSpaceWidthMm = 0.79;
            double wordWidth = CalculateWordWidthTimes9(word) + whiteSpaceWidthMm;
            bool lineReturn = false;

            if (wordWidth > availableLineWidth)
            {
                lineReturn = true;
                availableLineWidth = 0;
                Console.WriteLine($"New Line before: {word}");
                trackLineCount++;
            }
            else
            {
                availableLineWidth -= wordWidth;
            }

            return (availableLineWidth, trackLineCount, lineReturn, wordWidth);
        }

        private static double CalculateWordWidthTimes9(string word)
        {
            double wordMmLength = 0;
            foreach (char c in word)
            {
                if (CharWidthsTimesSize9.TryGetValue(c, out double letterWidth))
                    wordMmLength += letterWidth;
                else
                {
                    Console.WriteLine($"{c} NOT in CharWidthsTimesSize9");
                    wordMmLength += 1; // default fallback
                }
            }
            return wordMmLength;
        }
    }
}
