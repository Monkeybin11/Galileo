namespace RaspberryPi.Connection
{
    public class Galileo : IGalileo
    {
        public bool Start()
        {
            return true;
        }

        public bool Stop(bool immeadeatly)
        {
            return true;
        }

        public bool Shutdown()
        {
            return false;
        }

        public bool Restart()
        {
            return false;
        }
    }
}
