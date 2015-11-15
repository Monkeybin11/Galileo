using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    using System;
    using System.Xml;

    /// <summary>
    /// I2C connection protocol.
    /// </summary>
    internal class I2CConnection
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly II2CBus bus;

        private readonly byte port;

        public static I2CConnection Create(XElement configuration)
        {
            Log.Trace("Create connection.");

            try
            {
                var typeName = configuration.Attribute(DriverConfigurationConstant.ConnectionTypeAttribute).Value;
                var type = (ConnectionProtocolType)Enum.Parse(typeof(ConnectionProtocolType), typeName, true);
                switch (type)
                {
                    case ConnectionProtocolType.I2C:
                        return CreateI2cConnection(configuration);
                        break;
                    default:
                        return null;
                }
            }
            catch (XmlException)
            {
                return null;
            }

            return null;
        }

        private static I2CConnection CreateI2cConnection(XElement configuration)
        {
            throw new NotImplementedException();
        }

        public string Name { get; private set; }

        private I2CConnection(II2CBus bus, byte port)
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
                Log.Error("Null data for sending via {0}", this);
                return;
            }

            bus.WriteBytes(port, data);
            Log.Trace("I2C sending {0} bytes", data.Length);
        }
    }
}