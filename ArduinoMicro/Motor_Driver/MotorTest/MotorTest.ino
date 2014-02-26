#include <Wire.h>
#include <Dagu4Motor.h>

/*************************************************************************************
Notes
pwmPin: Digital pin to set motor speed
dirPin: Digital pin to set motor direction
currPin: Analog pin to monitor current usage 
encA: Digital pin for encoder A, should be interrupt pin
encB: Digital pin for encoder B
*************************************************************************************/
Dagu4Motor* motor1;
Dagu4Motor* motor2;
Dagu4Motor* motor3;
Dagu4Motor* motor4;

void setup()
{  
  // define slave address (0x2A = 42)
  #define SLAVE_ADDRESS 0x2A
  // initialize i2c as slave
  Wire.begin(SLAVE_ADDRESS);
   
  // define callbacks for i2c communication
  Wire.onReceive(receiveData);
  Wire.onRequest(sendData); 
  
  Serial.begin(9600);
  Serial.println("Initialized");
  motor1 = new Dagu4Motor(5, 7, 4);
  motor2 = new Dagu4Motor(6, 9, 8 );
  motor3 = new Dagu4Motor(11, 14, 10);
  motor4 = new Dagu4Motor(13, 15, 12);
  
  motor1->begin();
  motor2->begin();
  motor3->begin();
  motor4->begin();  
  
  Serial.println("Initialized");
  delay(2000);
}

// callback for received data
void receiveData(int byteCount) 
{ 
    Serial.print("Receive" );
    Serial.println(byteCount);
}

// callback for sending data
void sendData()
{ 
  Serial.println("Send data");
}

bool direction = false;

void loop()
{  
  Serial.println("New Loop");
  
/*  motor1->stopMotors(); 
  motor1->setMotorDirection(direction);
  motor1->setSpeed(i); */
}
