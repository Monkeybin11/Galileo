using System;

namespace GalileoDriver.Exceptions
{
    public class InvalidDriverConfiguration : Exception
    {
        public InvalidDriverConfiguration(){}

        public InvalidDriverConfiguration(string message): base(message) {}
    }
}
