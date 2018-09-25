#include <PS3USB.h>
#include "defineZeros.h"
#define DownLimit 117
#define UpperLimit 137

USB Usb;
PS3USB PS3(&Usb);

int m_dir [6] = {0};
int m_pwm [6] = {0};
int value [4] = {0};  // LHATy, LHATx, L2, R2


void setup() {
  Serial.begin(112500);
}

void loop() {
  Usb.Task();
  if (PS3.PS3Connected) {
    if (PS3.getAnalogHat(LeftHatY) > UpperLimit) {
      value[0] = PS3.getAnalogHat(LeftHatY);
      value[0] = map (value[0], 255, UpperLimit, 255, 0);
      equateElementsArray(m_dir, 0, 4, 0);
      equateElementsArray(m_pwm, 0, 4, value[0]);
    }
    else if (PS3.getAnalogHat(LeftHatY) < DownLimit) {
      value[0] = PS3.getAnalogHat(LeftHatY);
      value[0] = map (value[0], 0, DownLimit, 255, 0);
      equateElementsArray(m_dir, 0, 4, 1);
      equateElementsArray(m_pwm, 0, 4, value[0]);
    }
    if (PS3.getAnalogHat(LeftHatX) > UpperLimit) {
      value[1] = PS3.getAnalogHat(LeftHatX);
      value[1] = map (value[1], 255, UpperLimit, 255, 0);
      m_dir[0] = 0;
      m_dir[1] = 1;
      m_dir[2] = 0;
      m_dir[3] = 1;
      equateElementsArray(m_pwm, 0, 4, value[1]);
    }
    else if (PS3.getAnalogHat(LeftHatX) < DownLimit) {
      value[1] = PS3.getAnalogHat(LeftHatX);
      value[1] = map (value[1], 0, DownLimit, 255, 0);
      m_dir[0] = 1;
      m_dir[1] = 0;
      m_dir[2] = 1;
      m_dir[3] = 0;
      equateElementsArray(m_pwm, 0, 4, value[1]);
    }
    if (PS3.getButtonClick(L2)) {
      m_dir[4] = 1;
      m_dir[5] = 0;
      m_pwm[4] = value[2];
    }
    if (PS3.getButtonClick(R2)) {
      m_dir[4] = 0;
      m_dir[5] = 1;
      m_pwm[5] = value[3];
    }
      equateElementsArray(m_dir, 0, 6, 0);
      equateElementsArray(m_pwm, 0, 6, 0);
  }
}
