/**
 * @file    local_sidereal_time_display_claude.ino
 * @brief   Local Sidereal Time (LST) display for remote viewing session timing.
 *
 * Displays Local Sidereal Time, UTC, and local time on an e-ink screen.
 * Includes a countdown to the peak remote viewing window (12:45–14:15 LST),
 * based on research suggesting ~400% improved accuracy near 13:30 LST when
 * the galactic center aligns with the horizon.
 *
 * Hardware:  Soldered Inkplate 10 (ESP32-based e-ink display)
 * Library:   Inkplate by Soldered (https://github.com/SolderedElectronics/Inkplate-Arduino-library)
 * NTP:       Requires WiFi for time synchronization via pool.ntp.org
 *
 * Configuration:
 *   Set ssid, password, LONGITUDE, LATITUDE, and gmtOffset_sec before flashing.
 *   daylightOffset_sec should remain 3600; DST transitions are handled automatically.
 *
 * @author   Phillip Berger
 * @version  1.2
 * @date     2025
 * @license  MIT
 */

#include "Inkplate.h"
#include "WiFi.h"
#include "time.h"
#include <math.h>

// ---------------------------------------------------------------------------
// User configuration -- edit these before flashing
// ---------------------------------------------------------------------------

// WiFi credentials
const char* ssid     = "TheOasis";
const char* password = "ABC1234567";

// Location (default: Cedar Hill, Texas)
const double LONGITUDE = -96.9561;  // Decimal degrees; West is negative
const double LATITUDE  =  32.5885;  // Decimal degrees; North is positive

// Timezone
// Set gmtOffset_sec to your standard-time UTC offset only (e.g., CST = -6 * 3600).
// Do NOT manually adjust this for DST -- configTime() and the ESP32 SNTP stack
// handle DST automatically using daylightOffset_sec. The UTC conversion functions
// below derive UTC directly from the Unix epoch to avoid any double-subtraction.
const long gmtOffset_sec      = -6 * 3600;  // CST (UTC-6); CDT is handled automatically
const int  daylightOffset_sec =  3600;       // 1 hour DST offset -- do not change

// ---------------------------------------------------------------------------

const char* ntpServer = "pool.ntp.org";

Inkplate display(INKPLATE_1BIT);

// ---------------------------------------------------------------------------
// Helper: center text horizontally
// ---------------------------------------------------------------------------
int getCenterX(String text, int textSize) {
  int textWidth = text.length() * 6 * textSize;  // Approximate character width
  return (display.width() - textWidth) / 2;
}

// ---------------------------------------------------------------------------
// Setup
// ---------------------------------------------------------------------------
void setup() {
  Serial.begin(115200);

  display.begin();
  display.setRotation(3);  // 270 degrees -- landscape, flipped
  display.clearDisplay();
  display.setTextColor(BLACK);

  display.setTextSize(2);
  display.setCursor(50, 50);
  display.print("Connecting to WiFi...");
  display.display();

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("WiFi connected!");

  // configTime sets the ESP32 SNTP client. It will apply gmtOffset_sec and
  // daylightOffset_sec automatically going forward; getLocalTime() returns
  // correctly adjusted local time including DST transitions.
  configTime(gmtOffset_sec, daylightOffset_sec, ntpServer);

  struct tm timeinfo;
  while (!getLocalTime(&timeinfo)) {
    delay(1000);
    Serial.println("Waiting for time sync...");
  }
  Serial.println("Time synchronized");

  display.clearDisplay();
  display.display();
}

// ---------------------------------------------------------------------------
// Main loop
// ---------------------------------------------------------------------------
void loop() {
  updateDisplay();
  delay(30000);  // Refresh every 30 seconds
}

// ---------------------------------------------------------------------------
// Display update
// ---------------------------------------------------------------------------
void updateDisplay() {
  struct tm timeinfo;
  if (!getLocalTime(&timeinfo)) {
    Serial.println("Failed to obtain time");
    return;
  }

  double lst = calculateLocalSiderealTime();
  double utc = calculateUTC();

  display.clearDisplay();

  // --- Header (black background, white text) --------------------------------
  display.fillRect(0, 0, display.width(), 300, BLACK);
  display.setTextColor(WHITE);

  display.setTextSize(4);
  String title = "Local Sidereal Time";
  display.setCursor(getCenterX(title, 4), 20);
  display.print(title);

  display.setTextSize(2);
  String location = "Cedar Hill, Texas, USA";
  display.setCursor(getCenterX(location, 2), 80);
  display.print(location);

  String coords = String(LATITUDE, 4) + " N, " + String(-LONGITUDE, 4) + " W";
  display.setCursor(getCenterX(coords, 2), 110);
  display.print(coords);

  // LST (large)
  display.setTextSize(8);
  int lstHours   = (int)lst;
  int lstMinutes = (int)((lst - lstHours) * 60);
  int lstSeconds = (int)(((lst - lstHours) * 60 - lstMinutes) * 60);
  String lstTime = "";
  if (lstHours   < 10) lstTime += "0";
  lstTime += String(lstHours) + ":";
  if (lstMinutes < 10) lstTime += "0";
  lstTime += String(lstMinutes) + ":";
  if (lstSeconds < 10) lstTime += "0";
  lstTime += String(lstSeconds);

  display.setCursor(getCenterX(lstTime, 8), 180);
  display.print(lstTime);

  display.setTextSize(3);
  String lstLabel = "LST";
  display.setCursor(getCenterX(lstLabel, 3), 250);
  display.print(lstLabel);

  // --- Body (black text on white) ------------------------------------------
  display.setTextColor(BLACK);

  // Local time
  display.setTextSize(3);
  display.setCursor(50, 320);
  display.print("Local Time:");
  display.setCursor(50, 360);
  display.printf("%02d:%02d:%02d %s",
    (timeinfo.tm_hour > 12)  ? timeinfo.tm_hour - 12 :
    (timeinfo.tm_hour == 0)  ? 12 : timeinfo.tm_hour,
    timeinfo.tm_min,
    timeinfo.tm_sec,
    (timeinfo.tm_hour >= 12) ? "PM" : "AM");

  // UTC time
  display.setCursor(50, 420);
  display.print("UTC Time:");
  display.setCursor(50, 460);
  int utcHours   = (int)utc;
  int utcMinutes = (int)((utc - utcHours) * 60);
  int utcSeconds = (int)(((utc - utcHours) * 60 - utcMinutes) * 60);
  display.printf("%02d:%02d:%02d", utcHours, utcMinutes, utcSeconds);

  // --- Psychic cognition window status -------------------------------------
  display.setTextSize(3);
  double lstDecimal = lst;

  if (lstDecimal < 12.75) {
    // Before window: count down to 12:45
    double timeUntilPeak = 12.75 - lstDecimal;
    int hoursUntil   = (int)timeUntilPeak;
    int minutesUntil = (int)((timeUntilPeak - hoursUntil) * 60);
    display.setCursor(50, 520);
    display.print("Time until peak psychic cognition:");
    display.setCursor(50, 560);
    display.printf("%02d:%02d", hoursUntil, minutesUntil);

  } else if (lstDecimal <= 14.25) {
    // Inside peak window: 12:45 to 14:15
    double timeUntilEnd = 14.25 - lstDecimal;
    int hoursUntil   = (int)timeUntilEnd;
    int minutesUntil = (int)((timeUntilEnd - hoursUntil) * 60);
    display.setCursor(50, 520);
    display.print("In peak psychic cognition window now.");
    display.setCursor(50, 560);
    display.printf("Time remaining: %02d:%02d", hoursUntil, minutesUntil);

  } else if (lstDecimal < 17.5) {
    // Post-peak, pre-low: 14:15 to 17:30
    double timeUntilLow = 17.5 - lstDecimal;
    int hoursUntil   = (int)timeUntilLow;
    int minutesUntil = (int)((timeUntilLow - hoursUntil) * 60);
    display.setCursor(50, 520);
    display.print("Time until low psychic cognition:");
    display.setCursor(50, 560);
    display.printf("%02d:%02d", hoursUntil, minutesUntil);

  } else if (lstDecimal <= 20.0) {
    // Galactic center overhead: 17:30 to 20:00
    double timeUntilEnd = 20.0 - lstDecimal;
    int hoursUntil   = (int)timeUntilEnd;
    int minutesUntil = (int)((timeUntilEnd - hoursUntil) * 60);
    display.setCursor(50, 520);
    display.print("Galaxy is directly overhead until 20h");
    display.setCursor(50, 560);
    display.printf("Time remaining: %02d:%02d", hoursUntil, minutesUntil);

  } else {
    // After 20:00: count down to next peak at 12:45
    double timeUntilNextPeak = (24.0 - lstDecimal) + 12.75;
    int hoursUntil   = (int)timeUntilNextPeak;
    int minutesUntil = (int)((timeUntilNextPeak - hoursUntil) * 60);
    display.setCursor(50, 520);
    display.print("Time until next peak psychic cognition:");
    display.setCursor(50, 560);
    display.printf("%02d:%02d", hoursUntil, minutesUntil);
  }

  // --- Date (right column) -------------------------------------------------
  display.setTextSize(3);
  display.setCursor(450, 320);
  display.print("Date:");
  display.setCursor(450, 360);
  const char* months[] = {"Jan", "Feb", "Mar", "Apr", "May", "Jun",
                           "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
  display.printf("%s %d, %d", months[timeinfo.tm_mon], timeinfo.tm_mday,
                 timeinfo.tm_year + 1900);

  // --- WiFi status ---------------------------------------------------------
  display.setTextSize(2);
  display.setCursor(50, 600);
  display.printf("WiFi: %s | Signal: %d dBm",
    (WiFi.status() == WL_CONNECTED) ? "Connected" : "Disconnected",
    WiFi.RSSI());

  // --- Explanatory text ----------------------------------------------------
  display.setCursor(50, 640);
  display.print("Sidereal time is based on Earth's rotation relative to");
  display.setCursor(50, 670);
  display.print("distant stars, not the Sun.");

  display.setCursor(50, 720);
  display.print("At 13:30 h local sidereal time (LST), data shows that one");
  display.setCursor(50, 750);
  display.print("would have 400 percent greater success with remote viewing.");
  display.setCursor(50, 780);
  display.print("At this time, our planet is oriented with the Milky Way so");
  display.setCursor(50, 810);
  display.print("that the galactic center is located directly on the horizon.");
  display.setCursor(50, 840);
  display.print("This daily period of peak psychic cognition lasts for about");
  display.setCursor(50, 870);
  display.print("one and a half hours, from 12:45 to 14:15 h LST. When the");
  display.setCursor(50, 900);
  display.print("center of the galaxy is directly overhead, or at our zenith,");
  display.setCursor(50, 930);
  display.print("psychic cognition drops to its lowest point, between 17:30");
  display.setCursor(50, 960);
  display.print("and 20:00 h LST.");

  display.display();
}

// ---------------------------------------------------------------------------
// calculateLocalSiderealTime()
//
// Derives UTC from the Unix epoch (time(nullptr)) rather than from the tm
// struct fields, which avoids the double-subtraction bug that would occur
// if DST is active: configTime() already folds DST into getLocalTime(), so
// manually subtracting daylightOffset_sec a second time would produce an
// LST that is off by one hour during CDT.
// ---------------------------------------------------------------------------
double calculateLocalSiderealTime() {
  time_t now = time(nullptr);          // Seconds since Unix epoch (always UTC)
  struct tm utcTime;
  gmtime_r(&now, &utcTime);            // Decompose into UTC calendar fields

  int year  = utcTime.tm_year + 1900;
  int month = utcTime.tm_mon  + 1;
  int day   = utcTime.tm_mday;

  if (month <= 2) {
    year--;
    month += 12;
  }

  int a = year / 100;
  int b = 2 - a + (a / 4);

  double jd = floor(365.25 * (year + 4716))
            + floor(30.6001 * (month + 1))
            + day + b - 1524.5;

  double ut = utcTime.tm_hour
            + (utcTime.tm_min  / 60.0)
            + (utcTime.tm_sec  / 3600.0);
  jd += ut / 24.0;

  // Days since J2000.0
  double d = jd - 2451545.0;

  // Greenwich Mean Sidereal Time (hours)
  double gmst = 18.697374558 + 24.06570982441908 * d;
  gmst = fmod(gmst, 24.0);
  if (gmst < 0) gmst += 24.0;

  // Local Sidereal Time
  double lst = gmst + (LONGITUDE / 15.0);
  lst = fmod(lst, 24.0);
  if (lst < 0) lst += 24.0;

  return lst;
}

// ---------------------------------------------------------------------------
// calculateUTC()
//
// Same approach: read the Unix epoch directly so UTC is always correct
// regardless of DST state.
// ---------------------------------------------------------------------------
double calculateUTC() {
  time_t now = time(nullptr);
  struct tm utcTime;
  gmtime_r(&now, &utcTime);

  double ut = utcTime.tm_hour
            + (utcTime.tm_min  / 60.0)
            + (utcTime.tm_sec  / 3600.0);

  // No rollover handling needed; gmtime_r always returns 0-23
  return ut;
}
