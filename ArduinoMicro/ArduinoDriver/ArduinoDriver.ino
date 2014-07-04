#include <Wire.h>
#include <Dagu4Motor.h>

/*************************************************************************************
i2c - http://blog.oscarliang.net/raspberry-pi-arduino-connected-i2c/
Notes
pwmPin: Digital pin to set motor speed
dirPin: Digital pin to set motor direction
currPin: Analog pin to monitor current usage 
*************************************************************************************/
// define slave address (0x2A = 42)
#define SLAVE_ADDRESS 0x2A

// Front - Right
#define FrontRightPwmPin 4
#define FrontRightDirPin 5
#define FrontRightCurPin 14

//Front - Left
#define FrontLeftPwmPin 6
#define FrontLeftDirPin 12
#define FrontLeftCurPin 14

// Back - Right
#define BackRightPwmPin 8
#define BackRightDirPin 9
#define BackRightCurPin 14

// Back - Left
#define BackLeftPwmPin 10
#define BackLeftDirPin 11
#define BackLeftCurPin 14

Dagu4Motor* frontRight;
Dagu4Motor* frontLeft;
Dagu4Motor* backRight;
Dagu4Motor* backLeft;

bool receivingData = false;
int* message;
int messageLength = 0;
int received = 0;
bool processed = true;

//======================================= S E T U P ===============================================
void setup(){ 
        delay(2000);
	Serial.begin(9600);
	InitializeI2C();
	InitializeMotors();
  
	pinMode(13, OUTPUT); //?????????????????/  	
}

void InitializeI2C(){
  Wire.begin(SLAVE_ADDRESS);     
  Wire.onReceive(OnReceiveData);  
  Wire.onRequest(OnRequestData);
  Serial.println("I2C Initialized");
}

void InitializeMotors(){	
	Serial.println("Motors initialization");
	
	String space = ", ";  
	String vhileName = "Front-Right ";
	frontRight =   new Dagu4Motor(FrontRightPwmPin,  FrontRightDirPin, FrontRightCurPin);
	Serial.println(vhileName + FrontRightPwmPin + space + FrontRightDirPin + space + FrontRightCurPin);
	
	vhileName = "Front-Left ";
	frontLeft =   new Dagu4Motor(FrontLeftPwmPin,   FrontLeftDirPin,  FrontLeftCurPin);
	Serial.println(vhileName + FrontLeftPwmPin + space + FrontLeftDirPin + space + FrontLeftCurPin);
	
	vhileName = "Back-Right ";
	backRight  =   new Dagu4Motor(BackRightPwmPin, BackRightDirPin,  BackRightCurPin);
	Serial.println(vhileName + BackRightPwmPin + space + BackRightDirPin + space + BackRightCurPin);
	
	vhileName = "Back-Left ";
	backLeft   =   new Dagu4Motor(BackLeftPwmPin,    BackLeftDirPin,   BackLeftCurPin);
	Serial.println(vhileName + BackLeftPwmPin + space + BackLeftDirPin + space + BackLeftCurPin);
	
	frontRight->begin();
	frontLeft->begin();
	backRight->begin();
	backLeft->begin();
	Serial.println("Motors initialized.");
}

// callback for received data
void OnReceiveData(int byteCount) 
{ 
  receivingData = true;    
  free(message);
  messageLength = 0;
  
  Serial.print("Receive" );
  Serial.println(byteCount);
  
  message = (int*)calloc(byteCount, sizeof(int));
  messageLength = byteCount;
  int received = 0;
  while(Wire.available())
  {
    message[received] = Wire.read();    
    digitalWrite(13, HIGH); 
    delay(200); 
    digitalWrite(13, LOW);    
    received++;
  }    
  processed = false;
  receivingData = false;
}

void OnRequestData(){
	 Wire.write(received - 1);
}

bool direction = false;

void loop()
{  
  // Verify are all data received;
  if(receivingData)
  {
    Serial.println("Data not received");
    delay(10);
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

