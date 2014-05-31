using System;
using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    /// <summary>
    /// Create Driver according to the xml configuration
    /// </summary>
    internal abstract class DriverBuilder
    {
        /// <summary>
        /// Factory method for driver creation and initialization
        /// </summary>
        /// <param name="configuration">XML donfiguration of device</param>
        /// <returns>Described Device in the XML of null</returns>
        public static Driver CreateDriver(XElement configuration)
        {
            var log = LogManager.GetCurrentClassLogger();
            if (configuration == null)
            {
                log.Error("Null driver configuration section.");
                return null;
            }
            var name = configuration.Name.LocalName;

            DriverType type;

            if (!Enum.TryParse(name, true, out type))
            {
                log.Error("Can't parse configuration of {0}", name);
                return null;
            }

            Driver result;

            switch (type)
            {
                case DriverType.Transmission:
                    result = new Transmission(configuration);
                    break;
                
                default:
                    log.Error("{0} initialization not implemented.", type);
                    return null;
                    break;
            }
            result.Initialize(configuration);
            return result;
        }
    }
}
