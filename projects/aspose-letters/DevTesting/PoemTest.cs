using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Text;
using AsposeLetters.Helpers;

namespace AsposeLetters.DevTesting
{
    internal class PoemTest
    {
        /// <summary>
        /// This method is to test Calculating the height and width of multiple lines of text.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="pageHeightMm"></param>
        public static void CreatTestPoem(Page page, Graph graph) // change the pageHeightMm param, just pull value from page
        {
            Console.WriteLine("started PoemTest.CreatTestPoem");
            double pageHeightMm = page.PageInfo.Height;
            // A drawing rect will be added around the poem (the rect must be transparent as drawing is added to the page after text builder)
            TextFragment textFragmentTitle = new TextFragment("A Line-storm Song\r\n");
            PdfFontUtils.SetFontTimesBoldTitle(textFragmentTitle);
            textFragmentTitle.TextState.HorizontalAlignment = HorizontalAlignment.Center;

            // Use multiple TextSegments to add to a single TextFragment so I can use different font stylings
            TextSegment textSegmentAuthor = new TextSegment("Robert Frost");
            PdfFontUtils.SetFontTextSegmentArialDarkSlateBlue(textSegmentAuthor);

            TextSegment textSegmentAuthorDate = new TextSegment("    1874 - 1963\r\n\r\n");
            PdfFontUtils.SetFontTextSegmentArialMediumSlateBlue(textSegmentAuthorDate);

            // create TextFragment and add the differently formated TextSegments
            TextFragment textFragmentAuthorAndDate = new TextFragment();
            textFragmentAuthorAndDate.Segments.Add(textSegmentAuthor);
            textFragmentAuthorAndDate.Segments.Add(textSegmentAuthorDate);

            TextFragment textFragmentPoem = new TextFragment(
                "The line-storm clouds fly tattered and swift,\r\n  " +
                "The road is forlorn all day,\r\n" +
                "Where a myriad snowy quartz stones lift,\r\n  " +
                "And the hoof-prints vanish away.\r\n" +
                "The roadside flowers, too wet for the bee,\r\n  " +
                "Expend their bloom in vain.\r\n" +
                "Come over the hills and far with me,\r\n  " +
                "And be my love in the rain.\r\n\r\n" +

                "The birds have less to say for themselves\r\n  " +
                "In the wood-world’s torn despair\r\n" +
                "Than now these numberless years the elves,\r\n " +
                "Although they are no less there:\r\n" +
                "All song of the woods is crushed like some\r\n  " +
                "Wild, easily shattered rose. \r\n" +
                "Come, be my love in the wet woods; come, \r\n  " +
                "Where the boughs rain when it blows. \r\n\r\n" +

                "There is the gale to urge behind \r\n  " +
                "And bruit our singing down, \r\n" +
                "And the shallow waters aflutter with wind\r\n  " +
                "From which to gather your gown.    \r\n" +
                "What matter if we go clear to the west, \r\n  " +
                "And come not through dry-shod? \r\n" +
                "For wilding brooch shall wet your breast \r\n  " +
                "The rain-fresh goldenrod. \r\n\r\n" +

                "Oh, never this whelming east wind swells\r\n  " +
                "But it seems like the sea’s return \r\n" +
                "To the ancient lands where it left the shells\r\n  " +
                "Before the age of the fern;\r\n" +
                "And it seems like the time when after doubt\r\n  " +
                "Our love came back amain.      \r\n" +
                "Oh, come forth into the storm and rout\r\n  " +
                "And be my love in the rain.");
            PdfFontUtils.SetFontTimes(textFragmentPoem);

            // Calc Text Height of all TextFragments for the below paragraph rectangle
            double titleHeight = PdfMeasurementUtils.CalculateHeightMultipleRows(textFragmentTitle, "\r\n");
            //Console.WriteLine($"titleHeight: {titleHeight} | titleHeight in mm: {PdfMeasurementUtils.MmToPoints(titleHeight)}");
            double authorHeight = PdfMeasurementUtils.CalculateHeightMultipleRows(textFragmentAuthorAndDate, "\r\n");
            //Console.WriteLine($"authorHeight: {authorHeight} | authorHeight in mm: {PdfMeasurementUtils.MmToPoints(authorHeight)}");
            double poemHeight = PdfMeasurementUtils.CalculateHeightMultipleRows(textFragmentPoem, "\r\n"); // Problem is HERE! this is only measuring 11 mm
            //Console.WriteLine($"poemHeight: {poemHeight} | poemHeight in mm: {PdfMeasurementUtils.MmToPoints(poemHeight)}");
            double totalPoemHeight = titleHeight + authorHeight + poemHeight;
            // calc the poem width 
            double poemWidth = PdfMeasurementUtils.CalculateWidthMultipleRows(textFragmentPoem, "\r\n");
            //Console.WriteLine($"poemWidth: {poemWidth} | poemWidth in mm: {PdfMeasurementUtils.MmToPoints(poemWidth)}");
            poemWidth += 1; // add some extra room

            // X position of rectangle based on Page Width
            float pageWidth = (float)page.PageInfo.Width;
            float centerOfPage = pageWidth * (float)0.5;
            float adjustedXcenterPage = centerOfPage - (float)(poemWidth * 0.5);
            float lowerLeftX = adjustedXcenterPage;
            float upperRightX = lowerLeftX + (float)poemWidth; // left margin + calulated poem width
            // For the Height, get the upper Y first
            // 1. Add the total page height: pageHeightMm (279.4 mm || 297 mm), Y is now at the very top
            // 2. Subtract the measurement where you want the poem Title: 24 mm
            float upperYpoem = (float)PdfMeasurementUtils.FromMMToPoints(24); // To be subtracted from total page height
            float upperRightY = (float)pageHeightMm - upperYpoem; // Rect parameter, Measures from the Bottom
            float lowerLeftY = upperRightY - (float)totalPoemHeight; // Rect parameter, Measures from the Bottom

            TextParagraph textParagraphPoem = new TextParagraph();
            // Rectangle for Poem, different constructor from drawing rect as you set each x/y point
            textParagraphPoem.Rectangle = new Aspose.Pdf.Rectangle(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
            textParagraphPoem.FormattingOptions.WrapMode = TextFormattingOptions.WordWrapMode.ByWords;
            textParagraphPoem.VerticalAlignment = VerticalAlignment.Top;
            textParagraphPoem.HorizontalAlignment = HorizontalAlignment.Left;
            textParagraphPoem.AppendLine(textFragmentTitle);
            textParagraphPoem.AppendLine(textFragmentAuthorAndDate);
            textParagraphPoem.AppendLine(textFragmentPoem);

            // Measure the paragraph rect
            List<TextFragment> tfMeasureLinesList = new List<TextFragment>();
            tfMeasureLinesList.AddRange(PdfDrawingUtils.DrawMeasureLineRectangle(page, graph, textParagraphPoem.Rectangle));
            foreach (TextFragment tf in tfMeasureLinesList)
            {
                page.Paragraphs.Add(tf);
            }
            // Left = X, Top = Y (AddRectangle measures Y from the top)
            // The poem will be placed inside this drawing rect
            double buffer = PdfMeasurementUtils.FromMMToPoints(5);
            //Console.WriteLine($"lowerLeftX MM: {PdfMeasurementUtils.FromPointsToMm(lowerLeftX)} | lowerLeftY MM: {PdfMeasurementUtils.FromPointsToMm(lowerLeftY)}");
            //Console.WriteLine($"poemWidth MM: {PdfMeasurementUtils.FromPointsToMm(poemWidth)} | totalPoemHeight MM: {PdfMeasurementUtils.FromPointsToMm(totalPoemHeight)}");
            PdfDrawingUtils.AddDrawingRectangle(graph, lowerLeftX, lowerLeftY, (float)buffer, poemWidth, totalPoemHeight, Color.Transparent, 0);

            // append the Text Paragraph to the Pdf page with the TextBuilder
            TextBuilder textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(textParagraphPoem);

            double singleLineSpace = PdfMeasurementUtils.FromMMToPoints(3.175);

            // Poem creation explaination
            double rectYtracking = pageHeightMm - PdfMeasurementUtils.FromMMToPoints(30);
            string descRectString1 = "This poem's x position has been calculated based on the total text height and maximum text width. " +
                $"The only hardcoded number is the y indent value.";
            double description1RectHeight = PdfTextUtils.TextRectangle(page, graph, PdfMeasurementUtils.FromMMToPoints(8), PdfMeasurementUtils.FromMMToPoints(65),
                rectYtracking, descRectString1, false);

            rectYtracking -= description1RectHeight + singleLineSpace;
            string descRectString2 = "All text outside of the poem was added using the TextRectangel method in the PdfTextUtils helper class.";
            double description2RectHeight = PdfTextUtils.TextRectangle(page, graph, PdfMeasurementUtils.FromMMToPoints(8), PdfMeasurementUtils.FromMMToPoints(65),
                rectYtracking, descRectString2, false);

            rectYtracking -= description2RectHeight + singleLineSpace;
            string descRectString3 = "The boarder around the poem was added using a Aspose.Pdf.Drawing.Rectangle. My understanding of Aspose is that the drawing " +
                "layer is always added to the page after the text builder layer. If you were to add a color to this drawing rectangle, it will be applied on top " +
                "of the text, regardless of the Z-Index (draw order) set on the Graph object. This drawing rectangle is transparent with a grey border.";
            _ = PdfTextUtils.TextRectangle(page, graph, PdfMeasurementUtils.FromMMToPoints(8), PdfMeasurementUtils.FromMMToPoints(65),
                rectYtracking, descRectString3, false);

        }
    }
}
