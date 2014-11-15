/**************************************************************************************
Dagu4Motor.h - Library for driving the Dagu4Motor Driver code. Created by William Garrdio Created: 02/03/2012
*************************************************************************************
Notes
pwmPin: Digital pin to set motor speed
dirPin: Digital pin to set motor direction
currPin: Analog pin to monitor current usage
**************************************************************************************/
#ifndef Dagu4Motor_h  
#define Dagu4Motor_h 
#include "Arduino.h" 

class Dagu4Motor{  
public:  
    Dagu4Motor(int pwmPin, int dirPin, int currPin); 
	void begin();
    void stopMotors();  
    void setSpeed(int speedMotor);  
    void setMotorDirection(bool isMotor);  
	int getCurrent();
	int getSpeed();
	
private:  
    int _pwm;  
    int _dir; 
	int _curr;
	int _currRate;
	float _distance;
	int _speed;
	long int _ticks;
};
#endif