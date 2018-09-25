//Delay for 1000ms
long time_now = 0, period = 1000;

void setup() {
    Serial.begin(9600);
}
 
void loop() {
    time_now = millis();
    Serial.println("Hello");
    while(millis() < time_now + period){}
}
