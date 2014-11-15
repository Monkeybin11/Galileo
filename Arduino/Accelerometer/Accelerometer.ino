//void setup() {} void loop() {} 
#include <Adafruit_LSM303.h>
#include <Wire.h>

// http://habrahabr.ru/post/140274/
class KalmanFilter{
public:
  KalmanFilter(double q, double r, double f, double h ){
    Q = q;    R = r;    F = f;    H = h;
  }	
  double X0; // predicted state
  double P0; // predicted covariance
  double F; // factor of real value to previous real value
  double Q; // measurement noise    
  double H; // factor of measured value to real value
  double R; // environment noise
  double State;
  double Covariance;
	
  void SetState(double state, double covariance){
    State = state;
    Covariance = covariance;
  }

  double Calculate(double data){
    //time update - prediction
    X0 = F*State;
    P0 = F*Covariance*F + Q;
    //measurement update - correction
    double K = H*P0 / (H*P0*H + R);
    State = X0 + K*(data - H*X0);
    Covariance = (1 - K*H)*P0;
    return State;
  }
};

int led = 13; // Led on board
int ButtonPin = 5; // button
Adafruit_LSM303 lsm;

KalmanFilter* filterAx = new KalmanFilter(1,20,1,1); // Q - measurement noise; R - environment noise; F - factor of real value to previous real value; H - factor of measured value to real value
KalmanFilter* filterAy = new KalmanFilter(1,20,1,1);
KalmanFilter* filterAz = new KalmanFilter(1,20,1,1);

int stopped = 0;

void setup() {                
  pinMode(led, OUTPUT); // Set LED output  
  pinMode(ButtonPin, INPUT_PULLUP);
  Serial.begin(115200);   // Open Serial connection to PC
  lsm.begin();
  digitalWrite(led, HIGH); // TurnOn led - waiting for Serial connection from PC
  delay(2000);
  while(!Serial)  {
    if( StopLoopOnButtonClick()) {
      break;
    }
  }  
    lsm.read();    
    lsm.read();    
    filterAx->SetState(lsm.accelData.x, 10); // Firs point and predictable error
    filterAy->SetState(lsm.accelData.y, 10); // Firs point and predictable error
    filterAz->SetState(lsm.accelData.z, 10); // Firs point and predictable error
  
  digitalWrite(led, LOW); // Serial port connected;  
  Serial.println("Conf.");
}

void PrintData(Adafruit_LSM303::lsm303AccelData accelData,  double x, double y, double z){
  const char delimeter = ' ';
  Serial.print((int)accelData.x);   Serial.print(delimeter); Serial.print((int)accelData.y);   Serial.print(delimeter); Serial.print((int)accelData.z);
  Serial.print(delimeter); Serial.print(x); Serial.print(delimeter); Serial.print(y);                  Serial.print(delimeter); Serial.println(z);
}

// the loop routine runs over and over again forever:
void loop() {  
  if(stopped)  {
    Blink(5);
  }
  else  {
//    Blink(1);
    lsm.read();    
    double x = filterAx->Calculate(lsm.accelData.x);
    double y = filterAy->Calculate(lsm.accelData.y);
    double z = filterAz->Calculate(lsm.accelData.z);
    PrintData(lsm.accelData, x, y, z);
    StopLoopOnButtonClick();  
    delay(50);  
  }
}

int StopLoopOnButtonClick(){
  if (digitalRead(ButtonPin) == LOW){ // Button pressed 
    return 0;
  } 
  else{ //Button released  
    stopped = 1;
    Serial.println("disc");
    delay(1000);
    Serial.end();
    Blink(3);
    return 1;
  }
}

void Blink(int n){
  for (int i = 0; i < n; i++)  {
    digitalWrite(led, HIGH);
    delay(100);    
    digitalWrite(led, LOW);
    delay(100);          
  }
}

