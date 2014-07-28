#include "Arduino.h" 

enum MessageTypes{
		InvalidMessageType, 
		PingMessageType, 
		MotorsDataMessageType,
		GetStatusMessageType, 
		NewConfiguration
	};

class TransmissionMessage
{  
private :
	bool isInProcessing;
	MessageTypes messageType;
public: 
	TransmissionMessage(){
		isInProcessing = true;
		messageType = InvalidMessageType;
	}

	bool IsInProcessing(){
		return isInProcessing;
	}	
	
	MessageTypes GetMesageType(){
		return messageType;
	}
};