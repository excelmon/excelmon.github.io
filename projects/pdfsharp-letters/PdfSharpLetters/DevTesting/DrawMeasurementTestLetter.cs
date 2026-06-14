using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.Helpers;
using System.Reflection.Emit;

namespace PdfSharpLetters.DevTesting
{
    /// <summary>
    /// Dev/testing letter that draws measurement lines and labeled points to help
    /// verify layout positions. Run by setting isDevTesting = true in Program.cs.
    ///
    /// In the original Aspose project this also generated a charWidthsDict.txt output
    /// by measuring each character. In PdfSharp you can use gfx.MeasureString() directly
    /// at draw time, so the static dictionary is only needed for the pre-render height
    /// estimate in PdfMeasurementUtils.CalculateTextRectangleHeight().
    /// </summary>
    internal class DrawMeasurementTestLetter
    {
        public static void CreateMeasureTestLetter(string outputPath)
        {
            Console.WriteLine("\nstarted DrawMeasurementTestLetter.CreateMeasureTestLetter");

            var doc = new PdfDocument();
            doc.Info.Title = "Draw & Measure Test";

            PdfPage page = doc.AddPage();
            page.Width = XUnit.FromMillimeter(PdfTextUtils.LETTER_WIDTH_MM);
            page.Height = XUnit.FromMillimeter(PdfTextUtils.LETTER_HEIGHT_MM);

            var font = PdfFontUtils.FontTimes();
            var brush = new XSolidBrush(PdfFontUtils.ColorBlack);

            using (XGraphics gfx = XGraphics.FromPdfPage(page)) 
            {
                // Draw a grid of horizontal measurement lines every 10 mm from top
                for (int mm = 10; mm <= 270; mm += 10)
                {
                    double y = PdfMeasurementUtils.FromMMToPoints(mm);
                    string label = PdfDrawingUtils.DrawMeasureLineMM(gfx, 0, (float)mm, 215.9f, (float)mm);
                    gfx.DrawString($"{mm}mm  {label}", font, brush, new XPoint(3, y - 2));
                }

                // Draw a sample text block with bounding box visible
                double textY = PdfMeasurementUtils.FromMMToPoints(15);
                double leftX = PdfMeasurementUtils.FromMMToPoints(38);
                double rightX = PdfMeasurementUtils.FromMMToPoints(96);
                string sampleText = "The quick brown fox jumps over the lazy dog. " +
                    "This line tests word-wrap across the standard letter body margin. " +
                    "Additional text ensures multiple lines are produced for height calculation testing.";

                double blockHeight = PdfTextUtils.TextRectangle(gfx, leftX, rightX, textY, sampleText, true);
                Console.WriteLine($"Sample text block height: {PdfMeasurementUtils.FromPointsToMm(blockHeight):0.00} mm");

                // Draw a vertical measurement line on the right margin
                string vLabel = PdfDrawingUtils.DrawMeasureLineMM(gfx, 190, 10, 190, 270); // 260 mm
                gfx.DrawString(vLabel, font, brush, new XPoint(
                    PdfMeasurementUtils.FromMMToPoints(190),
                    PdfMeasurementUtils.FromMMToPoints(130)));

                // Draw a diagonal measurement line across the page
                string dLabel = PdfDrawingUtils.DrawMeasureLineMM(gfx, 0, 0, 215.9f, 279.4f);
                gfx.DrawString(dLabel, font, brush, new XPoint(
                    PdfMeasurementUtils.FromMMToPoints(100),
                    PdfMeasurementUtils.FromMMToPoints(150)));
            }

            PdfPage page2 = doc.AddPage();
            using (XGraphics gfx = XGraphics.FromPdfPage(page2)) 
            {
                PoemTest.CreateTestPoem(doc, page2, gfx); // includes its own measurement overlays
            }

            doc.Save(outputPath);
            Console.WriteLine($"saved: {outputPath}");
        }
    }
}
