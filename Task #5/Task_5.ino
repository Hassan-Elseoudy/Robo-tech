void setup() {
  Serial.begin(9600);

}

void loop() {
  char takeTheCharacter = Serial.read();
  while((takeTheCharacter >= 'a' && takeTheCharacter <= 'z') || takeTheCharacter == ' '){
    Serial.println(takeTheCharacter);
    takeTheCharacter = Serial.read();
    }
}
