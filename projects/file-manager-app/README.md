# FileManagerApp

A .NET 8 C# library for automating file management operations across local and network directories. Built to handle high-volume, repetitive file workflows — moving, renaming, auditing, and organizing files by date — with structured logging and full error handling on every operation.

---

## Project Structure

```
FileManagerApp/          # AppLibrary — the core class library
├── Enums/               # RenameMode, SubDirectoryType
├── FileMovers/          # Move managers (standard and date-filtered)
├── FileRenamers/        # Rename manager (by text or extension)
├── GeneralInterface/    # Shared IDirectoryLocation interface
├── Helpers/             # Factory, FileOrganizer, DirectoryManager, utilities
└── Loggers/             # ILogger / Logger (StreamWriter-based, IDisposable)

ConsoleUI/               # Thin console runner — entry point for scheduled execution
└── Program.cs
```

---

## Features

### File Moving
Move files from an origin directory to a destination, with optional filtering by filename keyword and automatic subdirectory sorting by the file's last-modified date.

```csharp
// Move all PDFs older than 2 weeks into an archive folder
IFileMoveManagerByDate fileManager = Factory.CreateFileMoveManagerByDate(
    originPath, destinationPath, ".pdf",
    fromDate: new DateOnly(2025, 1, 1),
    toDate: DateOnly.FromDateTime(DateTime.Now).AddDays(-14),
    loggerPath, loggerFileName);

fileManager.MoveFiles();
```

Subdirectory sorting options:

| `SubDirectoryType` | Output structure |
|---|---|
| `None` | Files moved flat into destination |
| `Year` | `destination\2025\file.pdf` |
| `YearMonth` | `destination\2025\3 - March\file.pdf` |

### File Renaming
Rename files in-place by replacing a keyword in the filename or swapping a file extension across all matching files.

```csharp
// Replace "IWO" with "Incoming Withholding Order" in all .docx filenames
IFileRenameManager renamer = Factory.CreateFileRenameManager(
    originPath, loggerPath, loggerFileName,
    findWord: "IWO", renameTo: "Incoming Withholding Order",
    fileType: ".docx", RenameMode.ByText);

renamer.RenameFiles();
```

`RenameMode` options:

| Mode | Behavior |
|---|---|
| `ByText` | Replaces matching text within the filename |
| `ByExtension` | Replaces the file extension across all matched files |

### Open File Detection
Scan a directory for files currently locked by another process — useful for identifying files held open before attempting batch operations.

```csharp
IFileOpenManager openManager = Factory.CreateFileOpenManager(
    originPath, ".pdf", loggerPath, loggerFileName);

openManager.FindOpenFiles();
```

Locked files are identified by attempting an exclusive `ReadWrite` open — no third-party handle detection library required. The owner and last modified timestamp are logged for each locked file found.

### Logging
Every operation writes a timestamped log file via a `StreamWriter`-based `Logger`. Logs are created fresh per run with datetime-stamped filenames (e.g. `app-log-2025-03-30.txt`). The logger implements `IDisposable` and is scoped to each manager's lifetime via `using`.

---

## Architecture

The library follows interface-driven design throughout, with dependency injection via a static `Factory` class.

```
IDirectoryLocation          (shared: OriginDirectory, FileType)
    └── IFileMoveManager    (adds: DestinationPath, MoveFiles overloads)
            └── IFileMoveManagerByDate  (adds: FromDate, ToDate)
    └── IFileOpenManager    (FindOpenFiles overloads)
IFileRenameManager          (FindWord, RenameTo, RenameMode, RenameFiles)
ILogger                     (WriteLog, Close, IDisposable)
```

`FileOrganizer` acts as a high-level orchestration layer, composing Factory calls into named, reusable workflow methods. `DirectoryManager` centralizes all path definitions and handles subdirectory creation. `DateTimeExtends` provides `DateTime` → `DateOnly` conversion and timestamp string formatting.

---

## Usage

Workflows are defined as static methods in `FileOrganizer` and called from `ConsoleUI/Program.cs`. To add a new workflow:

1. Add path constants to `DirectoryManager`
2. Define the workflow method in `FileOrganizer` using `Factory`
3. Call it from `Program.cs`

The `ConsoleUI` project is designed to be run on a schedule (Windows Task Scheduler or similar) as a lightweight automation runner.

---

## Requirements

- .NET 8 SDK
- Windows (path conventions and `FileInfo.GetAccessControl()` for locked file owner detection are Windows-specific)

---

## Author

[Phillip Berger](https://phillipberger.com) — phillipberger.com
