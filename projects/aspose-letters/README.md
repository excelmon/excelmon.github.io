# AsposeLetters

A PDF generation framework built with **Aspose.PDF** and **Aspose.Words** for .NET 6, developed to explore programmatic document generation with precise layout control.

## Overview

This project demonstrates how to build structured, data-driven PDF letters using a composable architecture. Rather than relying on templates or mail-merge tools, all layout and text placement is handled in code — giving full control over positioning, formatting, and dynamic content.

The included `DemoLetterConstructor` generates a two-page sample letter that walks through the available utility methods and layout patterns.

## Project Structure

```
AsposeLetters/
├── DevTesting/         # Layout debugging tools (measurement lines, font width charts)
├── FieldSchema/        # Typed field system (TextField, NumberField, DateField, BoolField)
│                       # with Salesforce API name mapping
├── Helpers/            # Core PDF utilities
│   ├── PdfTextUtils         # High-level text placement methods
│   ├── PdfFontUtils         # Font presets and amount formatting
│   ├── PdfDrawingUtils      # Measurement line drawing for layout debugging
│   └── PdfMeasurementUtils  # MM↔points conversion, text width/height calculation
├── Sections/           # Reusable letter sections (Letterhead, Address blocks, Footer, Signature)
├── TestLetters/        # Letter constructors
│   ├── BaseLetterConstructor       # Abstract base with shared layout logic
│   └── DemoLetterConstructor       # Concrete demo implementation
└── Program.cs
```

## Key Concepts

**Section composition** — Each part of a letter (letterhead, address blocks, footer, signature) is an independent class that can be reused across letter types.

**Typed field schema** — Fields are strongly typed (`TextField`, `NumberField`, `DateField`) and carry both a human-readable label and an API name, making it straightforward to swap hardcoded test values for data from an external source like Salesforce.

**MM-based layout** — All positioning is authored in millimeters and converted to PDF points at runtime, making layout measurements match physical print dimensions predictably.

**Text rectangle height estimation** — `PdfMeasurementUtils` uses a precomputed character width dictionary for Times New Roman 9pt to estimate word-wrapped text block heights before rendering, enabling dynamic vertical positioning.

## Running the Project

1. Clone the repo and open `AsposeLetters.sln` in Visual Studio
2. Build and run — output saves to `C:\Users\{username}\Documents\Aspose Test PDFs\`
3. Set `isDevTesting = true` in `Program.cs` to also generate the layout measurement test document

## Dependencies

- [Aspose.PDF for .NET](https://products.aspose.com/pdf/net/) `24.11.0`
- [Aspose.Words for .NET](https://products.aspose.com/words/net/) `25.1.0`

> **Note:** This project runs under the Aspose evaluation license. Output PDFs will include an evaluation watermark. A valid Aspose license is required to generate clean output.

## Notes

The `FieldSchema/Fields.cs` class uses a static structure with hardcoded test values. In a production integration, these values would be populated from an external data source. The Salesforce API names on each field are illustrative of how that mapping would work.
