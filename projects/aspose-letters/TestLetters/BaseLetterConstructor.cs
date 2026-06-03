using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using AsposeLetters.FieldSchema;
using AsposeLetters.Helpers;
using AsposeLetters.Sections;

namespace AsposeLetters.TestLetters
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
        private const string CLOSING_STATEMENT = "This is where you would add some standardized verbiage to use as a closing statement. You could " +
            "include disclaimers, specific instructions for contacting your company or any other legalese jargon in this section. The idea is that " +
            "you can reuse this statement on all your letter templates and having it as a constant all letter templates to pull the same text.";
        private const string SIGNATURE_STATEMENT = "Here's where you would add your sworn statement verbiage that goes above the signature section. " +
            "In the world of garnishments, some states do require specific statements, but for the most part you can use a general statement.";

        // === Shared Fields ===
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

        protected void AddLetterhead(Document pdfDocument, Page page)
        {
            new LetterheadSection(pdfDocument, page).AddToDocument();
            new RecipientAddressSection(pdfDocument, page, Fields.MailingRecipientAddress).AddToDocument();
            new EmployeeAddressSection(pdfDocument, page, Fields.EmployeeAddress).AddToDocument();
            new CustomerAddressSection(pdfDocument, page, Fields.CustomerAddress).AddToDocument();
        }

        protected void AddClosingStatements(Document pdfDocument, Page page, Graph graph, bool addMeasurements, double yTracking)
        {
            double garnisheeStatementHeight = PdfTextUtils.TextRectangle(page, graph, _marginLetterBody, _rightMarginLetterBody, yTracking, CLOSING_STATEMENT, addMeasurements);
            yTracking -= garnisheeStatementHeight + _yAdjustmentForRectangle;

            double signatureStatementHeight = PdfTextUtils.TextRectangle(page, graph, _marginLetterBody, _rightMarginLetterBody, yTracking, SIGNATURE_STATEMENT, addMeasurements);
            yTracking -= signatureStatementHeight + PdfMeasurementUtils.FromMMToPoints(SIGNATURE_LINE_Y_OFFSET_MM);

            new SignatureSection(pdfDocument, page, yTracking).AddToDocument();
        }

        protected void AddFooter(Document pdfDocument)
        {
            new FooterSection(pdfDocument).AddToDocument(
                Fields.CustomerBillingId.Value!,
                Fields.LetterType.Value!,
                Fields.LTRName.Value!);
        }

        protected Page CreatePage(Document pdfDocument)
        {
            Page page = pdfDocument.Pages.Add();
            page.SetPageSize(PageSize.PageLetter.Width, PageSize.PageLetter.Height);
            page.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            return page;
        }

        protected Graph CreateGraph(Page page)
        {
            Graph graph = new Graph(page.PageInfo.Width, page.PageInfo.Height);
            page.Paragraphs.Add(graph);
            return graph;
        }

        protected Document CreateDocument(string title, string keywords)
        {
            Document pdfDocument = new Document();
            pdfDocument.Info.Keywords = keywords;
            pdfDocument.Info.Title = title;
            return pdfDocument;
        }

        protected double AddRELine(Page page, double yTracking)
        {
            PdfTextUtils.SimpleTextFragmentTimes(page, _marginLetterBody, yTracking, $"For Employee: {Fields.EmployeeFullName}");
            yTracking -= _ySpacingAddress;
            PdfTextUtils.SimpleTextFragmentTimes(page, _marginLetterBody, yTracking,
                $"Garnishment Case Number: {Fields.GarnishmentCaseNumber.Value};    Processed: {Fields.LetterProcessedDate.Value?.ToString("MMMM d, yyyy") ?? "N/A"}");
            return yTracking;
        }

        protected double AddGreeting(Page page, double yTracking)
        {
            yTracking -= PdfMeasurementUtils.FromMMToPoints(GREETING_LINE_Y_OFFSET_MM);
            PdfTextUtils.SimpleTextFragmentTimes(page, _marginLetterBody, yTracking, $"Dear {Fields.MailingRecipientAddress.AddressName}:");
            yTracking -= _ySpacingAddress * ADDRESS_SPACING_MULTIPLIER;
            return yTracking;
        }

        protected double AddIntroduction(Page page, Graph graph, double yTracking, bool addMeasurements)
        {
            string letterBodyText = $"This is where you would add your standard introduction paragraph. It's intended to be a general statement that can " +
                "be reused for this letter type (or various letter templates). You could insert various types of information into this paragraph, such as " +
                $"a name - {Fields.EmployeeFullName} or a date - {Fields.LetterProcessedDate.Value?.ToString("MMMM d, yyyy")}.";
            double letterBodyHeight = PdfTextUtils.TextRectangle(page, graph, _marginLetterBody, _rightMarginLetterBody, yTracking, letterBodyText, addMeasurements);
            return yTracking - _ySpacing - letterBodyHeight;
        }

        protected double GetInitialYPositionForRELine()
        {
            var pageTopPosition = PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.LETTER_HEIGHT_MM);
            return pageTopPosition - PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.LETTER_HEADER_Y_OFFSET_MM);
        }

        protected double GetInitialYPositionForAdditionalPage()
        {
            var pageTopPosition = PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.LETTER_HEIGHT_MM);
            return pageTopPosition - PdfMeasurementUtils.FromMMToPoints(PdfTextUtils.ADDITIONAL_PAGE_START_Y_OFFSET_MM);
        }

        // === Abstract Methods (must be implemented by child classes) ===

        protected abstract double AddLetterQuestions(Page page, Graph graph, double yTracking, bool addMeasurements);

        public abstract void CreateTestLetter(bool addMeasurements, string outputPath);
    }
}
