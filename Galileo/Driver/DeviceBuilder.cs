using System;
using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    /// <summary>
    /// Create Device according to the xml configuration
    /// </summary>
    internal abstract class DeviceBuilder
    {
        /// <summary>
        /// Factory method for device creation and initialization
        /// </summary>
        /// <param name="configuration">XML donfiguration of device</param>
        /// <returns>Described Device in the XML of null</returns>
        public static Device CreateDevice(XElement configuration)
        {
            var log = LogManager.GetCurrentClassLogger();
            if (configuration == null)
            {
                log.Error("Null device configuration section.");
                return null;
            }
            var name = configuration.Name.LocalName;

            DeviceName deviceName;

            if (!Enum.TryParse(name, true, out deviceName))
            {
                log.Error("Can't parse configuration of {0}", name);
                return null;
            }

            Device result;

            switch (deviceName)
            {
                case DeviceName.RaspberryPi2:
                    result = new RaspberryPi2.RaspberryPi2();
                    break;
                case DeviceName.ArduinoMicro:
                    result = new ArduinoMicro.ArduinoMicro();
                    break;
                default:
                    log.Error("{0} initialization not implemented.", deviceName);
                    return null;
                    break;
            }
            result.Initialize(configuration);
            return result;
        }
    }
}
