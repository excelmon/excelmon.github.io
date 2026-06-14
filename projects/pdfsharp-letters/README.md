# PdfSharpLetters

A C# .NET 8 library for generating structured business letters as PDF files, built on [PDFsharp 6.2](https://www.nuget.org/packages/PDFsharp/6.2.1) (MIT license). Originally developed as a proof-of-concept to demonstrate PDF generation for garnishment correspondence, and subsequently converted from Aspose.PDF to PDFsharp to eliminate the commercial license dependency.

<img width="715" height="922" alt="image" src="https://github.com/user-attachments/assets/7966b65a-288a-498e-82f6-63442ec78c3a" />

---

## Background

This project started as an **Aspose.PDF** implementation built to demonstrate PDF letter generation capabilities in a payroll/garnishment operations context. It was then fully converted to **PDFsharp** — an open source, MIT-licensed library — to produce equivalent output without requiring a paid license.

The conversion involved a complete rearchitecture of the rendering model:

| Aspose | PDFsharp |
|---|---|
| `TextFragment` / `TextSegment` objects added to `Page.Paragraphs` | Direct `gfx.DrawString()` calls on an `XGraphics` context |
| `TextBuilder` / `TextParagraph` for word-wrapped blocks | Custom word-wrap loop using `gfx.MeasureString()` |
| `Graph` object for drawing shapes | `XGraphics.DrawLine()`, `DrawRectangle()`, etc. |
| Bottom-left origin, Y increases upward | Top-left origin, Y increases downward |
| Automatic system font discovery | `IFontResolver` required for the Core build |

---

## Project Structure

```
PdfSharpLetters/
├── Program.cs                        # Entry point; registers font resolver and runs letter constructors
│
├── FieldSchema/                      # Typed field definitions that mirror Salesforce API names
│   ├── IField.cs                     # Interface: Label, API name, FieldType, Value
│   ├── TextField.cs
│   ├── NumberField.cs
│   ├── DateField.cs
│   ├── BoolField.cs
│   └── Fields.cs                     # Static registry of all field instances with sample data
│
├── Helpers/                          # Low-level PDF utilities
│   ├── WindowsFontResolver.cs        # IFontResolver implementation — reads .ttf files from C:\Windows\Fonts
│   ├── PdfFontUtils.cs               # XFont factory methods and color constants
│   ├── PdfMeasurementUtils.cs        # MM ↔ points conversion, word-wrap height estimation
│   ├── PdfDrawingUtils.cs            # Shape drawing and measurement line overlays
│   └── PdfTextUtils.cs               # All text drawing patterns (simple, numbered, word-wrapped)
│
├── Sections/                         # Reusable letter sections, each responsible for one visual area
│   ├── IAddress.cs / Address.cs      # Address data model
│   ├── LetterheadSection.cs          # Return address, letter title, date
│   ├── RecipientAddressSection.cs    # Mailing recipient block
│   ├── EmployeeAddressSection.cs     # Debtor/employee address block
│   ├── CustomerAddressSection.cs     # "In Care of" employer address block
│   ├── SignatureSection.cs           # Signature labels and pre-filled values
│   └── FooterSection.cs              # Per-page footer: customer ID, letter name, page count
│
├── TestLetters/                      # Letter constructors that assemble sections into complete documents
│   ├── BaseLetterConstructor.cs      # Abstract base: shared constants, page/document creation, common sections
│   └── DemoLetterConstructor.cs      # Concrete demo letter demonstrating all question/answer patterns
│
└── DevTesting/                       # Development and layout verification tools
    ├── DrawMeasurementTestLetter.cs  # Generates a ruler page with measurement overlays and a text block test
    └── PoemTest.cs                   # Demonstrates mixed font styles, bounding boxes, and layering order
```

---

## Features

### Letter construction patterns

`PdfTextUtils` exposes the following drawing methods, each targeting a specific layout pattern used in garnishment correspondence:

| Method | Pattern | Example |
|---|---|---|
| `SimpleTextFragmentTimes` | Plain text at an absolute position | Address lines, greeting |
| `SimpleTextFragmentTimesRed` | Red text | Date, letter title |
| `SimpleTextFragmentTimesUnderlined` | Underlined text | Email address |
| `NumberedStatement` | `[N.]  [statement]` | Numbered paragraphs, section headers |
| `NumberedUnderlineAnswer` | `[N.]  [question text][underlined answer]` | Text answer fields |
| `NumberedUnderlineDollarAnswer` | `[N.]  [question]  [$ ________] ← [amount]` | Currency answer fields |
| `NumberedUnderLineDollarAnswerRightLabel` | `[a.]  [$ ________] ← [amount]  [Label]` | Tax sub-question rows |
| `TextRectangle` | Word-wrapped paragraph with optional bounding box | Body paragraphs, closing statements |

### Field schema

Fields are defined as typed objects (`TextField`, `NumberField`, `DateField`, `BoolField`) that carry both a human-readable label and a Salesforce API name alongside their value. This makes it straightforward to swap hardcoded sample data for live API responses:

```csharp
public static NumberField GrossEarnings = new("Gross Amount", "Gross_Amount__c", 15000.00);
```

### Font resolver

PDFsharp's Core build (the cross-platform NuGet package) does not auto-discover system fonts. `WindowsFontResolver` implements `IFontResolver` and reads `Times New Roman` and `Arial` directly from `C:\Windows\Fonts`, caching each font file in memory after the first read. It is registered once at startup before any `XFont` is created:

```csharp
GlobalFontSettings.FontResolver = new WindowsFontResolver();
```

To add a new font family, add cases to `ResolveTypeface()` and the corresponding filename constant.

### XGraphics lifetime rule

PDFsharp enforces a single active `XGraphics` context per page. Each page's drawing work is wrapped in its own `using` block so the context is disposed — and the page's content stream closed — before any subsequent operation (such as the footer pass) tries to reopen it:

```csharp
PdfPage page1 = CreatePage(pdfDocument);
using (XGraphics gfx1 = CreateGraphics(page1))
{
    // ... all page 1 drawing ...
} // disposed here

// Footer can now safely open a new context for page 1
AddFooter(pdfDocument);
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- Windows (required by `WindowsFontResolver` — see [Extending](#extending) to support other platforms)

### Running

```bash
dotnet run
```

Output PDFs are written to `C:\Users\<username>\Documents\PdfSharp Test PDFs\`.

Toggle between the demo letter and the dev testing page in `Program.cs`:

```csharp
bool isDevTesting = false; // true → DrawAndMeasureTest.pdf, false → DemoLetterConstructor.pdf
```

Both run simultaneously regardless of the flag — `isDevTesting` only controls whether the measure test page is generated in addition to the demo letter.

### Dependencies

| Package | Version | License |
|---|---|---|
| [PDFsharp](https://www.nuget.org/packages/PDFsharp/6.2.1) | 6.2.1 | MIT |

---

## Dev Testing

Setting `isDevTesting = true` generates `DrawAndMeasureTest.pdf`, a two-page diagnostic document:

**Page 1** — a ruler grid with horizontal measurement lines every 10 mm across the full page width, a sample word-wrapped text block with a visible bounding box, a labeled vertical line, and a diagonal measurement line. Used to verify that `FromMMToPoints` conversions, word-wrap logic, and bounding box positioning are all consistent.

**Page 2** — `PoemTest`: renders Robert Frost's *A Line-storm Song* using mixed font styles (Times New Roman bold-italic title, Arial author attribution, Times New Roman body), a centered layout calculated from live `MeasureString` metrics, a bounding rectangle drawn before the text to demonstrate PdfSharp's painter-order layering, and measurement overlays on the bounding box.

### Coordinate system note

All positions in this project are expressed in **millimeters** and converted to PDF points with `PdfMeasurementUtils.FromMMToPoints()` before being passed to any PdfSharp API. `XPoint`, `DrawLine`, `DrawRectangle`, and `DrawString` all take points — passing a raw millimeter value will place content approximately 2.8× closer to the origin than intended.

```csharp
// Correct
gfx.DrawString(label, font, brush, new XPoint(
    PdfMeasurementUtils.FromMMToPoints(190),
    PdfMeasurementUtils.FromMMToPoints(130)));

// Wrong — 190 and 130 are interpreted as points, not mm
gfx.DrawString(label, font, brush, new XPoint(190, 130));
```

---

## Extending

### Adding a new letter type

1. Create a new class in `TestLetters/` that extends `BaseLetterConstructor`.
2. Implement `AddLetterQuestions(XGraphics gfx, double yTracking, bool addMeasurements)` using the patterns in `PdfTextUtils`.
3. Implement `CreateTestLetter(bool addMeasurements, string outputPath)`, following the scoped `using` block pattern from `DemoLetterConstructor`.
4. Add new fields to `Fields.cs` as needed.

### Supporting non-Windows fonts

Replace or extend `WindowsFontResolver` to load fonts from embedded resources or a configurable path. The `IFontResolver` contract only requires two methods:

```csharp
FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic);
byte[]? GetFont(string faceName);
```

Any font file readable as a `byte[]` can be served — embedded resources, Azure Blob Storage, or a local path on Linux/macOS.

---

## License

This project is provided as a portfolio demonstration. PDFsharp is used under its [MIT license](https://github.com/empira/PDFsharp/blob/master/LICENSE).
