using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace AsposeLetters.Helpers
{
    internal class PdfMeasurementUtils
    {
        // Precomputed character widths for Times New Roman, size 9
        // Additional dictionarys can be quickly created for other font families/sizes from the txt output file of DrawMeasurementTestLetter.cs
        public static readonly Dictionary<char, double> CharWidthsTimesSize9 = new Dictionary<char, double>()
        {
            { 'A', 2.29 },
            { 'B', 2.12 },
            { 'C', 2.12 },
            { 'D', 2.29 },
            { 'E', 1.94 },
            { 'F', 1.77 },
            { 'G', 2.29 },
            { 'H', 2.29 },
            { 'I', 1.06 },
            { 'J', 1.24 },
            { 'K', 2.29 },
            { 'L', 1.94 },
            { 'M', 2.82 },
            { 'N', 2.29 },
            { 'O', 2.29 },
            { 'P', 1.77 },
            { 'Q', 2.29 },
            { 'R', 2.12 },
            { 'S', 1.77 },
            { 'T', 1.94 },
            { 'U', 2.29 },
            { 'V', 2.29 },
            { 'W', 3.00 },
            { 'X', 2.29 },
            { 'Y', 2.29 },
            { 'Z', 1.94 },
            { 'a', 1.41 },
            { 'b', 1.59 },
            { 'c', 1.41 },
            { 'd', 1.59 },
            { 'e', 1.41 },
            { 'f', 1.06 },
            { 'g', 1.59 },
            { 'h', 1.59 },
            { 'i', 0.88 },
            { 'j', 0.88 },
            { 'k', 1.59 },
            { 'l', 0.88 },
            { 'm', 2.47 },
            { 'n', 1.59 },
            { 'o', 1.59 },
            { 'p', 1.59 },
            { 'q', 1.59 },
            { 'r', 1.06 },
            { 's', 1.24 },
            { 't', 0.88 },
            { 'u', 1.59 },
            { 'v', 1.59 },
            { 'w', 2.29 },
            { 'x', 1.59 },
            { 'y', 1.59 },
            { 'z', 1.41 },
            { '.', 0.79 },
            { ',', 0.79 },
            { ';', 0.88 },
            { '?', 1.41 },
            { ':', 0.88 },
            { '#', 1.59 },
            { '%', 2.64 },
            { '$', 1.59 },
            { '&', 2.47 },
            { '\'', 0.57 },
            { '(', 1.06 },
            { ')', 1.06 },
            { '-', 1.06 },
            { '_', 1.59 },
            { '+', 1.79 },
            { '*', 1.59 },
            { '[', 1.06 },
            { ']', 1.06 },
            { ' ', 0.79 },
            { '1', 1.59 },
            { '2', 1.59 },
            { '3', 1.59 },
            { '4', 1.59 },
            { '5', 1.59 },
            { '6', 1.59 },
            { '7', 1.59 },
            { '8', 1.59 },
            { '9', 1.59 },
            { '0', 1.59 }
        };

        /// <summary>
        /// Convert millimeters to PDF points
        /// </summary>
        /// <param name="mm">Input mm</param>
        /// <returns>PDF Points</returns>
        public static double FromMMToPoints(double mm)
        {
            double pdfPoint = mm / .352778; // each point is 1/72 (one, seventy-second) inch 
            return pdfPoint;
        }

        /// <summary>
        /// Convert PDF points to millimeters
        /// </summary>
        /// <param name="point">Input PDF points</param>
        /// <returns>mm measurment</returns>
        public static double FromPointsToMm(double point)
        {
            double mmLenght = point * .352778;
            return mmLenght;
        }

        /// <summary>
        /// Calculate the centered x margin position based on text width and page widith
        /// </summary>
        /// <param name="page"></param>
        /// <param name="textFragment"></param>
        /// <returns></returns>
        public static double CalculateCenteredXPosition(Page page, TextFragment textFragment)
        {
            double textWidth = PdfMeasurementUtils.CalculateWidth(textFragment);
            double centerX = page.PageInfo.Width / 2;
            return centerX - textWidth / 2;
        }

        /// <summary>
        /// Calculates the width of a TextFragment. The font family and size must be assigned first.
        /// </summary>
        /// <param name="textFragment"></param>
        /// <returns>Calcuated Text Width</returns>
        public static double CalculateWidth(TextFragment textFragment)
        {
            double textWidth = textFragment.TextState.Font.MeasureString(textFragment.Text, textFragment.TextState.FontSize);
            return textWidth;
        }

        /// <summary>
        /// <para>Calculates the maximum width value of a TextFragment that has "\n" or "\r\n" included in the text.</para>
        /// <para>Must have consistent designated carriage return type.</para>
        /// </summary>
        /// <param name="textFragment">TextFragment with "\n" or "\r\n"</param>
        /// <param name="splitCharacter">Carriage return as string</param>
        /// <returns>Max width measurement of the text</returns>
        public static double CalculateWidthMultipleRows(TextFragment textFragment, string splitCharacter)
        {
            List<string> textRows = new List<string>(textFragment.Text.Split(new string[] { splitCharacter }, StringSplitOptions.None));
            double maxTextWidth = 0.00;
            foreach (string line in textRows)
            {
                double textWidth = textFragment.TextState.Font.MeasureString(line, textFragment.TextState.FontSize);
                if (maxTextWidth < textWidth) { maxTextWidth = textWidth; }
            }
            return maxTextWidth;
        }

        public static double CalculateHeight(TextFragment textFragment)
        {
            double fontSize = textFragment.TextState.FontSize;
            double textHeight = textFragment.TextState.LineSpacing == 0 ? fontSize : textFragment.TextState.LineSpacing;
            return textHeight;
        }

        /// <summary>
        /// <para>Calculate the height of a TextFragment that has "\n" or "\r\n" included in the text.</para>
        /// <para>Must have consistent designated carriage return type.</para>
        /// </summary>
        /// <param name="textFragment">TextFragment with "\n" or "\r\n"</param>
        /// <param name="splitCharacter">Carriage return as string</param>
        /// <returns>Total height measurement</returns>
        public static double CalculateHeightMultipleRows(TextFragment textFragment, string splitCharacter)
        {
            List<string> textRows = new List<string>(textFragment.Text.Split(new string[] { splitCharacter }, StringSplitOptions.None));
            double totalTextHeight = 0.00;
            double fontSize = textFragment.TextState.FontSize;
            for (int i = 0; i < textRows.Count; i++)
            {
                double textHeight = textFragment.TextState.LineSpacing == 0 ? fontSize : textFragment.TextState.LineSpacing;
                totalTextHeight += textHeight;
            }
            return totalTextHeight;
        }

        /// <summary>
        /// <para>Splits text into individual words. Calculates the width of each word by adding each char width.</para>
        /// <para>Tracks when a new line is required by comparing the word width to the available line width.</para>
        /// </summary>
        /// <param name="text">String that will be split by " "</param>
        /// <param name="totalLineWidth">The millimeter line width</param>
        /// <returns>Height of the rectangle. (text height * line count)</returns>
        public static double CalculateTextRectangleHeight(string text, double totalLineWidth) 
        {
            Console.WriteLine("calculating rectangle height");
            string[] myWordList = text.Split(' ');
            int lineCount = 1;
            double remainingLineWidth = totalLineWidth;
            for (int i = 0; i < myWordList.Length; i++)
            {
                var result = CalculateLineBreak(myWordList[i], remainingLineWidth, lineCount);
                remainingLineWidth = result.Item1;
                lineCount = result.Item2;
                if (result.Item3)
                {
                    // Subtract this word's width from the remainingLineWidth of new line
                    remainingLineWidth = totalLineWidth - result.Item4; 
                }
            }
            Console.WriteLine($"final line count: {lineCount}\n");
            double textHeight = FromMMToPoints(3.175); // Line height for Times New Roman size 9
            double finalHeight = textHeight * lineCount;
            //Console.WriteLine($"final Height: {finalHeight}");
            return finalHeight;
        }

        /// <summary>
        /// This method determines if there is room to print a word on the same line. 
        /// </summary>
        /// <param name="word">The word, which will need it's width calcuated.</param>
        /// <param name="availableLineWidth">The workable length to print the word.</param>
        /// <param name="trackLineCount">Tracks how many line returns have been calculated.</param>
        /// <returns>(double) available line width, (int) track line count, (bool) line return, (double) word width mm</returns>
        private static (double, int, bool, double) CalculateLineBreak(string word, double availableLineWidth, int trackLineCount) 
        {
            
            bool lineReturn = false;
            double whiteSpaceWidth = 0.79;
            double estWordWidth = CalculateWordWidthTimes9(word) + whiteSpaceWidth; // "word "
            if (estWordWidth > availableLineWidth) 
            {
                lineReturn = true;
                availableLineWidth = 0;
                Console.WriteLine($"New Line before: {word}");
                trackLineCount++;
            } else
            {
                availableLineWidth -= estWordWidth; // lineReturn is false
            }
            return (availableLineWidth, trackLineCount, lineReturn, estWordWidth);
        }

        /// <summary>
        /// <para>Calculate the word width using a static dictionary of char sizes.</para>
        /// <para>This method could be made to work with any font family/size by passing in the dict as a parameter.</para>
        /// </summary>
        /// <param name="word">The word to calculate</param>
        /// <returns>word width as a mm value</returns>
        private static double CalculateWordWidthTimes9(string word)
        {
            
            //double letterWidth;
            double wordMMLength = 0;
            for (int i = 0; i < word.Length; i++) 
            {
                if (CharWidthsTimesSize9.TryGetValue(word[i], out double letterWidth))
                {
                    wordMMLength += letterWidth;
                }
                else 
                {
                    Console.WriteLine($"{word[i]} NOT in CharWidthsTimesSize9");
                    wordMMLength += 1; // Assign a default value
                }
            }
            //Console.WriteLine($"{word}: {string.Format("{0:N2}", wordMMLength)} mm");
            return wordMMLength;
        }
    }
}
