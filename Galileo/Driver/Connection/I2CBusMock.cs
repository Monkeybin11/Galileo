using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalileoDriver
{
    using System.Xml.Linq;

    using NLog;

    internal class I2CBusMock : II2CBus
    {
        protected Logger log = LogManager.GetCurrentClassLogger();

        public void Initialize(XElement configuration)
        {
            log.Info("Initalize Mock I2C Bus");
        }

        public void Dispose()
        {
            
        }

        public void Open(string busPath)
        {
            log.Info("Open I2C Bus");
        }

        public void Finalyze()
        {
            
        }

        public void WriteByte(int address, byte b)
        {
            log.Trace("Write one byte '{0}' to address {1}", b, address);
        }

        public void WriteBytes(int address, byte[] bytes)
        {
            log.Trace("Write {0} bytes to address {1}", bytes.Length, address);
        }

        public byte[] ReadBytes(int address, int count)
        {
            log.Warn("Try to read bytes from I2C Bus");
            return new byte[0];
        }
    }
}
