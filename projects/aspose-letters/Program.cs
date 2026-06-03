using AsposeLetters.TestLetters;
using AsposeLetters.DevTesting;


bool isDevTesting = true; // Set to 'true' to run dev testing code, 'false' to run demo letter constructor code

// Build path dynamically
string username = Environment.UserName;
string outputPath = Path.Combine("C:", "Users", username, "Documents", "Aspose Test PDFs");
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
string outputPathDemoLetterConstrutor = outputPath + "DemoLetterConstructor.pdf";
new DemoLetterConstructor().CreateTestLetter(false, outputPathDemoLetterConstrutor);


// PDF DevTesting: Testing drawing lines, measuring between points and measuring text for font utils.
if (isDevTesting)
{
    string outputPathDraw = outputPath + "DrawAndMeasureTest.pdf";
    string outputFileName = outputPath + "charWidthsDict.txt";
    // Outputs a txt file that can be used to construct a static dict of character measurements (see PdfMeasurementUtils)
    DrawMeasurementTestLetter.CreateMeasureTestLetter(outputPathDraw, outputFileName);
}

string finishedTime = DateTime.Now.ToString("HH:mm:ss");
Console.WriteLine($"finished {finishedTime}");

