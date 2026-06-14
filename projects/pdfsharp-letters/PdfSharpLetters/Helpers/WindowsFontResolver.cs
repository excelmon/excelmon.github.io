using PdfSharp.Fonts;

namespace PdfSharpLetters.Helpers
{
    /// <summary>
    /// Resolves font requests to physical .ttf files in C:\Windows\Fonts.
    /// Covers every typeface used in this project: Times New Roman, Arial.
    ///
    /// Register once at application startup:
    ///     GlobalFontSettings.FontResolver = new WindowsFontResolver();
    ///
    /// Why a custom resolver instead of GlobalFontSettings.UseWindowsFontsUnderWindows?
    /// The UseWindowsFontsUnderWindows flag is documented as "development only" by PDFsharp
    /// and cannot be relied on in production. A custom IFontResolver is the recommended
    /// production approach per https://docs.pdfsharp.net/PDFsharp/Topics/Fonts/Font-Resolving.html
    /// </summary>
    public class WindowsFontResolver : IFontResolver
    {
        // Face name constants — these are the keys passed between ResolveTypeface and GetFont.
        // Using the actual Windows filename (without extension) keeps things explicit.
        private const string TimesNewRoman         = "times";
        private const string TimesNewRomanBold     = "timesbd";
        private const string TimesNewRomanItalic   = "timesi";
        private const string TimesNewRomanBoldItalic = "timesbi";
        private const string Arial                 = "arial";
        private const string ArialBold             = "arialbd";
        private const string ArialItalic           = "ariali";
        private const string ArialBoldItalic       = "arialbi";

        private static readonly string FontFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts");

        // Cache font bytes so each file is read from disk only once per process lifetime.
        private static readonly Dictionary<string, byte[]> _cache = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Maps a (familyName, bold, italic) request to a face name string.
        /// Return null to let PDFsharp fall through to the fallback resolver.
        /// </summary>
        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string normalized = familyName.Trim();

            if (normalized.Equals("Times New Roman", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo(isBold && isItalic ? TimesNewRomanBoldItalic
                                          : isBold             ? TimesNewRomanBold
                                          : isItalic           ? TimesNewRomanItalic
                                                               : TimesNewRoman);
            }

            if (normalized.Equals("Arial", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo(isBold && isItalic ? ArialBoldItalic
                                          : isBold             ? ArialBold
                                          : isItalic           ? ArialItalic
                                                               : Arial);
            }

            // Unknown family — return null so PDFsharp can try the fallback resolver.
            return null;
        }

        /// <summary>
        /// Returns the raw bytes of the font file for the given face name.
        /// faceName is whatever string was returned from ResolveTypeface above.
        /// </summary>
        public byte[]? GetFont(string faceName)
        {
            if (_cache.TryGetValue(faceName, out byte[]? cached))
                return cached;

            string path = Path.Combine(FontFolder, faceName + ".ttf");

            if (!File.Exists(path))
            {
                Console.WriteLine($"[WindowsFontResolver] Font file not found: {path}");
                return null;
            }

            byte[] bytes = File.ReadAllBytes(path);
            _cache[faceName] = bytes;
            return bytes;
        }
    }
}
