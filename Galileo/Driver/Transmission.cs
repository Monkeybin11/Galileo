using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

namespace GalileoDriver
{
    public class Transmission : Driver
    {
        private const string VersionAttributeName = "version";
        private const string ConnectionElementName = @"Connection";
        private const string ConnectionNameAttributeName = @"name";
        private const string ConnectionPortAttributeName = @"port";
        
        private I2CConnection connection;

        private bool IsConnected
        {
            get
            {
                return connection.IsConnected;
            }
        }
        
        public override void Initialize(XElement configuration, UnityContainer container)
        {
            log.Trace("Transmission driver initialization");
            if (configuration == null)
            {
                log.Error("Empty Transmission configuration.");
                throw new InvalidDataException("Empty Transmission configuration.");
            }
            var versionAttribute = configuration.Attribute(VersionAttributeName);

            if (versionAttribute == null && !Version.TryParse(versionAttribute.Value, out version))
            {
                Version = new Version(0, 0, 0, 0);
                log.Warn("Can't loas Transmsission version. Default {0} was be set", Version);
            }

            var connectionElement = configuration.Element(ConnectionElementName);
            if (connectionElement == null)
            {
                log.Warn("Transmission configuration doesn't contains Connection elements");
            }
            else
            {
                var connectionName = connectionElement.Attribute(ConnectionNameAttributeName).Value;
                var connectionPort = connectionElement.Attribute(ConnectionPortAttributeName).Value;
                byte port;
                if (!byte.TryParse(connectionPort, out port))
                {
                    log.Error("Invalid connection port;");
                }

                var bus = container.Resolve<II2CBus>(connectionName);
                
                connection = new I2CConnection(bus, port);
            }
        }

        public void Move(float lineSpeed, float angularSpeed)
        {
            log.Trace("Move line speed: {0}, angulat speed: {1}", lineSpeed, angularSpeed);
            if (IsConnected)
            {
                connection.Send(new byte[1] { 1 });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection = null;
                Version = null;
            }

            GC.SuppressFinalize(this);
        }


    }
}
