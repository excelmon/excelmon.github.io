using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Text;
using AsposeLetters.Helpers;
using AsposeLetters.Sections;

namespace AsposeLetters.DevTesting
{
    internal class DrawMeasurementTestLetter
    {
        /// <summary>
        /// Creates a PDF document to test drawing and measuring the distance between line points in mm. 
        /// </summary>
        /// <param name="outputPath">Output PDF file</param>
        /// <param name="outputTextFile">Output txt file for creating static dictionary</param>
        /// 

        public static void CreateSimpleTestLetter(string outputPath)
        {
            Console.WriteLine("\nstarted DrawMeasurementTestLetter.CreateSimpleTestLetter");
            Document pdfDocument = new();
            // no blank page test
            for (int i = 1; i < 5; i++)
            {
                Page drawPage = pdfDocument.Pages.Add();
                drawPage.SetPageSize(PageSize.PageLetter.Width, PageSize.PageLetter.Height);
                drawPage.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
                Graph graph = new(drawPage.PageInfo.Width, drawPage.PageInfo.Height);
                drawPage.Paragraphs.Add(graph);
                string testString = $"page {i} test, please don't have blank pages.";
                PdfTextUtils.SimpleTextFragmentTimesRed(
                    drawPage,
                    PdfMeasurementUtils.FromMMToPoints(50),
                    PdfMeasurementUtils.FromMMToPoints(20),
                    testString);
            }
            // save document
            Console.WriteLine("saving... SimpleTestLetter");
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

        public static void CreateMeasureTestLetter(string outputPath, string outputFileName)
        {
            Console.WriteLine("\nstarted DrawMeasurementTestLetter.CreateMeasureTestLetter");
            Document pdfDocument = new();
            // Page 1 - Draw lines and measure them
            Page drawPage1 = pdfDocument.Pages.Add();
            drawPage1.SetPageSize(PageSize.PageLetter.Width, PageSize.PageLetter.Height);
            drawPage1.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            Graph graph1 = new(drawPage1.PageInfo.Width, drawPage1.PageInfo.Height);
            drawPage1.Paragraphs.Add(graph1);

            PdfTextUtils.SimpleTextFragmentLetterTitle(drawPage1, "Draw and Measure Test Page");

            List<TextFragment> tfLineList = new();

            // draw some random lines
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 15, 0, 15, 265)); // vertical
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 50, 160, 150, 260)); // diagonal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 0, 100, 210, 100)); // horizontal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 20, 250, 30, 250)); // horizontal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 40, 250, 30, 250)); // backwards horizontal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 60, 230, 80, 230)); // horizontal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 90, 25, 165, 100)); // diagonal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 25, 25, 50, 50)); // diagonal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 180, 10, 190, 20)); // diagonal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 112, 120, 102, 130)); // backwards diagonal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 132, 130, 122, 120)); // diagonal
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 163, 219, 187, 108)); // diagonal not 90 degree
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 122, 130, 187, 108)); // diagonal not 90 degree
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 200, 180, 200, 250)); // vertical
            tfLineList.Add(PdfDrawingUtils.DrawMeasureLineMM(drawPage1, graph1, 190, 190, 190, 180)); // backwards vertical

            foreach (TextFragment tf in tfLineList)
            {
                drawPage1.Paragraphs.Add(tf);
            }

            PdfTextUtils.SimpleTextFragmentTimes(
                drawPage1,
                PdfMeasurementUtils.FromMMToPoints(13),
                PdfMeasurementUtils.FromMMToPoints(10),
                "Just adding some test text below the footer with a fixed position.");

            // Page 2
            Page drawPage2 = pdfDocument.Pages.Add();
            drawPage2.SetPageSize(PageSize.PageLetter.Width, PageSize.PageLetter.Height);
            drawPage2.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            Graph graph2 = new(drawPage2.PageInfo.Width, drawPage2.PageInfo.Height);
            drawPage2.Paragraphs.Add(graph2);

            List<string> listOfCharacters = new List<string>()
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                ".", ",", ";", "?", ":", "#", "%", "$", "&", "\'", "(", ")", "-", "_", "+", "*", "[", "]", " ",
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
            };

            // Column 1 margins
            var marginCharacter1 = PdfMeasurementUtils.FromMMToPoints(13);
            var marginMeasurement1 = PdfMeasurementUtils.FromMMToPoints(16);
            var marginDictKey1 = PdfMeasurementUtils.FromMMToPoints(45);
            var marginDictValue1 = PdfMeasurementUtils.FromMMToPoints(55);

            // Column 2 margins
            var marginCharacter2 = PdfMeasurementUtils.FromMMToPoints(84);
            var marginMeasurement2 = PdfMeasurementUtils.FromMMToPoints(87);
            var marginDictKey2 = PdfMeasurementUtils.FromMMToPoints(116);
            var marginDictValue2 = PdfMeasurementUtils.FromMMToPoints(126);

            // Loop margin var
            double marginCharacterLoop;
            double marginMeasurementLoop;
            double marginDictKeyLoop;
            double marginDictValueLoop;

            var yMaxPosition = PdfMeasurementUtils.FromPointsToMm(drawPage2.PageInfo.Height);
            double yChartStartingPosition = PdfMeasurementUtils.FromMMToPoints(yMaxPosition - 20);
            double yTracking = yChartStartingPosition;
            var count = 0;
            using (StreamWriter sw = File.CreateText(outputFileName))
            {
                for (int i = 0; i < listOfCharacters.Count; i++)
                {
                    count++;
                    // Column 1 margin assignment
                    if (count <= 60)
                    {
                        marginCharacterLoop = marginCharacter1;
                        marginMeasurementLoop = marginMeasurement1;
                        marginDictKeyLoop = marginDictKey1;
                        marginDictValueLoop = marginDictValue1;
                    }
                    // Reset yTracking and use Column 2 assignment
                    else if (count == 61)
                    {
                        yTracking = yChartStartingPosition;
                        marginCharacterLoop = marginCharacter2;
                        marginMeasurementLoop = marginMeasurement2;
                        marginDictKeyLoop = marginDictKey2;
                        marginDictValueLoop = marginDictValue2;

                    }
                    else // Col 2 assignment
                    {
                        marginCharacterLoop = marginCharacter2;
                        marginMeasurementLoop = marginMeasurement2;
                        marginDictKeyLoop = marginDictKey2;
                        marginDictValueLoop = marginDictValue2;
                    }
                    var character = listOfCharacters[i];
                    TextFragment tf = new(character);
                    // Sets font size and family
                    PdfFontUtils.SetFontTimes(tf);
                    double width = PdfMeasurementUtils.CalculateWidth(tf);
                    tf.TextState.BackgroundColor = Color.LightCoral;
                    tf.Position = new Position(marginCharacterLoop, yTracking);
                    drawPage2.Paragraphs.Add(tf);

                    TextFragment tfMeasured = new($"width: {PdfFontUtils.FormatAmountAsString(PdfMeasurementUtils.FromPointsToMm(width))}");
                    PdfFontUtils.SetFontTimes(tfMeasured);
                    tfMeasured.Position = new Position(marginMeasurementLoop, yTracking);
                    drawPage2.Paragraphs.Add(tfMeasured);

                    // Dict Key
                    TextFragment tfKey = new($"{{ '{character}', ");
                    PdfFontUtils.SetFontTimes(tfKey);
                    tfKey.Position = new Position(marginDictKeyLoop, yTracking);
                    drawPage2.Paragraphs.Add(tfKey);

                    // Dict Value
                    string widthString = PdfFontUtils.FormatAmountAsString(PdfMeasurementUtils.FromPointsToMm(width));
                    TextFragment tfMeasuredValue = new($" {widthString} }},");
                    PdfFontUtils.SetFontTimes(tfMeasuredValue);
                    tfMeasuredValue.Position = new Position(marginDictValueLoop, yTracking);
                    drawPage2.Paragraphs.Add(tfMeasuredValue);
                    sw.WriteLine($"{{ '{character}', {widthString} }},"); // write to txt file
                    yTracking -= PdfMeasurementUtils.FromMMToPoints(3.5);
                }
            }

            PdfTextUtils.SimpleTextFragmentTimes(
                drawPage2,
                marginCharacter2,
                yTracking -= PdfMeasurementUtils.FromMMToPoints(7),
                "Times New Roman 9, width measurements by character");


            // Page 3 - Poem
            Page drawPage3 = pdfDocument.Pages.Add();
            drawPage3.SetPageSize(PageSize.PageLetter.Width, PageSize.PageLetter.Height);
            drawPage3.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            Graph graph3 = new(drawPage3.PageInfo.Width, drawPage3.PageInfo.Height);
            drawPage3.Paragraphs.Add(graph3);

            PoemTest.CreatTestPoem(drawPage3, graph3);

            // add footers
            FooterSection footer = new(pdfDocument);
            footer.AddSimpleFooterToDocument();

            // save document
            Console.WriteLine("saving... DrawMeasurementTestLetter");
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
