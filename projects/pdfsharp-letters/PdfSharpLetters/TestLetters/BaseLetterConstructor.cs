using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.FieldSchema;
using PdfSharpLetters.Helpers;
using PdfSharpLetters.Sections;

namespace PdfSharpLetters.TestLetters
{
    internal abstract class BaseLetterConstructor
    {
        // === Shared Measurement Constants (MM) ===
        protected const double MARGIN_LETTER_BODY_MM = 13;
        protected const double RIGHT_MARGIN_LETTER_BODY_MM = 196;
        protected const double MARGIN_QUESTION_NUMBER_MM = 20;
        protected const double MARGIN_QUESTION_TEXT_MM = 27;
        protected const double RIGHT_MARGIN_QUESTION_TEXT_MM = 194;
        protected const double MARGIN_SUB_NUMBER_MM = 34;
        protected const double MARGIN_SUB_NUMBER_DOLLAR_LINE_MM = 41;
        protected const double MARGIN_SUB_NUMBER_LABEL = 65;
        protected const double Y_LINE_SPACING_MM = 7;
        protected const double Y_ADJUSTMENT_FOR_RECT_MM = 3.175;
        protected const double Y_LINE_SPACING_ADDRESS_MM = 3.75;
        protected const double Y_LINE_SPACING_SUB_ANSWER_MM = 5.25;
        protected const double GREETING_LINE_Y_OFFSET_MM = 10;
        protected const double SIGNATURE_LINE_Y_OFFSET_MM = 14;
        protected const double ADDRESS_SPACING_MULTIPLIER = 2.0;

        // === Closing Verbiage ===
        private const string CLOSING_STATEMENT =
            "This is where you would add some standardized verbiage to use as a closing statement. You could " +
            "include disclaimers, specific instructions for contacting your company or any other legalese jargon in this section. The idea is that " +
            "you can reuse this statement on all your letter templates and having it as a constant all letter templates to pull the same text.";
        private const string SIGNATURE_STATEMENT =
            "Here's where you would add your sworn statement verbiage that goes above the signature section. " +
            "In the world of garnishments, some states do require specific statements, but for the most part you can use a general statement.";

        // === Shared Fields (pre-converted to points) ===
        protected readonly double _marginLetterBody;
        protected readonly double _rightMarginLetterBody;
        protected readonly double _marginQuestionNumber;
        protected readonly double _marginQuestionText;
        protected readonly double _rightMarginQuestionText;
        protected readonly double _marginSubNumber;
        protected readonly double _marginSubNumberDollarLine;
        protected readonly double _marginSubNumberLabel;
        protected readonly double _ySpacing;
        protected readonly double _yAdjustmentForRectangle;
        protected readonly double _ySpacingAddress;
        protected readonly double _ySpacingSubAnswer;

        protected BaseLetterConstructor()
        {
            _marginLetterBody = PdfMeasurementUtils.FromMMToPoints(MARGIN_LETTER_BODY_MM);
            _rightMarginLetterBody = PdfMeasurementUtils.FromMMToPoints(RIGHT_MARGIN_LETTER_BODY_MM);
            _marginQuestionNumber = PdfMeasurementUtils.FromMMToPoints(MARGIN_QUESTION_NUMBER_MM);
            _marginQuestionText = PdfMeasurementUtils.FromMMToPoints(MARGIN_QUESTION_TEXT_MM);
            _rightMarginQuestionText = PdfMeasurementUtils.FromMMToPoints(RIGHT_MARGIN_QUESTION_TEXT_MM);
            _marginSubNumber = PdfMeasurementUtils.FromMMToPoints(MARGIN_SUB_NUMBER_MM);
            _marginSubNumberDollarLine = PdfMeasurementUtils.FromMMToPoints(MARGIN_SUB_NUMBER_DOLLAR_LINE_MM);
            _marginSubNumberLabel = PdfMeasurementUtils.FromMMToPoints(MARGIN_SUB_NUMBER_LABEL);
            _ySpacing = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_MM);
            _yAdjustmentForRectangle = PdfMeasurementUtils.FromMMToPoints(Y_ADJUSTMENT_FOR_RECT_MM);
            _ySpacingAddress = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_ADDRESS_MM);
            _ySpacingSubAnswer = PdfMeasurementUtils.FromMMToPoints(Y_LINE_SPACING_SUB_ANSWER_MM);
        }

        // === Common Methods ===

        protected void AddLetterhead(PdfDocument pdfDocument, PdfPage page, XGraphics gfx)
        {
            new LetterheadSection(pdfDocument, page, gfx).AddToDocument();
            new RecipientAddressSection(pdfDocument, page, gfx, Fields.MailingRecipientAddress).AddToDocument();
            new EmployeeAddressSection(pdfDocument, page, gfx, Fields.EmployeeAddress).AddToDocument();
            new CustomerAddressSection(pdfDocument, page, gfx, Fields.CustomerAddress).AddToDocument();
        }

        protected void AddClosingStatements(PdfDocument pdfDocument, PdfPage page, XGraphics gfx,
            bool addMeasurements, double yTracking)
        {
            double closingHeight = PdfTextUtils.TextRectangle(gfx,
                _marginLetterBody, _rightMarginLetterBody, yTracking, CLOSING_STATEMENT, addMeasurements);
            yTracking += closingHeight + _yAdjustmentForRectangle;

            double signatureHeight = PdfTextUtils.TextRectangle(gfx,
                _marginLetterBody, _rightMarginLetterBody, yTracking, SIGNATURE_STATEMENT, addMeasurements);
            yTracking += signatureHeight + PdfMeasurementUtils.FromMMToPoints(SIGNATURE_LINE_Y_OFFSET_MM);

            new SignatureSection(pdfDocument, page, gfx, yTracking).AddToDocument();
        }

        protected void AddFooter(PdfDocument pdfDocument)
        {
            new FooterSection(pdfDocument).AddToDocument(
                Fields.CustomerBillingId.Value!,
                Fields.LetterType.Value!,
                Fields.LTRName.Value!);
        }

        /// <summary>
        /// Adds a new page sized to US Letter (215.9 x 279.4 mm).
        /// </summary>
        protected PdfPage CreatePage(PdfDocument pdfDocument)
        {
            PdfPage page = pdfDocument.AddPage();
            page.Width = XUnit.FromMillimeter(PdfTextUtils.LETTER_WIDTH_MM);
            page.Height = XUnit.FromMillimeter(PdfTextUtils.LETTER_HEIGHT_MM);
            return page;
        }

        /// <summary>
        /// Creates an XGraphics drawing context for the given page.
        /// Callers are responsible for disposing it (use 'using' or call Dispose()).
        /// </summary>
        protected XGraphics CreateGraphics(PdfPage page)
        {
            return XGraphics.FromPdfPage(page);
        }

        protected PdfDocument CreateDocument(string title, string keywords)
        {
            var doc = new PdfDocument();
            doc.Info.Title = title;
            doc.Info.Keywords = keywords;
            return doc;
        }

        /// <summary>
        /// Returns the initial Y position (from top) for the RE line on page 1,
        /// matching the original 107 mm header offset.
        /// </summary>
        protected double GetInitialYPositionForRELine()
        {
            return PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.LETTER_HEADER_Y_OFFSET_MM);
        }

        /// <summary>
        /// Returns the initial Y position for page 2+ content.
        /// </summary>
        protected double GetInitialYPositionForAdditionalPage()
        {
            return PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.ADDITIONAL_PAGE_START_Y_OFFSET_MM);
        }

        protected double AddRELine(XGraphics gfx, double yTracking)
        {
            PdfTextUtils.SimpleTextFragmentTimes(gfx, _marginLetterBody, yTracking,
                $"For Employee: {Fields.EmployeeFullName}");
            yTracking += _ySpacingAddress;
            PdfTextUtils.SimpleTextFragmentTimes(gfx, _marginLetterBody, yTracking,
                $"Garnishment Case Number: {Fields.GarnishmentCaseNumber.Value};    " +
                $"Processed: {Fields.LetterProcessedDate.Value?.ToString("MMMM d, yyyy") ?? "N/A"}");
            return yTracking;
        }

        protected double AddGreeting(XGraphics gfx, double yTracking)
        {
            yTracking += PdfMeasurementUtils.FromMMToPoints(GREETING_LINE_Y_OFFSET_MM);
            PdfTextUtils.SimpleTextFragmentTimes(gfx, _marginLetterBody, yTracking,
                $"Dear {Fields.MailingRecipientAddress.AddressName}:");
            yTracking += _ySpacingAddress * ADDRESS_SPACING_MULTIPLIER;
            return yTracking;
        }

        protected double AddIntroduction(XGraphics gfx, double yTracking, bool addMeasurements)
        {
            string introText =
                $"This is where you would add your standard introduction paragraph. It's intended to be a general statement that can " +
                "be reused for this letter type (or various letter templates). You could insert various types of information into this paragraph, such as " +
                $"a name - {Fields.EmployeeFullName} or a date - {Fields.LetterProcessedDate.Value?.ToString("MMMM d, yyyy")}.";
            double introHeight = PdfTextUtils.TextRectangle(gfx,
                _marginLetterBody, _rightMarginLetterBody, yTracking, introText, addMeasurements);
            return yTracking + _ySpacing + introHeight;
        }

        // === Abstract Methods ===

        protected abstract double AddLetterQuestions(XGraphics gfx, double yTracking, bool addMeasurements);

        public abstract void CreateTestLetter(bool addMeasurements, string outputPath);
    }
}
