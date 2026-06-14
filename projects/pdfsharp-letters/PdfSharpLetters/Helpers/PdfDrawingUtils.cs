using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfSharpLetters.Helpers
{
    /// <summary>
    /// Drawing utilities: shapes, measurement lines, and dev/testing overlays.
    ///
    /// In Aspose, shapes were added to a Graph object that was attached to a Page.
    /// In PdfSharp, we draw directly onto an XGraphics context — no intermediate Graph.
    /// All methods here take XGraphics gfx instead of Graph graph.
    /// </summary>
    internal class PdfDrawingUtils
    {
        // ── Filled rectangles (e.g. shaded backgrounds) ────────────────────────

        /// <summary>
        /// Draws a filled rectangle with a gray border, optionally buffered outward.
        /// </summary>
        public static void AddDrawingRectangle(XGraphics gfx, float xLeft, float yUpper,
            float buffer, double width, double height, XColor fillColor)
        {
            float left = xLeft - buffer;
            float top = yUpper - buffer;
            float w = (float)width + buffer * 2;
            float h = (float)height + buffer * 2;

            gfx.DrawRectangle(new XPen(PdfFontUtils.ColorGray, 0.5),
                new XSolidBrush(fillColor),
                new XRect(left, top, w, h));
        }

        // ── Measurement lines (dev/testing overlays) ───────────────────────────

        /// <summary>
        /// Draws a dotted measurement line between two mm-coordinate points and
        /// returns a label string (caller places with DrawString if desired).
        /// Orientation is detected automatically; tails are color-coded:
        ///   horizontal → blue, vertical → red, diagonal → green.
        /// </summary>
        public static string DrawMeasureLineMM(XGraphics gfx, float x1mm, float y1mm, float x2mm, float y2mm)
        {
            float x1 = (float)PdfMeasurementUtils.FromMMToPoints(x1mm);
            float y1 = (float)PdfMeasurementUtils.FromMMToPoints(y1mm);
            float x2 = (float)PdfMeasurementUtils.FromMMToPoints(x2mm);
            float y2 = (float)PdfMeasurementUtils.FromMMToPoints(y2mm);

            DetectOrientation(x1, y1, x2, y2,
                out string orientation, out XColor lineColor,
                out float lineLength, out float halfX, out float halfY);

            float mmLength = (float)PdfMeasurementUtils.FromPointsToMm(lineLength);

            // Dotted measurement line
            var pen = new XPen(lineColor, 0.5);
            pen.DashStyle = XDashStyle.Dash;
            gfx.DrawLine(pen, x1, y1, x2, y2);

            DrawLineTails(gfx, x1, y1, x2, y2, orientation);

            return $"{mmLength:0.00} mm";
        }

        /// <summary>
        /// Draws measurement lines around a rectangle (left margin, width, right margin, height).
        /// Returns label strings the caller can draw at the midpoints if desired.
        /// </summary>
        public static List<(string label, XPoint position)> DrawMeasureLineRectangle(
            XGraphics gfx, XRect rect)
        {
            var labels = new List<(string, XPoint)>();

            float leftPage = 0;
            float rightPage = (float)gfx.PageSize.Width;
            float leftRect = (float)rect.X;
            float rightRect = (float)(rect.X + rect.Width);
            float topRect = (float)rect.Y;
            float bottomRect = (float)(rect.Y + rect.Height);

            var pen = new XPen(XColors.Blue, 0.5) { DashStyle = XDashStyle.Dash };

            // 1. Left page edge → left rect wall
            float len1 = leftRect - leftPage;
            gfx.DrawLine(pen, leftPage, bottomRect, leftRect, bottomRect);
            DrawLineTails(gfx, leftPage, bottomRect, leftRect, bottomRect, "horizontal");
            labels.Add(($"{PdfMeasurementUtils.FromPointsToMm(len1):0.00} mm",
                new XPoint(leftPage + len1 / 2, bottomRect)));

            // 2. Left rect wall → right rect wall (width)
            float len2 = rightRect - leftRect;
            gfx.DrawLine(pen, leftRect, topRect, rightRect, topRect);
            DrawLineTails(gfx, leftRect, topRect, rightRect, topRect, "horizontal");
            labels.Add(($"{PdfMeasurementUtils.FromPointsToMm(len2):0.00} mm",
                new XPoint(leftRect + len2 / 2, topRect)));

            // 3. Right rect wall → right page edge
            float len3 = rightPage - rightRect;
            gfx.DrawLine(pen, rightRect, topRect, rightPage, topRect);
            DrawLineTails(gfx, rightRect, topRect, rightPage, topRect, "horizontal");
            labels.Add(($"{PdfMeasurementUtils.FromPointsToMm(len3):0.00} mm",
                new XPoint(rightRect + len3 / 2, topRect)));

            // 4. Rect height (right wall, vertical)
            float len4 = bottomRect - topRect;
            var penRed = new XPen(XColors.Red, 0.5) { DashStyle = XDashStyle.Dash };
            gfx.DrawLine(penRed, rightRect, topRect, rightRect, bottomRect);
            DrawLineTails(gfx, rightRect, topRect, rightRect, bottomRect, "vertical");
            labels.Add(($"{PdfMeasurementUtils.FromPointsToMm(len4):0.00} mm",
                new XPoint(rightRect, topRect + len4 / 2)));

            return labels;
        }

        // ── Private helpers ────────────────────────────────────────────────────

        private static void DetectOrientation(float x1, float y1, float x2, float y2,
            out string orientation, out XColor color, out float lineLength,
            out float halfX, out float halfY)
        {
            if (Math.Abs(y1 - y2) < 0.01f) // horizontal
            {
                orientation = "horizontal";
                color = XColors.Blue;
                float minX = Math.Min(x1, x2);
                float maxX = Math.Max(x1, x2);
                lineLength = maxX - minX;
                halfX = minX + lineLength / 2;
                halfY = y1;
            }
            else if (Math.Abs(x1 - x2) < 0.01f) // vertical
            {
                orientation = "vertical";
                color = XColors.Red;
                float minY = Math.Min(y1, y2);
                float maxY = Math.Max(y1, y2);
                lineLength = maxY - minY;
                halfX = x1;
                halfY = minY + lineLength / 2;
            }
            else // diagonal
            {
                orientation = "diagonal";
                color = XColors.Green;
                float dx = x2 - x1;
                float dy = y2 - y1;
                lineLength = (float)Math.Sqrt(dx * dx + dy * dy);
                halfX = (x1 + x2) / 2;
                halfY = (y1 + y2) / 2;
            }
        }

        private static void DrawLineTails(XGraphics gfx, float x1, float y1, float x2, float y2, string orientation)
        {
            const float tailLength = 3f;
            XPen tailPen;

            switch (orientation)
            {
                case "horizontal":
                    tailPen = new XPen(XColors.Blue, 0.5);
                    gfx.DrawLine(tailPen, x1, y1 - tailLength, x1, y1 + tailLength);
                    gfx.DrawLine(tailPen, x2, y2 - tailLength, x2, y2 + tailLength);
                    break;
                case "vertical":
                    tailPen = new XPen(XColors.Red, 0.5);
                    gfx.DrawLine(tailPen, x1 - tailLength, y1, x1 + tailLength, y1);
                    gfx.DrawLine(tailPen, x2 - tailLength, y2, x2 + tailLength, y2);
                    break;
                case "diagonal":
                    tailPen = new XPen(XColors.Green, 0.5);
                    gfx.DrawLine(tailPen, x1 - tailLength, y1 + tailLength, x1 + tailLength, y1 - tailLength);
                    gfx.DrawLine(tailPen, x2 - tailLength, y2 + tailLength, x2 + tailLength, y2 - tailLength);
                    break;
            }
        }
    }
}
