using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfSharpLetters.Helpers
{
    /// <summary>
    /// Text drawing utilities for the letter project.
    ///
    /// COORDINATE SYSTEM NOTE:
    /// PdfSharp uses a top-left origin where Y increases downward (screen coordinates),
    /// unlike Aspose which used bottom-left with Y increasing upward.
    /// All yTracking values in this project flow top-to-bottom: yTracking increases as
    /// we move down the page. Page height is added to position from top of page.
    /// </summary>
    internal class PdfTextUtils
    {
        // ── Measurement constants (mm) ─────────────────────────────────────────

        private const double ANSWER_WHITESPACE_MM = 2.0;
        private const double LETTER_TITLE_Y_OFFSET_MM = 24.0;

        // Page dimensions (mm)
        public const double LETTER_HEIGHT_MM = 279.4;
        public const double LETTER_WIDTH_MM = 215.9;

        // Y offsets from TOP of page (PdfSharp top-down orientation)
        public const double LETTER_HEADER_Y_OFFSET_MM = 107.0;       // 107 mm from the top
        public const double ADDITIONAL_PAGE_START_Y_OFFSET_MM = 16.5; // page 2+ content starts here

        // ── Simple text fragments ──────────────────────────────────────────────

        /// <summary>
        /// Draws a single line of Times New Roman 9pt text at the given position.
        /// </summary>
        public static void SimpleTextFragmentTimes(XGraphics gfx, double xIndent, double yIndent, string text)
        {
            var font = PdfFontUtils.FontTimes();
            gfx.DrawString(text, font, new XSolidBrush(PdfFontUtils.ColorBlack),
                new XPoint(xIndent, yIndent));
        }

        /// <summary>
        /// Draws a single line of Times New Roman 9pt text in red.
        /// </summary>
        public static void SimpleTextFragmentTimesRed(XGraphics gfx, double xIndent, double yIndent, string text)
        {
            var font = PdfFontUtils.FontTimes();
            gfx.DrawString(text, font, new XSolidBrush(PdfFontUtils.ColorRed),
                new XPoint(xIndent, yIndent));
        }

        /// <summary>
        /// Draws a single line of Times New Roman 9pt text with an underline drawn as a line
        /// beneath the text (PdfSharp does not have a built-in underline flag for DrawString).
        /// </summary>
        public static void SimpleTextFragmentTimesUnderlined(XGraphics gfx, double xIndent, double yIndent, string text)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);
            gfx.DrawString(text, font, brush, new XPoint(xIndent, yIndent));
            DrawUnderline(gfx, font, text, xIndent, yIndent);
        }

        /// <summary>
        /// Draws a centered red letter title at the standard title Y position.
        /// </summary>
        public static void SimpleTextFragmentLetterTitle(XGraphics gfx, double pageWidth, string text)
        {
            var font = PdfFontUtils.FontTimesBoldTitle();
            double textWidth = gfx.MeasureString(text, font).Width;
            double xIndent = PdfMeasurementUtils.CalculateCenteredXPosition(pageWidth, textWidth);
            double yIndent = PdfMeasurementUtils.FromMMToPoints(LETTER_TITLE_Y_OFFSET_MM);
            gfx.DrawString(text, font, new XSolidBrush(PdfFontUtils.ColorRed),
                new XPoint(xIndent, yIndent));
        }

        // ── Numbered question patterns ─────────────────────────────────────────

        /// <summary>
        /// Draws: [number]  [statement text]
        /// Example: [3.]  [List of outstanding debts]
        /// </summary>
        public static void NumberedStatement(XGraphics gfx, double xIndentNumber, double xIndentQuestion,
            double yIndent, string number, string statement)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);
            gfx.DrawString(number, font, brush, new XPoint(xIndentNumber, yIndent));
            gfx.DrawString(statement, font, brush, new XPoint(xIndentQuestion, yIndent));
        }

        /// <summary>
        /// Draws: [number]  [question text][underlined answer]
        /// Example: [1.]  [Date of last remitted payment:  ][ June 13, 2026]
        /// The answer segment is underlined by drawing a line beneath it.
        /// </summary>
        public static void NumberedUnderlineAnswer(XGraphics gfx, double xIndentNumber, double xIndentQuestion,
            double yIndent, string number, string question, string answer)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);

            gfx.DrawString(number, font, brush, new XPoint(xIndentNumber, yIndent));
            gfx.DrawString(question, font, brush, new XPoint(xIndentQuestion, yIndent));

            double questionWidth = gfx.MeasureString(question, font).Width;
            double answerX = xIndentQuestion + questionWidth;
            gfx.DrawString(answer, font, brush, new XPoint(answerX, yIndent));
            DrawUnderline(gfx, font, answer, answerX, yIndent);
        }

        /// <summary>
        /// Draws: [number]  [question text]  [$ __________] overlaid with [right-aligned dollar amount]
        /// Example: [2.]  [Amount of last remitted payment:]  [$ __________] ← [15,000.00]
        /// </summary>
        public static void NumberedUnderlineDollarAnswer(XGraphics gfx, double xIndentNumber, double xIndentQuestion,
            double yIndent, string number, string question, double answer)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);
            double answerWhiteSpace = PdfMeasurementUtils.FromMMToPoints(ANSWER_WHITESPACE_MM);

            gfx.DrawString(number, font, brush, new XPoint(xIndentNumber, yIndent));
            gfx.DrawString(question, font, brush, new XPoint(xIndentQuestion, yIndent));

            double questionWidth = gfx.MeasureString(question, font).Width;
            double dollarLineX = xIndentQuestion + questionWidth + answerWhiteSpace;
            const string dollarLine = "$ __________";
            gfx.DrawString(dollarLine, font, brush, new XPoint(dollarLineX, yIndent));

            string answerText = PdfFontUtils.FormatAmountAsString(answer);
            double dollarLineWidth = gfx.MeasureString(dollarLine, font).Width;
            double answerWidth = gfx.MeasureString(answerText, font).Width;
            double answerX = dollarLineX + dollarLineWidth - answerWidth;
            gfx.DrawString(answerText, font, brush, new XPoint(answerX, yIndent));
        }

        /// <summary>
        /// Draws: [subletter]  [$ __________] overlaid with [right-aligned dollar amount]  [label]
        /// Example: [a.]  [$ __________] ← [10,000.99]  [Federal Income Tax]
        /// </summary>
        public static void NumberedUnderLineDollarAnswerRightLabel(XGraphics gfx,
            double xIndentNumber, double xIndentDollarLine, double xIndentLabel,
            double yIndent, string number, string label, double answer)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);

            gfx.DrawString(number, font, brush, new XPoint(xIndentNumber, yIndent));

            const string dollarLine = "$ __________";
            gfx.DrawString(dollarLine, font, brush, new XPoint(xIndentDollarLine, yIndent));

            double dollarLineWidth = gfx.MeasureString(dollarLine, font).Width;
            string answerText = PdfFontUtils.FormatAmountAsString(answer);
            double answerWidth = gfx.MeasureString(answerText, font).Width;
            double answerX = xIndentDollarLine + dollarLineWidth - answerWidth;
            gfx.DrawString(answerText, font, brush, new XPoint(answerX, yIndent));

            gfx.DrawString(label, font, brush, new XPoint(xIndentLabel, yIndent));
        }

        // ── Paragraph / word-wrapped text rectangle ────────────────────────────

        /// <summary>
        /// Draws a word-wrapped paragraph within a bounding rectangle defined by
        /// xIndent (left), xRightMargin (right), and yIndent (top of the text block).
        ///
        /// Returns the height of the rendered text block in points so the caller
        /// can advance yTracking correctly.
        ///
        /// IMPORTANT: blockHeight is derived from the same MeasureString word-wrap pass
        /// that draws the text, so the bounding box (measure=true) always matches the
        /// actual rendered lines. Do not use CalculateTextRectangleHeight here — it uses
        /// the static char-width dict which can disagree with live font metrics.
        /// </summary>
        public static double TextRectangle(XGraphics gfx, double xIndent, double xRightMargin,
            double yIndent, string text, bool measure)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);
            double lineHeightPts = PdfMeasurementUtils.FromMMToPoints(PdfFontUtils.TIMES_9_LINE_HEIGHT_MM);

            // Single pass: word-wrap with MeasureString, draw each line, accumulate height.
            int lineCount = DrawWrappedText(gfx, font, brush, text, xIndent, yIndent, xRightMargin, lineHeightPts);
            double blockHeight = lineCount * lineHeightPts;

            if (measure)
            {
                // The bounding box top sits one ascent above the first baseline.
                // CellAscent and CellSpace are in design units; scale to points via GetHeight().
                double ascent = font.GetHeight() * font.CellAscent / font.CellSpace;
                var pen = new XPen(XColors.Blue, 0.5);
                gfx.DrawRectangle(pen, xIndent, yIndent - ascent,
                    xRightMargin - xIndent, blockHeight + ascent);
            }

            return blockHeight;
        }

        /// <summary>
        /// Draws a number label, then a word-wrapped question+underlined-answer block.
        /// Returns the block height in points.
        /// </summary>
        public static double NumberedTextRectangleUnderlineAnswer(XGraphics gfx,
            double xIndentNumber, double xIndentQuestion, double xRightMargin,
            double yIndent, string number, string question, string answer, bool measure)
        {
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);

            gfx.DrawString(number, font, brush, new XPoint(xIndentNumber, yIndent));

            double lineHeightPts = PdfMeasurementUtils.FromMMToPoints(PdfFontUtils.TIMES_9_LINE_HEIGHT_MM);

            // Single pass via DrawWrappedTextWithUnderlinedSuffix which returns the line count.
            int lineCount = DrawWrappedTextWithUnderlinedSuffix(gfx, font, brush, question, answer,
                xIndentQuestion, yIndent, xRightMargin, lineHeightPts);
            double blockHeight = lineCount * lineHeightPts;

            if (measure)
            {
                // CellAscent and CellSpace are in design units; scale to points via GetHeight().
                double ascent = font.GetHeight(gfx) * font.CellAscent / font.CellSpace;
                var pen = new XPen(XColors.Blue, 0.5);
                gfx.DrawRectangle(pen, xIndentQuestion, yIndent - ascent,
                    xRightMargin - xIndentQuestion, blockHeight + ascent);
            }

            return blockHeight;
        }

        // ── Internal drawing helpers ───────────────────────────────────────────

        /// <summary>
        /// Draws an underline beneath a string by measuring text width and drawing a horizontal line.
        /// Underline is placed 1.5pt below the text baseline (standard convention).
        /// </summary>
        internal static void DrawUnderline(XGraphics gfx, XFont font, string text, double x, double y)
        {
            double textWidth = gfx.MeasureString(text, font).Width;
            double underlineY = y + 1.5; // 1.5 points below baseline
            var pen = new XPen(PdfFontUtils.ColorBlack, 0.5);
            gfx.DrawLine(pen, x, underlineY, x + textWidth, underlineY);
        }

        /// <summary>
        /// Word-wraps text within [xLeft, xRight] starting at yTop, advancing by lineHeight per line.
        /// Returns the number of lines drawn, so callers can compute block height from a single pass.
        /// </summary>
        private static int DrawWrappedText(XGraphics gfx, XFont font, XBrush brush,
            string text, double xLeft, double yTop, double xRight, double lineHeight)
        {
            double maxWidth = xRight - xLeft;
            string[] words = text.Split(' ');
            string currentLine = "";
            double currentY = yTop;
            int lineCount = 0;

            foreach (string word in words)
            {
                string testLine = currentLine.Length == 0 ? word : currentLine + " " + word;
                double testWidth = gfx.MeasureString(testLine, font).Width;

                if (testWidth > maxWidth && currentLine.Length > 0)
                {
                    gfx.DrawString(currentLine, font, brush, new XPoint(xLeft, currentY));
                    currentY += lineHeight;
                    lineCount++;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            if (currentLine.Length > 0)
            {
                gfx.DrawString(currentLine, font, brush, new XPoint(xLeft, currentY));
                lineCount++;
            }

            return lineCount;
        }

        /// <summary>
        /// Like DrawWrappedText, but the answer suffix on the last line is underlined.
        /// Treats question+answer as a single flowing paragraph: wraps all words together,
        /// then underlines only the answer portion wherever it ends up.
        /// Returns the total line count so callers can compute block height.
        /// </summary>
        private static int DrawWrappedTextWithUnderlinedSuffix(XGraphics gfx, XFont font, XBrush brush,
            string question, string answer, double xLeft, double yTop, double xRight, double lineHeight)
        {
            // Wrap the full text as one paragraph so question and answer flow together naturally.
            string fullText = question + answer;
            double maxWidth = xRight - xLeft;
            string[] words = fullText.Split(' ');
            string currentLine = "";
            double currentY = yTop;
            int lineCount = 0;

            // Track how much of the question has been laid out so we know when we enter answer territory.
            double questionWidth = gfx.MeasureString(question, font).Width;
            double consumedWidth = 0;

            foreach (string word in words)
            {
                string testLine = currentLine.Length == 0 ? word : currentLine + " " + word;
                double testWidth = gfx.MeasureString(testLine, font).Width;

                if (testWidth > maxWidth && currentLine.Length > 0)
                {
                    // Draw the completed line. Determine if it is entirely in the answer region.
                    double lineWidth = gfx.MeasureString(currentLine, font).Width;
                    bool lineIsAnswer = consumedWidth >= questionWidth;
                    gfx.DrawString(currentLine, font, brush, new XPoint(xLeft, currentY));
                    if (lineIsAnswer)
                        DrawUnderline(gfx, font, currentLine, xLeft, currentY);
                    consumedWidth += lineWidth;
                    currentY += lineHeight;
                    lineCount++;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            // Draw the final (possibly partial) line.
            if (currentLine.Length > 0)
            {
                bool lineIsAnswer = consumedWidth >= questionWidth;
                gfx.DrawString(currentLine, font, brush, new XPoint(xLeft, currentY));
                if (lineIsAnswer)
                    DrawUnderline(gfx, font, currentLine, xLeft, currentY);
                lineCount++;
            }

            return lineCount;
        }
    }
}
