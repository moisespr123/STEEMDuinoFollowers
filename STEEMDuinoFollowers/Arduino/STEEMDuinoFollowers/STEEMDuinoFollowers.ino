#include <LiquidCrystal.h>

LiquidCrystal lcd(12, 11, 5, 4, 3, 2);
String steemname = "";
String followers = "";
String oldfollowers = "";
void setup() {
  lcd.begin(16, 2);
  Serial.begin(38400);
  
}

void loop() {
  int currentinfo = 0;
  steemname = "";
  followers = "";
  if (oldfollowers == "")
    oldfollowers = followers;
  while (Serial.available() == 0)
  {}
  steemname = Serial.readStringUntil('|');
  Serial.read();
  followers = Serial.readStringUntil('\0');
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print(steemname);
  lcd.setCursor(0, 1);
  lcd.print(followers);
  if (oldfollowers != followers)
  {
   tone(8, 800);
   delay(500);
   noTone(8);
   oldfollowers = followers;
  }
  
  Serial.println("OK");
}

