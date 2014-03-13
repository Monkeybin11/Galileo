#include <Wire.h>
#include <Dagu4Motor.h>

/*************************************************************************************
i2c - http://blog.oscarliang.net/raspberry-pi-arduino-connected-i2c/
Notes
pwmPin: Digital pin to set motor speed
dirPin: Digital pin to set motor direction
currPin: Analog pin to monitor current usage 
encA: Digital pin for encoder A, should be interrupt pin
encB: Digital pin for encoder B
*************************************************************************************/
/*Dagu4Motor* motor1;
Dagu4Motor* motor2;
Dagu4Motor* motor3;
Dagu4Motor* motor4;*/

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
/*  motor1 = new Dagu4Motor(4, 5, 14);
  motor2 = new Dagu4Motor(6, 12, 14);
  motor3 = new Dagu4Motor(8, 9, 14);
  motor4 = new Dagu4Motor(10, 11, 14);
  
  motor1->begin();
  motor2->begin();
  motor3->begin();
  motor4->begin();  
  */
  pinMode(13, OUTPUT);
  
  Serial.println("Initialized");
}

bool blinking = false;
bool receivingData = false;
int* message;
int messageLength = 0;
bool processed = true;

// callback for received data
void receiveData(int byteCount) 
{ 
  receivingData = true;    
  free(message);
  messageLength = 0;
  
  Serial.print("Receive" );
  Serial.println(byteCount);
  
  message = (int*)calloc(byteCount, sizeof(int));
  messageLength = byteCount;
  int i  = 0;
  while(Wire.available())
  {
    message[i] = Wire.read();    
    digitalWrite(13, HIGH); delay(200); digitalWrite(13, LOW);    
    i++;
  }    
  processed = false;
  receivingData = false;
}

// callback for sending data
void sendData() { Serial.println("Send data");}

bool direction = false;

void loop()
{  
  // Verify are all data received;
  if(receivingData)
  {
    Serial.println("Data not received");
    delay(100);
    return;
  }
  
  if(messageLength > 0 && !processed){
    Serial.print("Received ");
    for(int i = 0; i< messageLength; i++)
    {
      Serial.print(message[i]);
    }
    Serial.println(".");    
    processed = true;
  }
    
  if(blinking)
  { 
    pinMode(13, OUTPUT);
    digitalWrite(13, HIGH);
  
    delay(1000);
    digitalWrite(13, LOW);
    delay(1000);
  } 
  /*direction = !direction;
  Serial.println("New Loop");
  
  motor1->stopMotors();   
  motor2->stopMotors();   
  motor3->stopMotors();   
  motor4->stopMotors();   
        
  motor1->setMotorDirection(direction);  
  motor2->setMotorDirection(direction);    
  motor3->setMotorDirection(direction);    
  motor4->setMotorDirection(direction);    
  
  for(int i=0; i<255; i++){
    motor1->setSpeed(150); 
    motor2->setSpeed(150);     
    motor3->setSpeed(150); 
    motor4->setSpeed(150);     
    delay(30);
  }
  motor1->stopMotors(); 
  motor2->stopMotors();   
  motor3->stopMotors();   
  motor4->stopMotors(); 
  */
}
