# Name Parser — `full_name_split.py`

A Python utility for splitting a raw full name string into discrete `first_name` and `last_name` fields. Originally written to support OCR processing of garnishment orders, where name formats vary widely across jurisdictions and data sources.

---

## The Problem

Real-world name data is inconsistent. A single pipeline may encounter all of the following for the same person:

```
Phil Berger
Berger, Phil
Berger-Phil
Phil D. Berger Jr.
Von Berger Phil D
Phil D Berger Zapatero
```

Standard string splits fail silently on most of these. This utility handles them explicitly.

---

## Supported Formats

| Format | Example |
|---|---|
| First Last | `Phil Berger` |
| First Middle Last | `Phil Dale Berger` |
| First M. Last | `Phil D. Berger` |
| Last, First | `Berger, Phil` |
| Last, First Middle | `Berger, Phil Dale` |
| Hyphenated Last-First | `Berger-Phil` |
| Compound last name | `Phil D Berger-Brown` |
| Prefix and surnames | `Von Berger Phil D`, `Phil D Berger Zapatero` |
| With suffixes | `Phil Berger Jr.`, `Phil Dale Berger III` |
| With trailing punctuation | `Phil Berger;`, `Berger, Phil.` |

---

## Usage

```python
first_name, last_name = split_name("Berger, Phil Dale")
# first_name → 'Phil'
# last_name  → 'Berger'

first_name, last_name = split_name("Von Berger Phil D")
# first_name → 'Phil'
# last_name  → 'Von Berger'
```

---

## How It Works

1. **Suffix stripping** — common suffixes (Jr., Sr., I–X) are removed before any pattern matching, preventing them from being misidentified as name parts
2. **Comma detection** — names containing a comma are assumed to be `Last, First` format and handled separately
3. **Pattern matching** — the remaining token count and structure (hyphens, single-character initials, prefix tokens) determine which parsing branch applies
4. **Cleanup** — trailing non-alpha characters (periods, semicolons) are stripped from both output fields

---

## Context

This logic was developed for an OCR garnishment order processing pipeline where incoming name data arrives in inconsistent formats from court-generated documents across multiple states. Accurate name parsing is a prerequisite for downstream record matching and compliance workflows.

---

## Author

[Phillip Berger](https://phillipberger.com) — phillipberger.com
