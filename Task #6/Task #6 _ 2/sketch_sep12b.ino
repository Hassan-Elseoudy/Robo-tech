#define pushButton1 11
#define pushButton2 12
#define pushButton3 13

#define pinLed0 1
#define pinLed1 2
#define pinLed2 3
#define pinLed3 4
#define pinLed4 5
#define pinLed5 6
#define pinLed6 7
#define pinLed7 8

void setup() {
  pinMode(pinLed1, OUTPUT);
  pinMode(pinLed2, OUTPUT);
  pinMode(pinLed3, OUTPUT);
  pinMode(pinLed4, OUTPUT);
  pinMode(pinLed5, OUTPUT);
  pinMode(pinLed6, OUTPUT);
  pinMode(pinLed7, OUTPUT);

}

void loop() {
  if (digitalRead(pushButton1)) {
    if (digitalRead(pushButton2))
      digitalRead(pushButton3) == 1 ? digitalWrite(pinLed7, HIGH) : digitalWrite(pinLed6, HIGH);
    else
      digitalRead(pushButton3) == 1 ? digitalWrite(pinLed5, HIGH) : digitalWrite(pinLed4, HIGH);
  }
  else {
    if (pushButton2)
      digitalRead(pushButton3) == 1 ? digitalWrite(pinLed3, HIGH) : digitalWrite(pinLed2, HIGH);
    else
      digitalRead(pushButton3) == 1 ? digitalWrite(pinLed1, HIGH) : digitalWrite(pinLed0, HIGH);
  }
  delay(2000);
  digitalWrite(pinLed7, LOW);
  digitalWrite(pinLed6, LOW);
  digitalWrite(pinLed5, LOW);
  digitalWrite(pinLed4, LOW);
  digitalWrite(pinLed3, LOW);
  digitalWrite(pinLed2, LOW);
  digitalWrite(pinLed1, LOW);
  digitalWrite(pinLed0, LOW);
}
