using PdfSharp.Fonts;
using PdfSharpLetters.DevTesting;
using PdfSharpLetters.Helpers;
using PdfSharpLetters.TestLetters;

// ── Font resolver must be set before any XFont is created ─────────────────────
// PDFsharp's Core build does not auto-discover system fonts.
// WindowsFontResolver reads Times New Roman and Arial directly from C:\Windows\Fonts.
GlobalFontSettings.FontResolver = new WindowsFontResolver();

bool isDevTesting = true; // Set to 'true' to run dev testing code, 'false' to run demo letter constructor code

// Build path dynamically
string username = Environment.UserName;
string outputPath = Path.Combine("C:", "Users", username, "Documents", "PdfSharp Test PDFs");
// Create directory if it doesn't exist (does nothing if it already exists)
Directory.CreateDirectory(outputPath);

// Ensures path ends with directory separator
if (!outputPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
{
    outputPath += Path.DirectorySeparatorChar;
}

string startTime = DateTime.Now.ToString("HH:mm:ss");
Console.WriteLine($"started {startTime}");

// PDF Test: Utilizes PdfTextUtils and other helper classes to quickly build a PDF template
string outputPathDemoLetterConstructor = outputPath + "DemoLetterConstructor.pdf";
new DemoLetterConstructor().CreateTestLetter(false, outputPathDemoLetterConstructor);

// PDF DevTesting: Testing drawing lines and measuring between points.
if (isDevTesting)
{
    string outputPathDraw = outputPath + "DrawAndMeasureTest.pdf";
    DrawMeasurementTestLetter.CreateMeasureTestLetter(outputPathDraw);
}

string finishedTime = DateTime.Now.ToString("HH:mm:ss");
Console.WriteLine($"finished {finishedTime}");
