using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.Helpers;

namespace PdfSharpLetters.Sections
{
    internal class FooterSection
    {
        private const double FOOTER_Y_POSITION_MM = 258.4;
        private const double FOOTER_INFO_MARGIN_MM = 24.75;
        private const double FOOTER_PAGE_COUNT_MARGIN_MM = 119;
        private const double FOOTER_OCR_MARGIN_MM = 189.5;

        private readonly PdfDocument _pdfDocument;

        public FooterSection(PdfDocument pdfDocument)
        {
            _pdfDocument = pdfDocument;
        }

        public void AddToDocument(string customerId, string letterType, string letterName)
        {
            ApplyFooterToAllPages(page => AddFooterToPage(customerId, letterType, letterName, page));
        }

        public void AddSimpleFooterToDocument()
        {
            ApplyFooterToAllPages(AddSimpleFooterToPage);
        }

        private void ApplyFooterToAllPages(Action<PdfPage> footerAction)
        {
            foreach (PdfPage page in _pdfDocument.Pages)
            {
                footerAction(page);
            }
        }

        private void AddSimpleFooterToPage(PdfPage page)
        {
            using var gfx = XGraphics.FromPdfPage(page);
            var font = PdfFontUtils.FontTimes();
            double yFooter = PdfMeasurementUtils.FromMMToPoints(FOOTER_Y_POSITION_MM);

            // PdfSharp pages are 1-indexed; get current page number by position
            int pageNumber = GetPageNumber(page);
            int pageCount = _pdfDocument.Pages.Count;
            string pageText = $"Page {pageNumber} of {pageCount}";

            double textWidth = gfx.MeasureString(pageText, font).Width;
            double centerX = PdfMeasurementUtils.CalculateCenteredXPosition(page.Width, textWidth);
            gfx.DrawString(pageText, font, new XSolidBrush(PdfFontUtils.ColorBlack),
                new XPoint(centerX, yFooter));
        }

        private void AddFooterToPage(string customerId, string letterType, string letterName, PdfPage page)
        {
            using var gfx = XGraphics.FromPdfPage(page);
            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);

            double yFooter = PdfMeasurementUtils.FromMMToPoints(FOOTER_Y_POSITION_MM);
            double marginInfo = PdfMeasurementUtils.FromMMToPoints(FOOTER_INFO_MARGIN_MM);
            double marginPageCount = PdfMeasurementUtils.FromMMToPoints(FOOTER_PAGE_COUNT_MARGIN_MM);
            double marginOcr = PdfMeasurementUtils.FromMMToPoints(FOOTER_OCR_MARGIN_MM);

            int pageNumber = GetPageNumber(page);
            int pageCount = _pdfDocument.Pages.Count;
            string pageCountText = $"Page {pageNumber} of {pageCount}";

            if (pageNumber == 1)
            {
                gfx.DrawString($"[{customerId}] [{letterName}] [{letterType}]",
                    font, brush, new XPoint(marginInfo, yFooter));
                gfx.DrawString(pageCountText, font, brush, new XPoint(marginPageCount, yFooter));
                gfx.DrawString("d[ o_0 ]b", font, brush, new XPoint(marginOcr, yFooter));
            }
            else
            {
                gfx.DrawString($"[{letterName}]", font, brush, new XPoint(marginInfo, yFooter));
                gfx.DrawString(pageCountText, font, brush, new XPoint(marginPageCount, yFooter));
            }
        }

        private int GetPageNumber(PdfPage page)
        {
            for (int i = 0; i < _pdfDocument.Pages.Count; i++)
            {
                if (_pdfDocument.Pages[i] == page) return i + 1;
            }
            return 1;
        }
    }
}
