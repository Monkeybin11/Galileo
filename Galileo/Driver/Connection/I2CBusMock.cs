namespace GalileoDriver
{
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;

    using NLog;

    internal class I2CBusMock : II2CBus
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public string BusPath { get; private set; }

        public void Initialize(XElement configuration, UnityContainer container)
        {
            log.Info("Initalize Mock I2C Bus");
            BusPath = string.Empty;
        }

        public void Dispose()
        {   
        }

        public void Open(string busPath)
        {
            log.Info("Open I2C Bus {0}", busPath);
            BusPath = busPath;
        }

        public void Finalyze()
        {
        }

        public void WriteByte(int address, byte b)
        {
            log.Trace("Write one byte '{0}' to address {1}, ButhPath {2}", b, address, BusPath);
        }

        public void WriteBytes(int address, byte[] bytes)
        {
            log.Trace("Write {0} bytes to address {1}, Bus path {2}", bytes.Length, address, BusPath);
        }

        public byte[] ReadBytes(int address, int count)
        {
            log.Warn("Try to read bytes from I2C Bus");
            return new byte[0];
        }
    }
}
