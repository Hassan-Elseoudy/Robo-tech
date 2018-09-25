#define mostDimPin A0
#define leastDimPin A1
#define ledPin 9

void setup() {
  pinMode(mostDimPin, INPUT);
  pinMode(leastDimPin, INPUT);
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  analogWrite(ledPin, ((255.0 / 1023.0) * analogRead(mostDimPin) * 0.75 + (255.0 / 1023.0) * analogRead(leastDimPin) * 0.25));
}
