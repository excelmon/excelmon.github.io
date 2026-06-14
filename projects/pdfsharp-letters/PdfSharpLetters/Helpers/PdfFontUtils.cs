using PdfSharp.Drawing;

namespace PdfSharpLetters.Helpers
{
    /// <summary>
    /// Font factory for the letter project.
    /// Each method returns a ready-to-use XFont. Callers pass it to gfx.DrawString().
    /// XFont is immutable and lightweight — create once per draw call, or cache if desired.
    /// </summary>
    internal class PdfFontUtils
    {
        // Line height constant for Times New Roman size 9 (in mm).
        // Used by word-wrap height calculation in PdfMeasurementUtils.
        public const double TIMES_9_LINE_HEIGHT_MM = 3.175;

        // ── Body text fonts ────────────────────────────────────────────────────

        /// <summary>Times New Roman 9pt, regular — default body text.</summary>
        public static XFont FontTimes()
            => new XFont("Times New Roman", 9, XFontStyleEx.Regular);

        /// <summary>Times New Roman 9pt, bold — for labels and section headers.</summary>
        public static XFont FontTimesBold()
            => new XFont("Times New Roman", 9, XFontStyleEx.Bold);

        /// <summary>Times New Roman 14pt, bold italic — letter title.</summary>
        public static XFont FontTimesBoldTitle()
            => new XFont("Times New Roman", 14, XFontStyleEx.BoldItalic);

        /// <summary>Arial 8pt, regular — supplementary text.</summary>
        public static XFont FontArial()
            => new XFont("Arial", 8, XFontStyleEx.Regular);

        /// <summary>Times New Roman 9pt, regular (alias used for dollar-line text).</summary>
        public static XFont FontDollarLine()
            => new XFont("Times New Roman", 11, XFontStyleEx.Regular);

        // ── Color constants ────────────────────────────────────────────────────

        public static readonly XColor ColorBlack = XColors.Black;
        public static readonly XColor ColorRed = XColors.Red;
        public static readonly XColor ColorDarkBlue = XColors.DarkBlue;
        public static readonly XColor ColorDarkSlateBlue = XColors.DarkSlateBlue;
        public static readonly XColor ColorMediumSlateBlue = XColors.MediumSlateBlue;
        public static readonly XColor ColorGray = XColors.Gray;
        public static readonly XColor ColorGreen = XColors.Green;
        public static readonly XColor ColorBlue = XColors.Blue;

        // ── Formatting helpers ─────────────────────────────────────────────────

        /// <summary>
        /// Converts a double to a currency string: comma-separated with 2 decimal places (1,000.00).
        /// </summary>
        public static string FormatAmountAsString(double amount)
            => string.Format("{0:N2}", amount);
    }
}
