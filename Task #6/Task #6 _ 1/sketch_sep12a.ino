#define pushButton 2
#define ledPin 13
int pushButtonCase = 0;

void setup() {
  pinMode(ledPin, OUTPUT);
  pinMode(pushButton, INPUT);
}

void loop() {
  // read the state of the pushbutton value:
  pushButtonCase = digitalRead(pushButton);

  // check pushbutton.
  pushButtonCase == HIGH ? digitalWrite(ledPin, HIGH) : digitalWrite(ledPin, LOW);
}
