using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RPi.I2C.Net;

namespace RaspberryPi
{
    internal class PwmServoDriver
    {

        private const string I2CBusPath = "/dev/i2c-1";
        private const int BusAdress = 0x40;
        private I2CBus bus;

        private const byte PCA9685_SUBADR1 = 0x2;
        private const byte PCA9685_SUBADR2 = 0x3;
        private const byte PCA9685_SUBADR3 = 0x4;
        private const byte PCA9685_MODE1 = 0x0;
        private const byte PCA9685_PRESCALE = 0xFE;
        private const byte LED0_ON_L = 0x6;
        private const byte LED0_ON_H = 0x7;
        private const byte LED0_OFF_L = 0x8;
        private const byte LED0_OFF_H = 0x9;
        private const byte ALLLED_ON_L = 0xFA;
        private const byte ALLLED_ON_H = 0xFB;
        private const byte ALLLED_OFF_L = 0xFC;
        private const byte ALLLED_OFF_H = 0xFD;


        public void begin()
        {
            Console.WriteLine("[Begin] Starting");
            bus = new I2CBus(I2CBusPath);
            Console.WriteLine("[Begin] Bus created");
            reset();
            Console.WriteLine("[Begin] reseted");
        }

        private void reset()
        {
            Console.WriteLine("[Reset] starting");
            write8(PCA9685_MODE1, 0x0);
            Console.WriteLine("[Reset] Done");
        }

        public void setPWMFreq(float freq)
        {
            //Serial.print("Attempting to set freq ");
            //Console.WriteLine(freq);

            float prescaleval = 25000000;
            prescaleval /= 4096;
            prescaleval /= freq;
            prescaleval -= 1;

            Console.Write("Estimated pre-scale: {0}", prescaleval);

            byte prescale = (byte)Math.Truncate(prescaleval + 0.5);

            Console.Write("Final pre-scale: {0}", prescale);

            byte oldmode = read8(PCA9685_MODE1);
            byte newmode = (byte)((oldmode & 0x7F) | 0x10); // sleep
            write8(PCA9685_MODE1, newmode); // go to sleep
            write8(PCA9685_PRESCALE, prescale); // set the prescaler
            write8(PCA9685_MODE1, oldmode);
            Thread.Sleep(5);
            write8(PCA9685_MODE1, (byte)(oldmode | 0xa1)); //  This sets the MODE1 register to turn on auto increment.
            // This is why the beginTransmission below was not working.
            // Console.Write("Mode now 0x"); Console.WriteLine(read8(PCA9685_MODE1), HEX);
        }

        public void setPWM(byte num, Char on, Char off)
        {
            //Serial.print("Setting PWM ");Console.Write(num);Console.Write(": ");Console.Write(on);Console.Write("->"); Console.WriteLine(off);

            //  WIRE.beginTransmission(_i2caddr);
            write(LED0_ON_L + 4 * num);
            write(on);
            write(on >> 8);
            write(off);
            write(off >> 8);
            //  WIRE.endTransmission();
        }

        private byte read8(byte addr)
        {
            //  WIRE.beginTransmission(_i2caddr);
            write(addr);
            //  WIRE.endTransmission();

            //  WIRE.requestFrom((byte)_i2caddr, (byte)1);
            return bus.ReadBytes(BusAdress, 1).First();
        }

        private void write8(byte addr, byte d)
        {
            Console.WriteLine("[Write 8] address {0}, byte {1}.");
            //  WIRE.beginTransmission(_i2caddr);
            write(addr);
            write(d);
            Console.WriteLine("[Write 8] done");
            //  WIRE.endTransmission();
        }

        private void write(int i)
        {
            Console.WriteLine("[Write] {0}", i);
            var bytes = BitConverter.GetBytes(i);
            bus.WriteBytes(BusAdress, bytes);
        }

        private void write(byte i)
        {
            Console.WriteLine("[Write] {0}", i);
            var bytes = BitConverter.GetBytes(i);
            bus.WriteBytes(BusAdress, bytes);
        }


        private void write(char i)
        {
            Console.WriteLine("[Write] {0}", i);
            var bytes = BitConverter.GetBytes(i);
            bus.WriteBytes(BusAdress, bytes);
        }
    }
}
