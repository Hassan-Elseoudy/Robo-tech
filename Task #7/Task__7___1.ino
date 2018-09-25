#include <string.h>
#define numberOfMotorsAndArms 8
int  m_dir [8];
int  m_pwm [7];
int i = 0;
String data = "";
char temp;

void setup() {
  Serial.begin(9600);
}

void loop() {
  // Input test -> A0B1C0D1E0F1G0H1I2J3K4L5M6N7O8Pa
  while (Serial.available() > 0) {
    temp = Serial.read();
    if (temp != 'a')
      data += temp;
    else if (temp == 'a') {
      for(i = 0 ; i < numberOfMotorsAndArms; i++)
      m_dir[i] = data.substring(data.indexOf(('A' + i)) + 1, data.indexOf(('A') + i + 1)).toInt();
      for(i = 0 ; i < numberOfMotorsAndArms - 1; i++)
      m_pwm[i] = data.substring(data.indexOf(('I' + i)) + 1, data.indexOf(('P') + i + 1)).toInt();
      for (i = 0; i < numberOfMotorsAndArms; i++)
        Serial.println(m_dir[i]);
      for (i = 0; i < numberOfMotorsAndArms - 1; i++)
        Serial.println(m_pwm[i]);
      data = "";
    } 
  }
}
