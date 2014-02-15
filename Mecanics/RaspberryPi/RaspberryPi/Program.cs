using System;
using RPi.I2C.Net;

namespace RaspberryPi
{
    class Program
    {
//        private const string I2CBusPath = "/dev/i2c-1";
//        private const int BusAdress = 0x40;

        PwmServoDriver pwm = new PwmServoDriver();

        const int SERVOMIN = 150; // this is the 'minimum' pulse length count (out of 4096)
        const int SERVOMAX = 600; // this is the 'maximum' pulse length count (out of 4096)

        byte servonum = 0;

        void setup()
        {
            Console.WriteLine("16 channel Servo test!");
            pwm.begin();
            pwm.setPWMFreq(60);  // Analog servos run at ~60 Hz updates
        }

        // you can use this function if you'd like to set the pulse length in seconds
        // e.g. setServoPulse(0, 0.001) is a ~1 millisecond pulse width. its not precise!
        void setServoPulse(byte n, double pulse)
        {
            double pulselength;
            pulselength = 1000000;   // 1,000,000 us per second
            pulselength /= 60;   // 60 Hz
            
            Console.Write(pulselength); Console.WriteLine(" us per period");

            pulselength /= 4096;  // 12 bits of resolution
            Console.Write(pulselength); Console.WriteLine(" us per bit");
            pulse *= 1000;
            pulse /= pulselength;
            Console.WriteLine(pulse);
            pwm.setPWM(n, (char)0, (char)pulse);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n\nStarting...");
            var p = new Program();
            p.setup();
            Console.WriteLine("Setuped");
            p.setServoPulse(5, 0.001);
            Console.WriteLine("Done.");

//            using (var bus = I2CBus.Open(I2CBusPath))
//            {
//                Console.WriteLine("Will start _ ");	
//                bus.WriteByte(BusAdress, 255);
//                Console.WriteLine("Done.");
//                Console.Read();
//            }



        }
    }
}
