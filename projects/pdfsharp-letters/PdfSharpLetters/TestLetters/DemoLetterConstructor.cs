using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.FieldSchema;
using PdfSharpLetters.Helpers;

namespace PdfSharpLetters.TestLetters
{
    class DemoLetterConstructor : BaseLetterConstructor
    {
        public DemoLetterConstructor() : base() { }

        protected override double AddLetterQuestions(XGraphics gfx, double yTracking, bool addMeasurements)
        {
            // 1. Question:  _______
            string number1QuestionText = "This is a demo of 'NumberedUnderlineAnswer' with a date answer:  ";
            string number1AnswerText = Fields.LetterProcessedDate.Value?.ToString("MMMM d, yyyy")
                ?? DateOnly.FromDateTime(DateTime.Now).ToString("MMMM d, yyyy");
            PdfTextUtils.NumberedUnderlineAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "1.", number1QuestionText, number1AnswerText);
            yTracking += _ySpacing;

            // 2. Question:  _______
            string number2QuestionText = "This is a demo of 'NumberedUnderlineAnswer' with a string answer:  ";
            PdfTextUtils.NumberedUnderlineAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "2.", number2QuestionText, "just a string");
            yTracking += _ySpacing;

            // 3. Question:  _______
            string number3QuestionText = "This is a demo of 'NumberedUnderlineAnswer' with a double converted to a string:  ";
            PdfTextUtils.NumberedUnderlineAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "3.", number3QuestionText, Fields.ScheduledWeeklyHours.Value.ToString());
            yTracking += _ySpacing;

            // 4. Statement — no underline
            string number4insertValue = "NumberedStatement";
            PdfTextUtils.NumberedStatement(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "4.", $"This is a demo of '{number4insertValue}', which you can use to insert a value.");
            yTracking += _ySpacing;

            // 5. Question:  $ _______
            PdfTextUtils.NumberedUnderlineDollarAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "5.", "This is a demo of 'NumberedUnderlineDollarAnswer' with a large number:",
                Fields.GrossEarnings.Value);
            yTracking += _ySpacing;

            // 6. Question:  $ _______
            PdfTextUtils.NumberedUnderlineDollarAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "6.", "This is a demo of 'NumberedUnderlineDollarAnswer' with a small number:",
                Fields.ProcessingFee.Value);
            yTracking += _ySpacing;

            // 7. Section header with sub-questions
            PdfTextUtils.NumberedStatement(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "7.",
                "This is 'NumberedStatement' being used as a header for a section with sub-questions, such as taxes:");
            yTracking += _ySpacing;

            // a. Federal Income Tax
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(gfx,
                _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel,
                yTracking, "a.", "Federal Income Tax", Fields.FederalIncomeTax.Value);
            yTracking += _ySpacingSubAnswer;

            // b. Social Security Tax
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(gfx,
                _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel,
                yTracking, "b.", "Social Security Tax", Fields.SocialSecurityTax.Value);
            yTracking += _ySpacingSubAnswer;

            // c. Medicare Tax
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(gfx,
                _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel,
                yTracking, "c.", "Medicare Tax", Fields.MedicareTax.Value);
            yTracking += _ySpacingSubAnswer;

            // d. Total Taxes
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(gfx,
                _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel,
                yTracking, "d.", "Total Taxes", Fields.TotalTaxes.Value);
            yTracking += _ySpacing;

            // 8. Section header + text paragraph
            PdfTextUtils.NumberedStatement(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "8.",
                "This is 'NumberedStatement' being used as a header for a section for a text paragraph:");
            yTracking += _ySpacing * 0.5;

            string number8Text =
                $"Sometimes, you need to reference the exact verbiage from official documents or forms. " +
                "This text paragraph is a placeholder example for adding lengthy text between questions to mimic official interrogatories. " +
                $"You can still insert values, such as the employee's name {Fields.EmployeeFullName} or maybe the garnishment case " +
                $"number: {Fields.GarnishmentCaseNumber.Value}.";
            double letterBodyHeight = PdfTextUtils.TextRectangle(gfx,
                _marginSubNumber, _rightMarginLetterBody, yTracking, number8Text, addMeasurements);
            yTracking += letterBodyHeight + _ySpacing;

            // 9. Question:  _______
            PdfTextUtils.NumberedUnderlineAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "9.",
                "Here, I'm adding a 'NumberedUnderlineAnswer' under the text paragraph:  ",
                "This should be underlined.");
            yTracking += _ySpacing;

            return yTracking;
        }

        protected double AddLetterQuestionsPage2(XGraphics gfx, double yTracking, bool addMeasurements)
        {
            // 10. Question:  $ _______
            PdfTextUtils.NumberedUnderlineDollarAnswer(gfx, _marginQuestionNumber, _marginQuestionText,
                yTracking, "10.", "This is one final answer, which will be added to the 2nd page:",
                Fields.EstimatedWithholding.Value);
            yTracking += _ySpacing;

            return yTracking;
        }

        public override void CreateTestLetter(bool addMeasurements, string outputPath)
        {
            Console.WriteLine("\nstarted DemoLetterConstructor.CreateTestLetter");

            PdfDocument pdfDocument = CreateDocument(
                "Demo Letter",
                $"Letter: {Fields.LTRName.Value}\nGarnishment Order: {Fields.GARName.Value}");

            // ── Page 1 ──────────────────────────────────────────────────────────
            // Each page's XGraphics context is wrapped in its own block so it is
            // disposed (and the page's content stream closed) before AddFooter
            // opens a new context for the same page. PDFsharp only allows one
            // active XGraphics per page at a time.
            PdfPage page1 = CreatePage(pdfDocument);
            using (XGraphics gfx1 = CreateGraphics(page1))
            {
                AddLetterhead(pdfDocument, page1, gfx1);

                double yTracking = GetInitialYPositionForRELine();
                yTracking = AddRELine(gfx1, yTracking);
                yTracking = AddGreeting(gfx1, yTracking);
                yTracking = AddIntroduction(gfx1, yTracking, addMeasurements);
                AddLetterQuestions(gfx1, yTracking, addMeasurements);
            } // gfx1 disposed here — page 1 content stream is now closed

            // ── Page 2 ──────────────────────────────────────────────────────────
            PdfPage page2 = CreatePage(pdfDocument);
            using (XGraphics gfx2 = CreateGraphics(page2))
            {
                double page2YTracking = GetInitialYPositionForAdditionalPage();
                page2YTracking = AddLetterQuestionsPage2(gfx2, page2YTracking, addMeasurements);
                AddClosingStatements(pdfDocument, page2, gfx2, addMeasurements, page2YTracking);
            } // gfx2 disposed here — page 2 content stream is now closed

            // Footer opens fresh XGraphics contexts for each page — safe now that
            // gfx1 and gfx2 are both disposed.
            AddFooter(pdfDocument);

            Console.WriteLine("saving... DemoLetterConstructor");
            try
            {
                pdfDocument.Save(outputPath);
                Console.WriteLine("saved!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
