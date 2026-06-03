using Aspose.Pdf;
using Aspose.Pdf.Text;
using AsposeLetters.Helpers;

namespace AsposeLetters.Sections
{
    internal class FooterSection
    {
        private const double FOOTER_Y_POSITION_MM = 21;
        private const double FOOTER_INFO_MARGIN_MM = 24.75;
        private const double FOOTER_PAGE_COUNT_MARGIN_MM = 119;
        private const double FOOTER_OCR_MARGIN_MM = 189.5;

        public Document PdfDocument { get; set; }

        public FooterSection(Document pdfDocument)
        {
            PdfDocument = pdfDocument;
        }

        public void AddToDocument(string customerId, string letterType, string letterName)
        {
            ApplyFooterToAllPages(page => AddFooterToPage(customerId, letterType, letterName, page));
        }

        public void AddSimpleFooterToDocument()
        {
            ApplyFooterToAllPages(AddSimpleFooterToPage);
        }

        private void ApplyFooterToAllPages(Action<Page> footerAction)
        {
            foreach (Page page in PdfDocument.Pages)
            {
                footerAction(page);
            }
        }

        private void AddSimpleFooterToPage(Page page) 
        {
            var yFooter = PdfMeasurementUtils.FromMMToPoints(FOOTER_Y_POSITION_MM);

            TextFragment footerPageCount = new TextFragment($"Page {page.Number} of {PdfDocument.Pages.Count}");
            PdfFontUtils.SetFontTimes(footerPageCount);

            double adjustedX = PdfMeasurementUtils.CalculateCenteredXPosition(page, footerPageCount);

            footerPageCount.Position = new Position(adjustedX, yFooter);
            page.Paragraphs.Add(footerPageCount);
        }

        private void AddFooterToPage(string customerId, string letterType, string letterName, Page page) 
        {
            // create the footer and ocr margin
            var yFooter = PdfMeasurementUtils.FromMMToPoints(FOOTER_Y_POSITION_MM);
            var ocrFooterAsterisk = PdfMeasurementUtils.FromMMToPoints(FOOTER_OCR_MARGIN_MM);
            var marginFooterInfo = PdfMeasurementUtils.FromMMToPoints(FOOTER_INFO_MARGIN_MM);
            var marginFooterPageCount = PdfMeasurementUtils.FromMMToPoints(FOOTER_PAGE_COUNT_MARGIN_MM);


            if (page.Number == 1)
            {
                // Footer Info (left), for page 1
                TextFragment footerInfo = new TextFragment($"[{customerId}] [{letterName}] [{letterType}]");
                PdfFontUtils.SetFontTimes(footerInfo);
                footerInfo.Position = new Position(marginFooterInfo, yFooter);
                page.Paragraphs.Add(footerInfo);

                // Footer Page Count
                TextFragment footerPageCount = new TextFragment($"Page {page.Number} of {PdfDocument.Pages.Count}");
                PdfFontUtils.SetFontTimes(footerPageCount);
                footerPageCount.Position = new Position(marginFooterPageCount, yFooter);
                page.Paragraphs.Add(footerPageCount);

                // Footer OCR Text. Make sure this does not move AND is only added to page 1
                TextFragment specialOCRText = new TextFragment("d[ o_0 ]b");
                PdfFontUtils.SetFontTimes(specialOCRText);
                specialOCRText.Position = new Position(ocrFooterAsterisk, yFooter);
                page.Paragraphs.Add(specialOCRText);
            }
            else 
            {
                TextFragment footerInfo = new TextFragment($"[{letterName}]");
                PdfFontUtils.SetFontTimes(footerInfo);
                footerInfo.Position = new Position(marginFooterInfo, yFooter);
                page.Paragraphs.Add(footerInfo);

                // Footer Page Count
                TextFragment footerPageCount = new TextFragment($"Page {page.Number} of {PdfDocument.Pages.Count}");
                PdfFontUtils.SetFontTimes(footerPageCount);
                footerPageCount.Position = new Position(marginFooterPageCount, yFooter);
                page.Paragraphs.Add(footerPageCount);
            }
        }
    }
}
