using NLog;

namespace GalileoDriver
{
    using System.Xml.Linq;

    /// <summary>
    /// I2C connection protocol
    /// </summary>
    public class I2CConnection : IConnection
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Number of SDA connector
        /// </summary>
        public int Sda { get; private set; }

        /// <summary>
        /// Number of SCL connector
        /// </summary>
        public int Scl { get; private set; }

        /// <summary>
        /// Device Address
        /// </summary>
        public int Address { get; private set; }

        public I2CConnection()
        {
            
        }

        /// <summary>
        /// Return short description of connection
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("I2C [SDA={0}, SLC={1}, Address={2}]", Sda, Scl, Address);
        }

        public void Initialize(XElement configuration)
        {
            log.Warn("Method not implemented");
            //            log.Trace("Constructing I2C [SDA={0}, SLC={1}, Address={2}]", sda, scl, address);
            //            Sda = sda;
            //            Scl = scl;
            //            Address = address;
        }

        /// <summary>
        /// Send bytes
        /// </summary>
        /// <param name="data">Bytes array</param>
        public void Send(byte[] data)
        {
            if (data == null)
            {
                log.Error("Null data for sending via {0}", this);
                return;
            }
            log.Trace("I2C sending {0} bytes", data.Length);
        }

        /// <summary>
        /// Return connection state
        /// </summary>
        public bool IsConnected
        {
            get
            {
                log.Error("Not implemented.");
                return false;
            }
        }

        public ConnectionProtocolType ConnectionProtocol
        {
            get { return ConnectionProtocolType.I2C; }
        }
    }
}