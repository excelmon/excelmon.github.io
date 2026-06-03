# Claude-Assisted Remote Viewing Protocol

**A structured, verifiable protocol for conducting remote viewing sessions with Claude AI as tasker or viewer — with cryptographic target security built in.**

---

## Why This Protocol Exists

A common objection to AI-assisted remote viewing is the possibility of motivated feedback: an AI tasker might unconsciously (or consciously) adjust the target description after the fact to match the viewer's impressions, making the session feel more successful than it was. Conversely, a human tasker working with an AI viewer might wonder whether the AI is simply generating plausible-sounding impressions rather than genuinely working the target.

This protocol eliminates both concerns by using a **password-protected Word document** as a tamper-evident target container. The target is locked before the session begins. Neither party can alter it without the other knowing. There is no room for motivated adjustment in either direction.

---

## Roles

| Role | Responsibility |
|------|---------------|
| **Tasker** | Selects the target, creates and locks the document, holds the password |
| **Viewer** | Works the target blind, submits impressions before seeing any feedback |

Either Claude or a human can fill either role. The protocol is the same regardless.

---

## The Prompt

To run a session with Claude as Tasker, paste the following into a new Claude chat:

---

```
I'd like to do a remote viewing session using a structured protocol. Please act as my Tasker using the following procedure:

TASKER STEPS (Claude):
1. Generate a random 5-character alphanumeric target label (mixed uppercase letters and digits).
2. Silently choose a real location on Earth with a strong, distinct gestalt — somewhere with notable geography, architecture, or atmosphere. Do not reveal the location yet.
3. Create a password-protected Word (.docx) file containing:
   - Date and time
   - Target label
   - Actual location name
   - A detailed gestalt description of the location (sensory, spatial, atmospheric)
4. Name the file using the target label (e.g. UFN2R.docx).
5. Generate a strong random password, encrypt the document with it, and provide the file for download.
6. Do NOT reveal the password or the location until I have submitted my viewing impressions.

VIEWER STEPS (Me):
1. I will download the word document to my local drive
2. I will work the target using the provided target label.
3. I will submit my impressions in text and/or as a photo of handwritten notes/sketches.
4. Once I submit, you will reveal the password and provide feedback.

FEEDBACK STEPS (Claude):
1. Reveal the password so I can open and verify the document myself.
2. State the target location clearly.
3. Go through my impressions one by one and compare them to the actual target.
4. Note strong hits, partial hits, misses, and any dismissed impressions that were actually signal.
5. Discuss what the session suggests about my data-receiving process.

Please begin by executing the Tasker steps now.
```

---

## Running a Session with a Human as Tasker (Claude as Viewer)

If you want to task Claude instead, use this prompt:

---

```
I'd like to conduct a remote viewing experiment where I am the Tasker and you are the Viewer. Here is the protocol:

TASKER STEPS (Me):
1. I have already selected a target location and created a password-protected Word document containing the target details.
2. I will provide you with the target label only — not the location or any other details.
3. I will share the encrypted document file now so it exists as a tamper-evident record. The password will only be revealed after you submit impressions.

VIEWER STEPS (Claude):
1. Work the provided target label using associative remote viewing principles.
2. Report your impressions: sensory qualities (textures, temperatures, sounds, smells), spatial geometry, emotional tone, motion or stillness, scale, and any specific structural features.
3. Do not speculate about the location by name. Report raw impressions only.
4. Submit impressions before asking for any feedback.

Please confirm you understand the protocol and are ready to receive the target label.
```

---

## Protocol Rules

These rules apply to both roles, human or AI:

- **The target is locked before the session begins.** The password-protected document is the record of this.
- **No impressions are shared before the viewer submits.** The tasker gives only the target label.
- **The viewer submits before unblinding.** No peeking, no asking for hints.
- **The password is shared only after impressions are submitted.** The viewer should open and read the document themselves to verify it matches the feedback given.
- **Dismissed impressions are still data.** If something felt like noise during the session, report it anyway. Post-session analysis often reveals that "noise" was accurate signal.

---

## On Target Security and Trust

The password-protected document is not a formality — it serves a specific function:

A session without target security is unfalsifiable. If the target can be changed after the fact, no result means anything. The document lock makes the session **falsifiable in principle**, which is the minimum requirement for any result to be meaningful.

This applies symmetrically:
- When Claude is the tasker, the viewer can verify that Claude's feedback matches what was locked in the document before the session.
- When a human is the tasker, Claude can verify the same.

Neither party needs to trust the other's memory or intentions. The document is the record.

---

## Tips for Viewers

- **Work in a quiet, low-distraction environment.**
- **Note your Local Sidereal Time (LST).** Some practitioners report improved accuracy during the 13:00–14:00 LST window. Recording LST allows you to track this over multiple sessions.
- **Sketch as well as describe.** Spatial geometry often comes through more clearly in drawings than in words.
- **Report everything, including what feels like noise.** Rhythmic motion, vague sensations, and dismissed impressions are frequently accurate signal in review.
- **Date and label your session notes** before beginning. This is your own tamper-evident record.
- **Don't force specifics.** Report the quality of what you perceive (dark, enclosed, wet, elevated) rather than reaching for a name or category.

---

## Session Log Format (Recommended)

```
Date: 
LST at session start:
Target Label:
---
Impressions:
[Your notes here]
---
Feedback received: [after unblinding]
Password verified: [yes/no]
Document matched feedback: [yes/no]
Notable hits:
Notable misses:
Dismissed impressions that were signal:
```

---

## About This Protocol

This protocol was developed through direct experimentation between a human practitioner and Claude, refining the procedure across live sessions. The password-protection mechanism was introduced specifically to address the falsifiability problem in AI-assisted RV and to give both parties — human and AI — a shared standard of integrity.

The goal is not to prove or disprove remote viewing. The goal is to ensure that whatever results emerge are clean enough to mean something.

---

*Protocol version 1.0 — May 2026*
