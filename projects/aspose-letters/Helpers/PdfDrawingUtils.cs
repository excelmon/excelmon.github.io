using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Text;

namespace AsposeLetters.Helpers
{
    internal class PdfDrawingUtils
    {
        public static void AddDrawingRectangle(Graph graph, float xLeft, float yLower, float buffer, double width, double height, Color color, int zIndex)
        {

            float left = xLeft - buffer;
            float bottom = yLower - buffer;
            // initialize a Aspose.Pdf.Drawing.Rectangle
            var rect = new Aspose.Pdf.Drawing.Rectangle(left, bottom, (float)width + buffer * 2, (float)height + buffer * 2)
            {
                GraphInfo =
                {
                    FillColor = color, // background color
                    Color = Color.Gray // border color
                }
            };
            // Add the rectangle to the Shapes collection of the Graph
            graph.Shapes.Add(rect);
            // set the Z-Index (draw order) for the Graph object to control Layering
            graph.ZIndex = zIndex;
        }

        private static void DrawLineTails(Page page, Graph graph, float x1, float y1, float x2, float y2, string orientation, float tailLength)
        {
            switch (orientation)
            {
                case "diagonal":
                    if (x1 < x2 && y1 < y2 || x1 > x2 && y1 > y2)
                    {
                        Line tailOneDia = new Line(new float[] {
                        x1 - tailLength,
                        y1 + tailLength,
                        x1 + tailLength,
                        y1 - tailLength });
                        Line tailTwoDia = new Line(new float[] {
                        x2 - tailLength,
                        y2 + tailLength,
                        x2 + tailLength,
                        y2 - tailLength });
                        tailOneDia.GraphInfo.LineWidth = (float)0.5;
                        tailTwoDia.GraphInfo.LineWidth = (float)0.5;
                        tailOneDia.GraphInfo.Color = Color.Green;
                        tailTwoDia.GraphInfo.Color = Color.Green;
                        graph.Shapes.Add(tailOneDia);
                        graph.Shapes.Add(tailTwoDia);
                    }
                    else // backwards 
                    {
                        //Console.WriteLine("backwards diagonal");
                        Line tailOneDia = new Line(new float[] {
                        x1 - tailLength,
                        y1 - tailLength,
                        x1 + tailLength,
                        y1 + tailLength });
                        Line tailTwoDia = new Line(new float[] {
                        x2 - tailLength,
                        y2 - tailLength,
                        x2 + tailLength,
                        y2 + tailLength });
                        tailOneDia.GraphInfo.LineWidth = (float)0.5;
                        tailTwoDia.GraphInfo.LineWidth = (float)0.5;
                        tailOneDia.GraphInfo.Color = Color.Green;
                        tailTwoDia.GraphInfo.Color = Color.Green;
                        graph.Shapes.Add(tailOneDia);
                        graph.Shapes.Add(tailTwoDia);
                    }
                    break;

                case "diagonal not 90":
                    // Create a small circle using an Ellipse
                    double diameter = PdfMeasurementUtils.FromMMToPoints(0.4);
                    double radius = diameter / 2;
                    Ellipse circleOne = new Ellipse(x1, y1, radius, radius);
                    Ellipse circleTwo = new Ellipse(x2, y2, radius, radius);
                    circleOne.GraphInfo.Color = Color.Green;
                    circleTwo.GraphInfo.Color = Color.Green;
                    graph.Shapes.Add(circleOne);
                    graph.Shapes.Add(circleTwo);
                    break;

                case "horizontal":
                    // vertical tail
                    // add a short line perpendicular to orientation
                    // x will remain the same, y will go above and below
                    Line tailOneHor = new Line(new float[] { x1, y1 - tailLength, x1, y1 + tailLength });
                    Line tailTwoHor = new Line(new float[] { x2, y2 - tailLength, x2, y2 + tailLength });
                    tailOneHor.GraphInfo.LineWidth = (float)0.5;
                    tailTwoHor.GraphInfo.LineWidth = (float)0.5;
                    tailOneHor.GraphInfo.Color = Color.Blue;
                    tailTwoHor.GraphInfo.Color = Color.Blue;
                    graph.Shapes.Add(tailOneHor);
                    graph.Shapes.Add(tailTwoHor);
                    break;
                case "vertical":
                    // horizontal tail
                    // add a short line perpendicular to orientation
                    // y will remain the same, x will go left and right
                    Line tailOneVer = new Line(new float[] { x1 - tailLength, y1, x1 + tailLength, y1 });
                    Line tailTwoVer = new Line(new float[] { x2 - tailLength, y2, x2 + tailLength, y2 });
                    tailOneVer.GraphInfo.LineWidth = (float)0.5;
                    tailTwoVer.GraphInfo.LineWidth = (float)0.5;
                    tailOneVer.GraphInfo.Color = Color.Red;
                    tailTwoVer.GraphInfo.Color = Color.Red;
                    graph.Shapes.Add(tailOneVer);
                    graph.Shapes.Add(tailTwoVer);
                    break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="x1mm">Point 1, X mm Position</param>
        /// <param name="y1mm">Point 1, Y mm Position</param>
        /// <param name="x2mm">Point 2, X mm Position</param>
        /// <param name="y2mm">Point 2, Y mm Position</param>
        /// <returns>A TextFragment to label the measurement</returns>
        public static TextFragment DrawMeasureLineMM(Page page, Graph graph, float x1mm, float y1mm, float x2mm, float y2mm)
        {
            // Page Margins will limit the drawing location and change the x,y position measurements.
            page.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            // X increases to the right, (0,0) is the bottom left corner of the page.
            // Y increases upwards (bottom to top)
            double maxXmm = Math.Max(x1mm, x2mm);
            double minXmm = Math.Min(x1mm, x2mm);
            double maxYmm = Math.Max(y1mm, y2mm);
            double minYmm = Math.Min(y1mm, y2mm);
            float x1 = (float)PdfMeasurementUtils.FromMMToPoints(x1mm);
            float y1 = (float)PdfMeasurementUtils.FromMMToPoints(y1mm);
            float x2 = (float)PdfMeasurementUtils.FromMMToPoints(x2mm);
            float y2 = (float)PdfMeasurementUtils.FromMMToPoints(y2mm);
            double maxXpoints = Math.Max(x1, x2);
            double minXpoints = Math.Min(x1, x2);
            double maxYpoints = Math.Max(y1, y2);
            double minYpoints = Math.Min(y1, y2);

            float halfX = 0;
            float halfY = 0;
            float lineLength;
            float mmLength = 0;
            string orientation = "";
            Color graphColor = Color.Red;
            if (x1 != x2 && y1 != y2)
            {
                if (maxXmm - minXmm == maxYmm - minYmm)
                {
                    orientation = "diagonal";
                }
                else 
                {
                    //Console.WriteLine($"{maxXmm} - {minXmm} != {maxYmm} - {minYmm}");
                    orientation = "diagonal not 90";
                }
                // requires pythagorean theorem
                double legOneLength = maxXpoints - minXpoints; // x triangle leg
                double legTwoLength = maxYpoints - minYpoints; // y triangle leg
                double legThreeLength = Math.Sqrt(Math.Pow(legOneLength, 2) + Math.Pow(legTwoLength, 2));
                lineLength = (float)legThreeLength;

                mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);

                halfX = (float)maxXpoints - (float)legOneLength * (float)0.5;
                halfY = (float)maxYpoints - (float)legTwoLength * (float)0.5;
                graphColor = Color.Green;
            }
            else if (x1 < x2 && y1 == y2 || x1 > x2 && y1 == y2)
            {
                orientation = "horizontal";
                halfY = y2; // y doesn't change
                maxXpoints = Math.Max(x1, x2);
                minXpoints = Math.Min(x1, x2);
                lineLength = (float)maxXpoints - (float)minXpoints;
                mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
                halfX = (float)minXpoints + lineLength * (float)0.5;
                graphColor = Color.Blue; // make horizontal a different color
            }
            else if (y1 < y2 && x1 == x2 || y1 > y2 && x1 == x2)
            {
                orientation = "vertical";
                halfX = x2; // x doesn't change
                maxYpoints = Math.Max(y1, y2);
                minYpoints = Math.Min(y1, y2);
                lineLength = (float)maxYpoints - (float)minYpoints;
                mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
                halfY = (float)minYpoints + lineLength * (float)0.5;

            }
            Line dottedLine = new Line(new float[] { x1, y1, x2, y2 });
            dottedLine.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            dottedLine.GraphInfo.LineWidth = (float)0.5;
            dottedLine.GraphInfo.Color = graphColor;
            graph.Shapes.Add(dottedLine);

            DrawLineTails(page, graph, x1, y1, x2, y2, orientation, 3);

            TextFragment label = new TextFragment($" {mmLength.ToString("0.00")} mm");
            label.Position = new Position(halfX, halfY);
            label.TextState.FontSize = 4;
            label.TextState.Font = FontRepository.FindFont("Arial");
            switch (orientation)
            {
                case "diagonal":
                    label.TextState.Rotation = 315;
                    break;
                case "diagonal not 90":
                    label.TextState.Rotation = 315;
                    break;
                case "horizontal":
                    label.TextState.Rotation = 90;
                    break;
                case "vertical":
                    break;

            }
            return label;
        }

        /// <summary>
        /// Measures between two TextFragments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="tf1"></param>
        /// <param name="tf2"></param>
        /// <returns>A TextFragment to label the measurement</returns>
        public static TextFragment DrawMeasureLineBetweenTF(Page page, Graph graph, TextFragment tf1, TextFragment tf2)
        {
            // Page Margins will limit the drawing location and change the x,y position measurements.
            page.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            float x1 = (float)tf1.Position.XIndent;
            float y1 = (float)tf1.Position.YIndent;
            float x2 = (float)tf2.Position.XIndent;
            float y2 = (float)tf2.Position.YIndent;

            float halfX = 0;
            float halfY = 0;
            float lineLength;
            float mmLength = 0;
            string orientation = "";

            float StartX;
            float EndX;
            float StartY;
            float EndY;

            Color graphColor = Color.Red;
            if (x1 < x2 && y1 == y2 || x1 > x2 && y1 == y2)
            {
                orientation = "horizontal";
                StartX = Math.Max(x1, x2);
                EndX = Math.Min(x1, x2);
                halfY = y2; // y doesn't change
                lineLength = StartX - EndX;
                mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
                halfX = StartX + lineLength * (float)0.5;
                graphColor = Color.Blue;
            }
            else if (y1 < y2 && x1 == x2 || y1 > y2 && x1 == x2)
            {
                orientation = "vertical";
                StartY = Math.Max(y1, y2);
                EndY = Math.Min(y1, y2);
                halfX = x2; // x doesn't change
                lineLength = StartY - EndY;
                mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
                halfY = y1 + lineLength * -1 * (float)0.5; // inverse lineLength
            }

            Line dottedLine = new Line(new float[] { x1, y1, x2, y2 });
            dottedLine.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            dottedLine.GraphInfo.LineWidth = (float)0.5;
            dottedLine.GraphInfo.Color = graphColor;
            graph.Shapes.Add(dottedLine);

            DrawLineTails(page, graph, x1, y1, x2, y2, orientation, 3);

            TextFragment label = new TextFragment($" {mmLength.ToString("0.00")} mm");
            label.Position = new Position(halfX, halfY);
            label.TextState.FontSize = 4;
            label.TextState.Font = FontRepository.FindFont("Arial");
            switch (orientation)
            {
                case "horizontal":
                    label.TextState.Rotation = 90;
                    break;
                case "vertical":
                    break;

            }
            return label;
        }

        /// <summary>
        /// Measures a TextFragment from the left side of the page to the start of the TextFragment.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="tf"></param>
        /// <returns>A TextFragment to label the measurement</returns>
        public static TextFragment DrawMeasureLineTFMargin(Page page, Graph graph, TextFragment tf)
        {
            // Page Margins will limit the drawing location and change the x,y position measurements.
            page.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            float x1 = 0;
            float y1 = (float)tf.Position.YIndent;
            float x2 = (float)tf.Position.XIndent; // measure x diff
            float y2 = (float)tf.Position.YIndent;

            Color graphColor = Color.Blue;

            float EndX = Math.Max(x1, x2);
            float StartX = Math.Min(x1, x2);
            float lineLength = EndX - StartX;
            float mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
            float halfX = StartX + lineLength * (float)0.5;

            Line dottedLine = new Line(new float[] { x1, y1, x2, y2 });
            dottedLine.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            dottedLine.GraphInfo.LineWidth = (float)0.5;
            dottedLine.GraphInfo.Color = graphColor;
            graph.Shapes.Add(dottedLine);

            DrawLineTails(page, graph, x1, y1, x2, y2, "horizontal", 3);

            TextFragment label = new TextFragment($" {mmLength.ToString("0.00")} mm");
            label.Position = new Position(halfX, y2);
            label.TextState.FontSize = 4;
            label.TextState.Font = FontRepository.FindFont("Arial");
            label.TextState.Rotation = 90;
            return label;
        }

        public static TextFragment DrawMeasureLineTSMargin(Page page, Graph graph, TextSegment ts)
        {
            // Page Margins will limit the drawing location and change the x,y position measurements.
            page.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            float x1 = 0;
            float y1 = (float)ts.Position.YIndent;
            float x2 = (float)ts.Position.XIndent; // measure x diff
            float y2 = (float)ts.Position.YIndent;

            Color graphColor = Color.Blue;

            float EndX = Math.Max(x1, x2);
            float StartX = Math.Min(x1, x2);
            float lineLength = EndX - StartX;
            float mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
            float halfX = StartX + lineLength * (float)0.5;

            Line dottedLine = new Line(new float[] { x1, y1, x2, y2 });
            dottedLine.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            dottedLine.GraphInfo.LineWidth = (float)0.5;
            dottedLine.GraphInfo.Color = graphColor;
            graph.Shapes.Add(dottedLine);

            DrawLineTails(page, graph, x1, y1, x2, y2, "horizontal", 3);

            TextFragment label = new TextFragment($" {mmLength.ToString("0.00")} mm");
            label.Position = new Position(halfX, y2);
            label.TextState.FontSize = 4;
            label.TextState.Font = FontRepository.FindFont("Arial");
            label.TextState.Rotation = 90;
            return label;
        }

        /// <summary>
        /// <para>This method will draw millimeter measurements for:</para>
        /// <para>1. Left side of page to the left rectangle wall</para>
        /// <para>2. Rectangle length</para>
        /// <para>3. Right rectangle wall to the right side of page</para>
        /// <para>4. Rectangle height</para>
        /// <para>NOTE: Page Margins will limit the drawing location and change the x,y position measurements, so page margins are changed with this method.</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="graph"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static List<TextFragment> DrawMeasureLineRectangle(Page page, Graph graph, Aspose.Pdf.Rectangle rect)
        {
            page.PageInfo.Margin = new MarginInfo { Right = 0, Bottom = 0, Left = 0, Top = 0 };
            List<TextFragment> tfLabelList = new List<TextFragment>();

            float leftWallPage = 0;
            float rightWallPage = (float)page.PageInfo.Width;
            float leftWallRect = (float)rect.LLX;
            float rightWallRect = (float)rect.URX;
            float lowerWallRect = (float)rect.LLY;
            float upperWallRect = (float)rect.URY;
            Color graphColor = Color.Blue;

            // 1. left side of page to left rect wall
            float lineLength = leftWallRect - leftWallPage;
            float mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
            float halfX = leftWallPage + lineLength * (float)0.5;

            Line leftWallPageToLeftWallRect = new Line(new float[] { leftWallPage, lowerWallRect, leftWallRect, lowerWallRect });
            leftWallPageToLeftWallRect.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            leftWallPageToLeftWallRect.GraphInfo.LineWidth = (float)0.5;
            leftWallPageToLeftWallRect.GraphInfo.Color = graphColor;
            graph.Shapes.Add(leftWallPageToLeftWallRect);
            DrawLineTails(page, graph, leftWallPage, lowerWallRect, leftWallRect, lowerWallRect, "horizontal", 3);

            TextFragment leftWallPageToLeftWallRectLabel = new TextFragment($" {mmLength.ToString("0.00")} mm");
            leftWallPageToLeftWallRectLabel.Position = new Position(halfX, lowerWallRect);
            leftWallPageToLeftWallRectLabel.TextState.FontSize = 4;
            leftWallPageToLeftWallRectLabel.TextState.Font = FontRepository.FindFont("Arial");
            leftWallPageToLeftWallRectLabel.TextState.Rotation = 90;
            tfLabelList.Add(leftWallPageToLeftWallRectLabel);

            // 2. rect length
            lineLength = rightWallRect - leftWallRect;
            mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
            halfX = leftWallRect + lineLength * (float)0.5;

            Line leftWallRectToRightWallRect = new Line(new float[] { leftWallRect, upperWallRect, rightWallRect, upperWallRect });
            leftWallRectToRightWallRect.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            leftWallRectToRightWallRect.GraphInfo.LineWidth = (float)0.5;
            leftWallRectToRightWallRect.GraphInfo.Color = graphColor;
            graph.Shapes.Add(leftWallRectToRightWallRect);
            DrawLineTails(page, graph, leftWallRect, upperWallRect, rightWallRect, upperWallRect, "horizontal", 3);

            TextFragment leftWallRectToRightWallRectLabel = new TextFragment($" {mmLength.ToString("0.00")} mm");
            leftWallRectToRightWallRectLabel.Position = new Position(halfX, upperWallRect);
            leftWallRectToRightWallRectLabel.TextState.FontSize = 4;
            leftWallRectToRightWallRectLabel.TextState.Font = FontRepository.FindFont("Arial");
            leftWallRectToRightWallRectLabel.TextState.Rotation = 90;
            tfLabelList.Add(leftWallRectToRightWallRectLabel);

            // 3. right rect wall to right of page
            lineLength = rightWallPage - rightWallRect;
            mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
            halfX = rightWallRect + lineLength * (float)0.5;

            Line rightWallRectToRightWallPage = new Line(new float[] { rightWallRect, upperWallRect, rightWallPage, upperWallRect });
            rightWallRectToRightWallPage.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            rightWallRectToRightWallPage.GraphInfo.LineWidth = (float)0.5;
            rightWallRectToRightWallPage.GraphInfo.Color = graphColor;
            graph.Shapes.Add(rightWallRectToRightWallPage);
            DrawLineTails(page, graph, rightWallRect, upperWallRect, rightWallPage, upperWallRect, "horizontal", 3);

            TextFragment rightWallRectToRightWallPageLabel = new TextFragment($" {mmLength.ToString("0.00")} mm");
            rightWallRectToRightWallPageLabel.Position = new Position(halfX, upperWallRect);
            rightWallRectToRightWallPageLabel.TextState.FontSize = 4;
            rightWallRectToRightWallPageLabel.TextState.Font = FontRepository.FindFont("Arial");
            rightWallRectToRightWallPageLabel.TextState.Rotation = 90;
            tfLabelList.Add(rightWallRectToRightWallPageLabel);

            // 4. Rect height, put on right wall
            float StartY = Math.Max(lowerWallRect, upperWallRect);
            float EndY = Math.Min(lowerWallRect, upperWallRect);
            lineLength = StartY - EndY;
            mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);
            float halfY = upperWallRect + lineLength * -1 * (float)0.5; // inverse lineLength

            Line rectHeight = new Line(new float[] { rightWallRect, lowerWallRect, rightWallRect, upperWallRect });
            rectHeight.GraphInfo.DashArray = new int[] { 2, 2 }; // 2-point dash, 2-point space
            rectHeight.GraphInfo.LineWidth = (float)0.5;
            rectHeight.GraphInfo.Color = Color.Red;
            graph.Shapes.Add(rectHeight);
            DrawLineTails(page, graph, rightWallRect, lowerWallRect, rightWallRect, upperWallRect, "vertical", 3);

            TextFragment rectHeightLabel = new TextFragment($" {mmLength.ToString("0.00")} mm");
            rectHeightLabel.Position = new Position(rightWallRect, halfY);
            rectHeightLabel.TextState.FontSize = 4;
            rectHeightLabel.TextState.Font = FontRepository.FindFont("Arial");
            tfLabelList.Add(rectHeightLabel);

            return tfLabelList;
        }
    }
}
