using NLog;
using System.Threading;

namespace GalileoDriver
{
    /// <summary>
    /// I2C connection protocol.
    /// </summary>
    internal class I2CConnection 
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly II2CBus bus;

        private readonly byte port;

        public I2CConnection(II2CBus bus, byte port)
        {
            this.bus = bus;
            this.port = port;
        }

        public bool IsConnected
        {
            get
            {
                return true;
            }
        }

        public override string ToString()
        {
            return string.Format("I2C [Bus = {0}, Port = {1}]", bus.BusPath, port);
        }

        public void Send(byte[] data)
        {
            if (data == null)
            {
                log.Error("Null data for sending via {0}", this);
                return;
            }
            
            bus.WriteBytes(port, data);
            log.Trace("I2C sending {0} bytes", data.Length);
	    Thread.Sleep(100);
            var result = bus.ReadBytes(port, 1);
	    log.Trace("Received {0}", result[0]);
        }

        public ConnectionProtocolType ConnectionProtocol
        {
            get { return ConnectionProtocolType.I2C; }
        }
    }
}