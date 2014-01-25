//void setup() {} void loop() {} /*

#include <Adafruit_LSM303.h>
#include <Wire.h>

int led = 13; // Led on board
int ButtonPin = 5; // button
Adafruit_LSM303 lsm;
int stopped = 0;

void setup() 
{                
  pinMode(led, OUTPUT); // Set LED output  
  pinMode(ButtonPin, INPUT_PULLUP);
  Serial.begin(115200);   // Open Serial connection to PC
  lsm.begin();
  digitalWrite(led, HIGH); // TurnOn led - waiting for Serial connection from PC
  delay(2000);
  while(!Serial)
  {
    if( StopLoopOnButtonClick())
    {
      break;
    }
  }
  digitalWrite(led, LOW); // Serial port connected;  
  Serial.println("Conf.");    
}

Adafruit_LSM303::lsm303AccelData prevAccel;
Adafruit_LSM303::lsm303MagData prevMag;

void PrintDif(Adafruit_LSM303::lsm303AccelData accelData,  Adafruit_LSM303::lsm303MagData magData, int treshold)
{
  int dx = accelData.x- prevAccel.x;
  int dy = accelData.y- prevAccel.y;  
  int dz = accelData.z- prevAccel.z;  
  prevAccel = accelData;  

  if(abs(dx)>treshold || abs(dy)> treshold || abs(dz)>treshold)
  {
    Serial.print("Accel dX: "); 
    Serial.print((int)dx);
    Serial.print(" dY: ");       
    Serial.print((int)dy);
    Serial.print(" dZ: ");       
    Serial.println((int)dz);
  }
}

void PrintData(Adafruit_LSM303::lsm303AccelData accelData,  Adafruit_LSM303::lsm303MagData magData)
{
  Serial.print("Accel X: "); 
  Serial.print((int)accelData.x);
  Serial.print(" Y: ");       
  Serial.print((int)accelData.y);
  Serial.print(" Z: ");       
  Serial.print((int)accelData.z);

  Serial.print("   Mag X: "); 
  Serial.print((int)magData.x);  
  Serial.print(" Y: ");        
  Serial.print((int)magData.y);
  Serial.print(" Z: ");        
  Serial.println((int)magData.z);
}

// the loop routine runs over and over again forever:
void loop() 
{  
  if(stopped)
  {
    Blink(5);
  }
  else
  {
    Blink(1);
    lsm.read();
//    PrintData(lsm.accelData, lsm.magData);
    PrintDif(lsm.accelData, lsm.magData, 30);
    StopLoopOnButtonClick();  
    delay(1000);  
  }
}

int StopLoopOnButtonClick()
{
  if (digitalRead(ButtonPin) == LOW) // Button pressed
  {
    return 0;
  }
  else //Button released
  {
    stopped = 1;
    Serial.println("disc");
    delay(1000);
    Serial.end();
    Blink(3);
    return 1;
  }
}

void Blink(int n)
{
  for (int i = 0; i < n; i++)
  {
    digitalWrite(led, HIGH);
    delay(100);    
    digitalWrite(led, LOW);
    delay(100);          
  }
}

