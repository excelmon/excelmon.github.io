using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Text;

namespace AsposeLetters.Helpers
{
    internal class PdfTextUtils
    {
        // Measurement constants (in millimeters)
        private const double ANSWER_WHITESPACE_MM = 2.0;
        private const double RECTANGLE_Y_ADJUSTMENT_MM = 3.175;
        private const double LETTER_TITLE_Y_OFFSET_MM = 24.0;

        // Positioning adjustments (in points)
        // TextSegments need to be moved 1 point lower to align with other text fragments on the same y position.
        private const double TEXT_SEGMENT_Y_ADJUSTMENT = -1.0;

        // Page dimensions (in millimeters)
        public const double LETTER_HEIGHT_MM = 279.4; // Letter height. Y measures from the BOTTOM of the page
        public const double LETTER_HEADER_Y_OFFSET_MM = 107; // 107 mm from the TOP
        public const double ADDITIONAL_PAGE_START_Y_OFFSET_MM = 16.5; // all letter types can start here on page 2

        /// <summary>
        /// <para>outline: [TextFragment]</para>
        /// <para>example: [555 Main Street]</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndent">The X Position of the TextFragment</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="text">The value for the text</param>
        public static void SimpleTextFragmentTimes(Page page, double xIndent, double yIndent, string text) 
        {
            TextFragment textFragment = new TextFragment(text);
            PdfFontUtils.SetFontTimes(textFragment);
            textFragment.Position = new Position(xIndent, yIndent);
            page.Paragraphs.Add(textFragment);
        }

        /// <summary>
        /// <para>outline: [(red) TextFragment]</para>
        /// <para>example: [February 19, 2025]</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndent">The X Position of the TextFragment</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="text">The value for the text</param>
        public static void SimpleTextFragmentTimesRed(Page page, double xIndent, double yIndent, string text)
        {
            TextFragment textFragment = new TextFragment(text);
            PdfFontUtils.SetFontTimesRed(textFragment);
            textFragment.Position = new Position(xIndent, yIndent);
            page.Paragraphs.Add(textFragment);
        }

        /// <summary>
        /// <para>outline: [(underlined) TextFragment]</para>
        /// <para>example: [berger.phil@gmail.com]</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndent"></param>
        /// <param name="yIndent"></param>
        /// <param name="text"></param>
        public static void SimpleTextFragmentTimesUnderlined(Page page, double xIndent, double yIndent, string text)
        {
            TextFragment textFragment = new TextFragment(text);
            PdfFontUtils.SetFontTimes(textFragment);
            textFragment.TextState.Underline = true;
            textFragment.Position = new Position(xIndent, yIndent);
            page.Paragraphs.Add(textFragment);
        }

        /// <summary>
        /// <para>outline: [(red) TextFragment]</para>
        /// <para>example: [Account Statement] ←centered on page width</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="text"></param>
        public static void SimpleTextFragmentLetterTitle(Page page, string text) 
        {           
            var yMaxPosition = PdfMeasurementUtils.FromMMToPoints(LETTER_HEIGHT_MM); // Letter size height
            double yLetterTitle = yMaxPosition - PdfMeasurementUtils.FromMMToPoints(LETTER_TITLE_Y_OFFSET_MM);
            TextFragment textFragment = new TextFragment(text);
            PdfFontUtils.SetFontTimesRed(textFragment);
            double titleWidth = PdfMeasurementUtils.CalculateWidth(textFragment);
            double centerX = page.PageInfo.Width / 2;
            double adjustedX = centerX - titleWidth / 2;
            textFragment.Position = new Position(adjustedX, yLetterTitle);
            page.Paragraphs.Add(textFragment);
        }

        /// <summary>
        /// <para>outline: [TextFragment] [TextFragment]</para>
        /// <para>example: [3.] [List of outstanding debts]</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndentNumber">The X Position of the Number</param>
        /// <param name="xIndentQuestion">The X Position of the Question Text</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="number">Enter as string "3." for 3.</param>
        /// <param name="statement">The value for the text</param>
        public static void NumberedStatement(Page page, double xIndentNumber, double xIndentQuestion, double yIndent, 
            string number, string statement) 
        {
            TextFragment numberTextFragment = new TextFragment(number);
            PdfFontUtils.SetFontTimes(numberTextFragment);
            numberTextFragment.Position = new Position(xIndentNumber, yIndent);
            page.Paragraphs.Add(numberTextFragment);

            TextFragment statementTextFragment = new TextFragment(statement);
            PdfFontUtils.SetFontTimes(statementTextFragment);
            statementTextFragment.Position = new Position(xIndentQuestion, yIndent);
            page.Paragraphs.Add(statementTextFragment);
        }

        /// <summary>
        /// <para>outline: [TextFragment] ([TextSegment]+[TextSegment]→ [TextFragment])</para>
        /// <para>example: [1.] ([Date of last remitted payment:] [ ____ ])</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndentNumber">The X Position of the Number</param>
        /// <param name="xIndentQuestion">The X Position of the Question text</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="number">Enter as string "1." for 1.</param>
        /// <param name="question">Question text</param>
        /// <param name="answer">Answer as string value</param>
        public static void NumberedUnderlineAnswer(Page page, double xIndentNumber, double xIndentQuestion, double yIndent, 
            string number, string question, string answer) 
        {
            TextFragment numberTextFragment = new TextFragment(number.ToString());
            PdfFontUtils.SetFontTimes(numberTextFragment);
            numberTextFragment.Position = new Position(xIndentNumber, yIndent);
            page.Paragraphs.Add(numberTextFragment);

            // Use 2 text segments for Question: _Answer_
            TextSegment questionTextSegment = new TextSegment(question);
            PdfFontUtils.SetFontTextSegment(questionTextSegment);
            TextSegment answerTextSegment = new TextSegment(answer);
            PdfFontUtils.SetFontTextSegment(answerTextSegment);
            answerTextSegment.TextState.Underline = true; // underline the answer. 0.16 mm line thickness
            // Add TextSegments to TextFragment
            TextFragment questionPlusAnswerTextFragment = new TextFragment();
            questionPlusAnswerTextFragment.Segments.Add(questionTextSegment);
            questionPlusAnswerTextFragment.Segments.Add(answerTextSegment);
            questionPlusAnswerTextFragment.Position = new Position(xIndentQuestion, yIndent + TEXT_SEGMENT_Y_ADJUSTMENT);
            page.Paragraphs.Add(questionPlusAnswerTextFragment);
        }

        /// <summary>
        /// <para>outline: [TextFragment] [TextFragment] [TextFragment] ←[TextFragment (overlayed)]</para>
        /// <para>example: [2.] [Amount of last remitted payment:] [$ __________ ] ←[double]</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndentNumber">The X Position of the Number</param>
        /// <param name="xIndentQuestion">The X Position of the Question text</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="number">Enter as string "2." for 2.</param>
        /// <param name="question">Question text</param>
        /// <param name="answer">Answer as double value</param>
        public static void NumberedUnderlineDollarAnswer(Page page, double xIndentNumber, double xIndentQuestion, double yIndent,
            string number, string question, double answer) 
        {
            var answerWhiteSpace = PdfMeasurementUtils.FromMMToPoints(ANSWER_WHITESPACE_MM); // margin between question text and answer text
            TextFragment numberTextFragment = new TextFragment(number.ToString());
            PdfFontUtils.SetFontTimes(numberTextFragment);
            numberTextFragment.Position = new Position(xIndentNumber, yIndent);
            page.Paragraphs.Add(numberTextFragment);

            TextFragment questionTextFragment = new TextFragment(question);
            PdfFontUtils.SetFontTimes(questionTextFragment);
            questionTextFragment.Position = new Position(xIndentQuestion, yIndent);
            page.Paragraphs.Add(questionTextFragment);

            TextFragment dollarLineTextFragment = new TextFragment("$ __________"); // 0.13 mm line thickness
            PdfFontUtils.SetFontTimes(dollarLineTextFragment);
            double questionStringWidth = PdfMeasurementUtils.CalculateWidth(questionTextFragment);
            double dollarLineMargin = xIndentQuestion + questionStringWidth + answerWhiteSpace;
            dollarLineTextFragment.Position = new Position(dollarLineMargin, yIndent);
            page.Paragraphs.Add(dollarLineTextFragment);

            TextFragment answerTextFragment = new TextFragment(PdfFontUtils.FormatAmountAsString(answer));
            PdfFontUtils.SetFontTimes(answerTextFragment);
            // get length of answer so it can be placed on the right margin
            double answerStringWidth = PdfMeasurementUtils.CalculateWidth(answerTextFragment);
            // find where the right margin will be for $ __________
            double answerRightMargin = PdfMeasurementUtils.CalculateWidth(dollarLineTextFragment) + dollarLineMargin;
            answerTextFragment.Position = new Position(answerRightMargin - answerStringWidth, yIndent);
            page.Paragraphs.Add(answerTextFragment);
        }

        /// <summary>
        /// <para>outline: [TextFragment] [TextFragment] ←[TextFragment (overlayed)] [TextFragment]</para>
        /// <para>example: [a.] [$ __________ ] ←[double] [Tax Name]</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xIndentNumber">The X Position of the Number</param>
        /// <param name="xIndentDollarLine">The X Position of [$ __________ ]</param>
        /// <param name="xIndentLabel">The X Position of the Label</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="number">Enter as string "a." for a.</param>
        /// <param name="label">Label text</param>
        /// <param name="answer">Answer as double value</param>
        public static void NumberedUnderLineDollarAnswerRightLabel(Page page, double xIndentNumber, double xIndentDollarLine, double xIndentLabel, double yIndent,
            string number, string label, double answer) 
        {
            TextFragment numberTextFragment = new TextFragment(number.ToString());
            PdfFontUtils.SetFontTimes(numberTextFragment);
            numberTextFragment.Position = new Position(xIndentNumber, yIndent);
            page.Paragraphs.Add(numberTextFragment);

            TextFragment dollarLineTextFragment = new TextFragment("$ __________");
            PdfFontUtils.SetFontTimes(dollarLineTextFragment);
            dollarLineTextFragment.Position = new Position(xIndentDollarLine, yIndent);
            page.Paragraphs.Add(dollarLineTextFragment);
            double rightMarginDollarLine = xIndentDollarLine + PdfMeasurementUtils.CalculateWidth(dollarLineTextFragment);

            TextFragment answerTextFragment = new TextFragment(PdfFontUtils.FormatAmountAsString(answer));
            PdfFontUtils.SetFontTimes(answerTextFragment);
            double answerStringWidth = PdfMeasurementUtils.CalculateWidth(answerTextFragment);
            answerTextFragment.Position = new Position(rightMarginDollarLine - answerStringWidth, yIndent);
            page.Paragraphs.Add(answerTextFragment);

            TextFragment labelTextFragment = new TextFragment(label);
            PdfFontUtils.SetFontTimes(labelTextFragment);
            labelTextFragment.Position = new Position(xIndentLabel, yIndent);
            page.Paragraphs.Add(labelTextFragment);
        }

        /// <summary>
        /// <para>outline: [TextFragment]→ TextParagraph.Rectangle→ TextBuilder.AppendParagraph</para>
        /// <para>example: ["In reference to the above-mentioned account, we hereby acknowledge receipt...(continue paragraph)"]</para>
        /// <para>note: This method works with Times New Roman size 9 text.</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="xIndent">The X Position of the text</param>
        /// <param name="xRightMargin">The right margin for the Question text</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="text">The value for the text</param>
        /// <param name="measure">Do you want to draw the rectangle measurements for testing?</param>
        /// <returns>The calculated height of the Rectangle</returns>
        public static double TextRectangle(Page page, Graph graph, double xIndent, double xRightMargin, double yIndent, string text, bool measure) 
        {
            TextFragment textFragment = new TextFragment(text);
            PdfFontUtils.SetFontTimes(textFragment);
            textFragment.Position.XIndent = xIndent;

            double totalLineWidth = PdfMeasurementUtils.FromPointsToMm(xRightMargin) - PdfMeasurementUtils.FromPointsToMm(xIndent);
            //Console.WriteLine($"totalLineWidth: {totalLineWidth}");
            // Use utility to calulate the estimated height
            double finalHeight = PdfMeasurementUtils.CalculateTextRectangleHeight(text, totalLineWidth);

            // Use a TextParagraph to control indents
            TextParagraph textParagraph = new TextParagraph();
            // Set the paragraph rectangle
            textParagraph.Rectangle = new Aspose.Pdf.Rectangle(xIndent, yIndent - finalHeight, xRightMargin, yIndent);
            // Set the word wrapping options
            textParagraph.FormattingOptions.WrapMode = TextFormattingOptions.WordWrapMode.ByWords;
            textParagraph.VerticalAlignment = VerticalAlignment.Top;
            textParagraph.AppendLine(textFragment);
            // append the textParagraph.Rectangle to the Pdf page with the TextBuilder
            TextBuilder textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(textParagraph);
            if (measure)
            {
                List<TextFragment> textFragments = new List<TextFragment>();
                textFragments.AddRange(PdfDrawingUtils.DrawMeasureLineRectangle(page, graph, textParagraph.Rectangle));
                foreach (TextFragment tf in textFragments)
                {
                    page.Paragraphs.Add(tf);
                }
            }
            double rectHeight = textParagraph.Rectangle.URY - textParagraph.Rectangle.LLY;

            return rectHeight;
        }

        /// <summary>
        /// <para>outline: [TextFragment] ([TextSegment]+[TextSegment]→ [TextFragment])→ TextParagraph.Rectangle→ TextBuilder.AppendParagraph</para>
        /// <para>example: [4.] ["Is this account over 90-day delinquent or has this account been...(continue paragraph)"] [No]</para>
        /// <para>note: This method works with Times New Roman size 9 text.</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="xIndentNumber">The X Position of the Number</param>
        /// <param name="xIndentQuestion">The X Position of the Question text</param>
        /// <param name="xRightMargin">The right margin for the Question text</param>
        /// <param name="yIndent">The Y Position of the TextFragment</param>
        /// <param name="number">Enter as string "4." for 11.</param>
        /// <param name="question">Question text</param>
        /// <param name="answer">Answer as string value</param>
        /// <param name="measure">Do you want to draw the rectangle measurements for testing?</param>
        /// <returns>The calculated height of the Rectangle</returns>
        public static double NumberedTextRectangleUnderlineAnswer(Page page, Graph graph, double xIndentNumber, double xIndentQuestion, double xRightMargin, double yIndent, 
            string number, string question, string answer, bool measure) 
        {
            TextFragment numberTextFragment = new TextFragment(number);
            PdfFontUtils.SetFontTimes(numberTextFragment);
            numberTextFragment.Position = new Position(xIndentNumber, yIndent);
            page.Paragraphs.Add(numberTextFragment);

            TextSegment questionTextSegment = new TextSegment(question);
            PdfFontUtils.SetFontTextSegment(questionTextSegment);

            TextSegment answerTextSegment = new TextSegment(answer);
            PdfFontUtils.SetFontTextSegment(answerTextSegment);
            answerTextSegment.TextState.Underline = true; // Underline answer

            TextFragment rectTextFragment = new TextFragment();
            // Add TextSegments to TextFragments (with font already formated!)
            rectTextFragment.Segments.Add(questionTextSegment);
            rectTextFragment.Segments.Add(answerTextSegment);

            TextParagraph textParagraph = new TextParagraph();

            double totalLineWidth = PdfMeasurementUtils.FromPointsToMm(xRightMargin) - PdfMeasurementUtils.FromPointsToMm(xIndentQuestion);
            double finalHeight = PdfMeasurementUtils.CalculateTextRectangleHeight(rectTextFragment.Text, totalLineWidth); 
            var yAdjustmentForRectangle = PdfMeasurementUtils.FromMMToPoints(RECTANGLE_Y_ADJUSTMENT_MM); // bring rectangle up to the height of the numberTextFragment

            textParagraph.Rectangle = new Aspose.Pdf.Rectangle(xIndentQuestion, 
                yIndent - finalHeight + yAdjustmentForRectangle, 
                xRightMargin, 
                yIndent + yAdjustmentForRectangle);

            textParagraph.FormattingOptions.WrapMode = TextFormattingOptions.WordWrapMode.ByWords;
            textParagraph.VerticalAlignment = VerticalAlignment.Top;
            textParagraph.AppendLine(rectTextFragment);

            TextBuilder textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(textParagraph);
            if (measure)
            {
                List<TextFragment> textFragments = new List<TextFragment>();
                textFragments.AddRange(PdfDrawingUtils.DrawMeasureLineRectangle(page, graph, textParagraph.Rectangle));
                foreach (TextFragment tf in textFragments)
                {
                    page.Paragraphs.Add(tf);
                }
            }
            double rectHeight = textParagraph.Rectangle.URY - textParagraph.Rectangle.LLY;
            return rectHeight;
        }
    }
}
