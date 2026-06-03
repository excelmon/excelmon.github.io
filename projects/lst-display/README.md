# LST Display — `local_sidereal_time_display.ino`

An Arduino sketch for the Soldered Inkplate 10 (ESP32-based e-ink display) that shows Local Sidereal Time (LST), UTC, and local time on a persistent e-ink screen. Built for remote viewing session timing, with a live countdown to the peak LST window based on published parapsychology research.

---

## Background

Local Sidereal Time is a measure of Earth's rotational orientation relative to distant stars rather than the Sun. Research conducted at the Princeton Engineering Anomalies Research (PEAR) lab and elsewhere suggests that remote viewing accuracy improves significantly near 13:30 LST — a period when the galactic center aligns with the horizon. This window, spanning roughly 12:45 to 14:15 LST, has been associated with approximately 400% greater success rates in controlled remote viewing trials.

This display keeps that window visible at a glance without requiring a phone or computer.

---

## Hardware

| Component | Details |
|---|---|
| Board | Soldered Inkplate 10 (ESP32) |
| Display | 9.7" e-ink, 1200×825 resolution |
| Connectivity | WiFi (NTP time sync via `pool.ntp.org`) |
| Power | USB or battery |

The Inkplate 10 is ideal for this use case — e-ink retains the image without power draw between refreshes, and the large screen accommodates all display elements at readable sizes.

---

## Display Layout

**Header (black background, white text)**
- Title: `Local Sidereal Time`
- Location name and decimal coordinates
- Current LST in `HH:MM:SS` format (large)

**Body (white background, black text)**
- Local time (12-hour with AM/PM)
- UTC time
- Peak window status — one of five states:
  - Countdown to peak window (before 12:45 LST)
  - In peak window with time remaining (12:45–14:15 LST)
  - Countdown to low window (14:15–17:30 LST)
  - Galactic center overhead with time remaining (17:30–20:00 LST)
  - Countdown to next peak (after 20:00 LST)
- Date (right column)
- WiFi status and signal strength
- Explanatory text describing the LST / remote viewing relationship

Display refreshes every 30 seconds.

---

## LST Calculation

LST is derived from the Unix epoch (`time(nullptr)`) via Julian Date → Greenwich Mean Sidereal Time → Local Sidereal Time using standard astronomical formulas. UTC is read directly from the epoch rather than from the ESP32's local time struct, which avoids a DST double-subtraction bug: `configTime()` already folds DST into `getLocalTime()`, so deriving UTC from the epoch keeps sidereal calculations correct year-round during CDT/CST transitions.

---

## Configuration

Before flashing, edit the constants at the top of the sketch:

```cpp
// WiFi
const char* ssid     = "YourWifi";
const char* password = "YourWifiPassword";

// Location (decimal degrees; West negative, North positive)
const double LONGITUDE = -96.8089;
const double LATITUDE  =  32.7792;

// Timezone (standard-time offset only -- DST handled automatically)
const long gmtOffset_sec      = -6 * 3600;  // CST (UTC-6)
const int  daylightOffset_sec =  3600;       // do not change
```

`daylightOffset_sec` should remain `3600`. The ESP32 SNTP stack handles DST transitions automatically — do not manually adjust `gmtOffset_sec` for summer time.

---

## Dependencies

- [Inkplate Arduino Library](https://github.com/SolderedElectronics/Inkplate-Arduino-library) by Soldered Electronics
- Arduino IDE with ESP32 board support
- Standard libraries: `WiFi.h`, `time.h`, `math.h`

---

## License

MIT — see file header.

---

## Author

[Phillip Berger](https://phillipberger.com) — phillipberger.com
