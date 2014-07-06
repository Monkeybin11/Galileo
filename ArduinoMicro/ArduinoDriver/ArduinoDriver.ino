#include <Wire.h>
#include <Dagu4Motor.h>
/*************************************************************************************
i2c - http://blog.oscarliang.net/raspberry-pi-arduino-connected-i2c/
Notes
pwmPin: Digital pin to set motor speed
dirPin: Digital pin to set motor direction
currPin: Analog pin to monitor current usage 
*************************************************************************************/

#define InvalidMessageType 0 
#define PingMessageType 1 
#define MotorsDataMessageType 2 
#define GetStatusMessageType 3 
#define NewConfiguration 4

struct MotorData{
	byte MotorId;
	byte Direction;
	byte Speed;
};

struct Message{
public:
  bool InProcessing;
  byte messageType;  
  MotorData* Motors;
public:  
  Message(){
    Motors = new MotorData[4]();
  }
};

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

String space = ", ";  
bool receivingData = false;
int* messageBytes;
int messageLength = 0;
byte received = 0;
bool processed = true;
Message message;

//======================================= S E T U P ===============================================
void setup(){ 
    delay(2000);
	Serial.begin(9600);
	InitializeI2C();
	InitializeMotors();    
	pinMode(13, OUTPUT);
}

void InitializeI2C(){
  Wire.begin(SLAVE_ADDRESS);     
  Wire.onReceive(OnReceiveData);  
  Wire.onRequest(OnRequestData);
  Serial.println("I2C Initialized");
}

void InitializeMotors(){	
	Serial.println("Motors initialization");	
	
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
	if(byteCount == 0)
	{
		return;
	}
	receivingData = true;
	messageLength = 0;
  
  Serial.print("Receive" );
  Serial.println(byteCount);
  
  messageBytes = (int*)calloc(byteCount, sizeof(int));
  messageLength = byteCount;
  int received = 0;
  while(Wire.available())
  {
    messageBytes[received] = Wire.read();
    received++;
  }    
  processed = false;
  receivingData = false;  
}

void OnRequestData(){
  Wire.write(message.messageType);
}

bool direction = false;

void ParseNewMotorState(){
	Serial.println("Parsing motors state");
	// If MessageType = MotorsDataMessageType; DataLength => 4*3 = 12. TotalLength = 3+12 = 15
	if(messageBytes[0] != 15) {
		message.messageType = InvalidMessageType;		
		return;
	}	
	message.Motors[messageBytes[2]].MotorId = messageBytes[2];
	message.Motors[messageBytes[2]].Direction = messageBytes[3];
	message.Motors[messageBytes[2]].Speed = messageBytes[4];
	
	message.Motors[messageBytes[5]].MotorId = messageBytes[5];
	message.Motors[messageBytes[5]].Direction = messageBytes[6];
	message.Motors[messageBytes[5]].Speed = messageBytes[7];
	
	message.Motors[messageBytes[8]].MotorId = messageBytes[8];
	message.Motors[messageBytes[8]].Direction = messageBytes[9];
	message.Motors[messageBytes[8]].Speed = messageBytes[10];
	
	message.Motors[messageBytes[11]].MotorId = messageBytes[11];
	message.Motors[messageBytes[11]].Direction = messageBytes[12];
	message.Motors[messageBytes[11]].Speed = messageBytes[13];

        PrintMotorState();
}

void PrintMotorState(){
  	Serial.println("Motors state");
	for(int i = 0; i < 4; i++){		
		Serial.println(message.Motors[i].MotorId + space + message.Motors[i].Direction + space + message.Motors[i].Speed);
	}
}

/* [0] - Length
   [1] - MessageType
   [2 - n-2] - message data
   [n-1] - Check sum 
*/
void ProcessMessage(){

	if(messageLength > 0 && !processed){
		message.InProcessing = true;
		
		if (messageBytes[0] != messageLength){
			message.messageType = InvalidMessageType;
			message.InProcessing = false;
			return;
		}
		
		message.messageType = messageBytes[1];
                String messageType = "Message type - ";
                Serial.println(messageType + message.messageType);
		
		if(message.messageType == MotorsDataMessageType)
                {
	          ParseNewMotorState();
		}
		
		byte checkSum = messageBytes[messageLength-1];
		processed = true;
		message.InProcessing = false;
                free(messageBytes);
                messageLength = 0;
	  }
}

void loop()
{  
  // Verify are all data received;
  if(receivingData)
  {
    Serial.println("Wait for data");
    delay(10);
    return;
  }
  
  ProcessMessage();
  
/*  Serial.println("New Loop");
  
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
  motor4->stopMotors();  */
}
