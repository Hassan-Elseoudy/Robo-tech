#define ledPin 13 // the number of the LED pin
int ledState = LOW;             
unsigned long previous = 0;        // Save previous time
const int delayTime = 1000;           // Delay Blinking

void setup() {
  // set the digital pin as output:
  pinMode(ledPin, OUTPUT);
}

void loop() {
  unsigned long current = millis();

  if (current - previous >= delayTime) {
    previous = current;
    ledState = ledState == LOW ? HIGH : LOW;
    // set/reset the led
    digitalWrite(ledPin, ledState);
  }
}
