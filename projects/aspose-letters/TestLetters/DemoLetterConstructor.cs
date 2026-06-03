using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using AsposeLetters.FieldSchema;
using AsposeLetters.Helpers;

namespace AsposeLetters.TestLetters
{
    class DemoLetterConstructor : BaseLetterConstructor
    {
        public DemoLetterConstructor() : base()
        {
        }

        protected override double AddLetterQuestions(Page page, Graph graph, double yTracking, bool addMeasurements)
        {
            // 1. Question:  _______
            string number1QuestionText = "This is a demo of 'NumberedUnderlineAnswer' with a date answer:  ";
            string number1AnswerText = Fields.LetterProcessedDate.Value?.ToString("MMMM d, yyyy") ?? DateOnly.FromDateTime(DateTime.Now).ToString("MMMM d, yyyy");
            PdfTextUtils.NumberedUnderlineAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "1.", number1QuestionText, number1AnswerText);
            yTracking -= _ySpacing;

            // 2. Question:  _______
            string number2QuestionText = "This is a demo of 'NumberedUnderlineAnswer' with a string answer:  ";
            string number2AnswerText = "just a string";
            PdfTextUtils.NumberedUnderlineAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "2.", number2QuestionText, number2AnswerText);
            yTracking -= _ySpacing;

            // 3. Question:  _______
            string number3QuestionText = "This is a demo of 'NumberedUnderlineAnswer' with a double converted to a string:  ";
            string number3AnswerText = Fields.ScheduledWeeklyHours.Value.ToString();
            PdfTextUtils.NumberedUnderlineAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "3.", number3QuestionText, number3AnswerText);
            yTracking -= _ySpacing;

            // 4. Statement to insert a {stringValue} cleanly into a numbered statement (no underline, just text)
            string number4insertValue = "NumberedStatement";
            string number4Statement = $"This is a demo of '{number4insertValue}', which you can use to insert a value.";
            PdfTextUtils.NumberedStatement(page, _marginQuestionNumber, _marginQuestionText, yTracking, "4.", number4Statement);
            yTracking -= _ySpacing;

            // 5. Question:  $ _______
            string number5QuestionText = "This is a demo of 'NumberedUnderlineDollarAnswer' with a large number:";
            PdfTextUtils.NumberedUnderlineDollarAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "5.", number5QuestionText, Fields.GrossEarnings.Value);
            yTracking -= _ySpacing;

            // 6. Question:  $ _______
            string number6QuestionText = "This is a demo of 'NumberedUnderlineDollarAnswer' with a small number:";
            PdfTextUtils.NumberedUnderlineDollarAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "6.", number6QuestionText, Fields.ProcessingFee.Value);
            yTracking -= _ySpacing;

            // 7. String in 'NumberedStatement' being used as a header for a section with sub-questions:
            string number7Statement = "This is 'NumberedStatement' being used as a header for a section with sub-questions, such as taxes:";
            PdfTextUtils.NumberedStatement(page, _marginQuestionNumber, _marginQuestionText, yTracking, "7.", number7Statement);
            yTracking -= _ySpacing;

            // a.  $ __________    Label
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(page, _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel, yTracking,
                "a.", "Federal Income Tax", Fields.FederalIncomeTax.Value);
            yTracking -= _ySpacingSubAnswer; // add 5.25mm to yTracking

            // b.  $ __________    Label
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(page, _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel, yTracking,
                "b.", "Social Security Tax", Fields.SocialSecurityTax.Value);
            yTracking -= _ySpacingSubAnswer;

            // c.  $ __________    Label
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(page, _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel, yTracking,
                "c.", "Medicare Tax", Fields.MedicareTax.Value);
            yTracking -= _ySpacingSubAnswer;

            // d.  $ __________    Label
            PdfTextUtils.NumberedUnderLineDollarAnswerRightLabel(page, _marginSubNumber, _marginSubNumberDollarLine, _marginSubNumberLabel, yTracking,
                "d.", "Total Taxes", Fields.TotalTaxes.Value);
            yTracking -= _ySpacing;

            // 8. String in 'NumberedStatement being used as a header for a section with sub-questions:
            string number8Statement = "This is 'NumberedStatement' being used as a header for a section for a text paragraph:";
            PdfTextUtils.NumberedStatement(page, _marginQuestionNumber, _marginQuestionText, yTracking, "8.", number8Statement);
            yTracking -= _ySpacing * .5; // add a little less spacing after the statement since the next section is a text paragraph, not another question

            string number8Text = $"Sometimes, you need to reference the exact verbiage from official documents or forms. " +
                "This text paragraph is a placeholder example for adding lengthy text between questions to mimic official interrogatories. " +
                $"You can still insert values, such as the employee's name {Fields.EmployeeFullName} or maybe the garnishment case " +
                $"number: {Fields.GarnishmentCaseNumber.Value}.";
            double letterBodyHeight = PdfTextUtils.TextRectangle(page, graph, _marginSubNumber, _rightMarginLetterBody, yTracking, number8Text, addMeasurements);
            yTracking -= letterBodyHeight + _ySpacing;
 

            // 9. Question:  _______
            string number9QuestionText = "Here, I'm adding a 'NumberedUnderlineAnswer' under the text paragraph:  ";
            string number9AnswerText = "This should be underlined.";
            PdfTextUtils.NumberedUnderlineAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "9.", number9QuestionText, number9AnswerText);
            yTracking -= _ySpacing;


            return yTracking;
        }

        // TODO: consider promoting page 2 question pattern to BaseLetterConstructor
        protected double AddLetterQuestionsPage2(Page page, Graph graph, double yTracking, bool addMeasurements)
        {
            // 10. Question:  $ _______
            string number10QuestionText = "This is one final answer, which will be added to the 2nd page:";
            PdfTextUtils.NumberedUnderlineDollarAnswer(page, _marginQuestionNumber, _marginQuestionText, yTracking, "10.", number10QuestionText, Fields.EstimatedWithholding.Value);
            yTracking -= _ySpacing;

            return yTracking;
        }

        public override void CreateTestLetter(bool addMeasurements, string outputPath)
        {
            Console.WriteLine("\nstarted DemoLetterConstructor.CreateTestLetter");

            Document pdfDocument = CreateDocument(
                "Demo Letter",
                $"Letter: {Fields.LTRName.Value}\n" +
                $"Garnishment Order: {Fields.GARName.Value}"); // adds a reference record (Salesforce or otherwise) to the PDF metadata for easier searching and organization

            // Page 1
            Page page1 = CreatePage(pdfDocument);
            Graph graph1 = CreateGraph(page1);


            AddLetterhead(pdfDocument, page1);

            double yTracking = GetInitialYPositionForRELine();
            yTracking = AddRELine(page1, yTracking);
            yTracking = AddGreeting(page1, yTracking);
            yTracking = AddIntroduction(page1, graph1, yTracking, addMeasurements);
            yTracking = AddLetterQuestions(page1, graph1, yTracking, addMeasurements);

            // Page 2
            Page page2 = CreatePage(pdfDocument);
            Graph graph2 = CreateGraph(page2);

            double page2YTracking = GetInitialYPositionForAdditionalPage();
            page2YTracking = AddLetterQuestionsPage2(page2, graph2, page2YTracking, addMeasurements);

            // ===== Closing Statements and Signature =====
            AddClosingStatements(pdfDocument, page2, graph2, addMeasurements, page2YTracking);
            AddFooter(pdfDocument);

            Console.WriteLine("saving... DemoLetterConstructor");
            try
            {
                pdfDocument.Save(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("saved!");
        }
    }
}
