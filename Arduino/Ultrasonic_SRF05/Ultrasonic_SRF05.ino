// Community of Robots
// http://communityofrobots.com/tutorial/kawal/srf05-ultrasonic-sensor-and-arduino
//SRF05 sample code//

void setup()
{
  Serial.begin(9600);
}

const int MaximumDistance = 400;

void loop()
{
  Serial.println(GetDiatance(7));                                              // Wait before looping to do it again
  delay(100);
}

int GetDiatance(int sensorPin)
{
  pinMode(sensorPin, OUTPUT);
  digitalWrite(sensorPin, LOW);                          // Make sure pin is low before sending a short high to trigger ranging
  delayMicroseconds(2);
  digitalWrite(sensorPin, HIGH);                         // Send a short 10 microsecond high burst on pin to start ranging
  delayMicroseconds(10);
  digitalWrite(sensorPin, LOW);                                  // Send pin low again before waiting for pulse back in
  pinMode(sensorPin, INPUT);
  int duration = pulseIn(sensorPin, HIGH);                        // Reads echo pulse in from SRF05 in micro seconds
  return duration/5.8;                                      // Dividing this by 58 gives us a distance in cm
}
