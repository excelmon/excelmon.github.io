using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpLetters.Helpers;

namespace PdfSharpLetters.DevTesting
{
    /// <summary>
    /// Tests drawing a multi-segment poem with mixed font styles, a bounding rectangle,
    /// and measurement lines — translated from the original Aspose PoemTest.
    ///
    /// KEY DIFFERENCE FROM ASPOSE:
    /// In Aspose, the drawing (Graph) layer was always rendered on top of the TextBuilder
    /// layer regardless of Z-Index, so a filled rectangle would obscure text placed before it.
    /// In PdfSharp, XGraphics draws in the order you call it — text and shapes share one canvas.
    /// You control layering simply by calling draws in the right order: shapes first, text on top.
    /// </summary>
    internal class PoemTest
    {
        public static void CreateTestPoem(PdfDocument doc, PdfPage page, XGraphics gfx)
        {
            Console.WriteLine("started PoemTest.CreateTestPoem");

            var fontTitle = PdfFontUtils.FontTimesBoldTitle();
            var fontTimes = PdfFontUtils.FontTimes();
            var fontArial = PdfFontUtils.FontArial();
            var brushBlack = new XSolidBrush(PdfFontUtils.ColorBlack);
            var brushDarkSlateBlue = new XSolidBrush(PdfFontUtils.ColorDarkSlateBlue);
            var brushMediumSlateBlue = new XSolidBrush(PdfFontUtils.ColorMediumSlateBlue);

            // ── Measure text blocks ────────────────────────────────────────────

            string titleText = "A Line-storm Song";
            string authorText = "Robert Frost";
            string authorDateText = "    1874 - 1963";

            string poemText =
                "The line-storm clouds fly tattered and swift,\n  " +
                "The road is forlorn all day,\n" +
                "Where a myriad snowy quartz stones lift,\n  " +
                "And the hoof-prints vanish away.\n" +
                "The roadside flowers, too wet for the bee,\n  " +
                "Expend their bloom in vain.\n" +
                "Come over the hills and far with me,\n  " +
                "And be my love in the rain.\n\n" +

                "The birds have less to say for themselves\n  " +
                "In the wood-world's torn despair\n" +
                "Than now these numberless years the elves,\n " +
                "Although they are no less there:\n" +
                "All song of the woods is crushed like some\n  " +
                "Wild, easily shattered rose.\n" +
                "Come, be my love in the wet woods; come,\n  " +
                "Where the boughs rain when it blows.\n\n" +

                "There is the gale to urge behind\n  " +
                "And bruit our singing down,\n" +
                "And the shallow waters aflutter with wind\n  " +
                "From which to gather your gown.\n" +
                "What matter if we go clear to the west,\n  " +
                "And come not through dry-shod?\n" +
                "For wilding brooch shall wet your breast\n  " +
                "The rain-fresh goldenrod.\n\n" +

                "Oh, never this whelming east wind swells\n  " +
                "But it seems like the sea's return\n" +
                "To the ancient lands where it left the shells\n  " +
                "Before the age of the fern;\n" +
                "And it seems like the time when after doubt\n  " +
                "Our love came back amain.\n" +
                "Oh, come forth into the storm and rout\n  " +
                "And be my love in the rain.";

            // Measure the widest poem line for centering the bounding box
            double maxPoemLineWidth = 0;
            foreach (string line in poemText.Split('\n'))
            {
                double w = gfx.MeasureString(line.TrimEnd(), fontTimes).Width;
                if (w > maxPoemLineWidth) maxPoemLineWidth = w;
            }

            double lineHeightTitle = fontTitle.GetHeight();
            double lineHeightTimes = fontTimes.GetHeight();
            double lineHeightArial = fontArial.GetHeight();

            int poemLineCount = poemText.Split('\n').Length;
            double totalPoemHeight =
                lineHeightTitle +          // title
                lineHeightArial +          // author
                lineHeightArial +          // author date
                lineHeightTimes +          // blank line after header
                (lineHeightTimes * poemLineCount);

            // ── Position the poem block ────────────────────────────────────────

            double poemBlockWidth = maxPoemLineWidth + PdfMeasurementUtils.FromMMToPoints(2);
            double pageWidth = page.Width;
            double centerX = pageWidth / 2;
            double poemLeftX = centerX - poemBlockWidth / 2;
            double poemTopY = PdfMeasurementUtils.FromMMToPoints(24);

            // Draw bounding rectangle FIRST (background layer) — transparent fill, gray border
            double bufferPts = PdfMeasurementUtils.FromMMToPoints(5);
            gfx.DrawRectangle(new XPen(PdfFontUtils.ColorGray, 0.5),
                poemLeftX - bufferPts, poemTopY - bufferPts,
                poemBlockWidth + bufferPts * 2, totalPoemHeight + bufferPts * 2);

            // Draw measurement overlay on the bounding rect
            var rectForMeasure = new XRect(poemLeftX, poemTopY, poemBlockWidth, totalPoemHeight);
            var labels = PdfDrawingUtils.DrawMeasureLineRectangle(gfx, rectForMeasure);
            foreach (var (labelText, pos) in labels)
            {
                gfx.DrawString(labelText, PdfFontUtils.FontArial(), brushBlack, pos);
            }

            // ── Draw poem text on top of the rectangle ─────────────────────────

            double yPos = poemTopY + lineHeightTitle;
            double titleWidth = gfx.MeasureString(titleText, fontTitle).Width;
            gfx.DrawString(titleText, fontTitle, brushBlack,
                new XPoint(centerX - titleWidth / 2, yPos));
            yPos += lineHeightArial;

            gfx.DrawString(authorText, fontArial, brushDarkSlateBlue, new XPoint(poemLeftX, yPos));
            double authorWidth = gfx.MeasureString(authorText, fontArial).Width;
            gfx.DrawString(authorDateText, fontArial, brushMediumSlateBlue,
                new XPoint(poemLeftX + authorWidth, yPos));
            yPos += lineHeightTimes * 2;

            foreach (string line in poemText.Split('\n'))
            {
                gfx.DrawString(line, fontTimes, brushBlack, new XPoint(poemLeftX, yPos));
                yPos += lineHeightTimes;
            }

            // ── Description text (right column) ───────────────────────────────

            double descLeftMm = 146; // right of the poem
            double descRightMm = 210;
            double descTopY = PdfMeasurementUtils.FromMMToPoints(30);
            double descLeft = PdfMeasurementUtils.FromMMToPoints(descLeftMm);
            double descRight = PdfMeasurementUtils.FromMMToPoints(descRightMm);
            double singleLine = PdfMeasurementUtils.FromMMToPoints(PdfFontUtils.TIMES_9_LINE_HEIGHT_MM);

            string desc1 = "This poem's x position is calculated based on the maximum text width across all lines. " +
                           "The only hardcoded value is the top y indent.";
            double h1 = PdfTextUtils.TextRectangle(gfx, descLeft, descRight, descTopY, desc1, false);
            descTopY += h1 + singleLine;

            string desc2 = "In PdfSharp, drawing order controls layering: the bounding rectangle is drawn first, " +
                           "then text is drawn on top. This is simpler than the Aspose Graph Z-Index model.";
            _ = PdfTextUtils.TextRectangle(gfx, descLeft, descRight, descTopY, desc2, false);
        }
    }
}
