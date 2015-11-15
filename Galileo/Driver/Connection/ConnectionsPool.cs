namespace GalileoDriver
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using NLog;

    internal class ConnectionsPool
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, I2CConnection> connections = new Dictionary<string, I2CConnection>();

        public void Initialize(XElement configuration)
        {
            log.Trace("ConnectionPool initialization.");
            log.Trace(configuration);

            if (configuration == null) return;

            foreach (var connectionSetting in configuration.Elements(DriverConfigurationConstant.ConfigurationElementName))
            {
                var connection = I2CConnection.Create(connectionSetting);
                if(!connections.ContainsKey(connection.Name))
                    connections.Add(connection.Name, connection);
            }
        }
        
        public I2CConnection GetConnection(string connectionName)
        {
            log.Trace("GetConnection {0}", connectionName);

            if (!this.connections.ContainsKey(connectionName))
            {
                this.log.Warn("Connection '{0}' not exist.");
                return null;
            }

            return this.connections[connectionName];
        }
    }
}
