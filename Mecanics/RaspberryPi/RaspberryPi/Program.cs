using System;
using RPi.I2C.Net;

namespace RaspberryPi
{
    class Program
    {
        private const string I2CBusPath = @"/dev/i2c-1";
        private const int BusAdress = 40;

        static void Main(string[] args)
        {
            using (var bus = I2CBus.Open(I2CBusPath))
            {
                Console.WriteLine("Will start");
                bus.WriteByte(BusAdress, 1);
                Console.WriteLine("Done.");
                Console.Read();
            }
        }
    }
}
