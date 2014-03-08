using System.Xml;
using System.Xml.Linq;
using GalileoDriver;
using NLog;

namespace GalileoDriver.RaspberryPi2
{
    public class RaspberryPi2 : Device
    {
        private new readonly Logger log = LogManager.GetCurrentClassLogger();

        public override DeviceType DeviceType
        {
            get { return DeviceType.Motherboard; }
        }

        internal override void Initialize(XElement configuration)
        {
            log.Info("RaspberryPi2 initializing");
            if (configuration == null)
            {
                log.Error("Empty device configuration.");
                return;
            }
            log.Debug("Configuration: \n{0}", configuration.ToString());
            ApplyConfiguration(configuration);
        }
        
        //<RaspberryPi2 IsEnable="true">
        //    <Wireless/>
        //    <RemoteControl/>
        //    <Devices/>
        //</RaspberryPi2>
        /// <summary>
        /// Configuration device
        /// </summary>
        /// <param name="config">device configuration</param>
        private void ApplyConfiguration(XElement config)
        {
            if (config == null) return;
            try
            {

                Name = config.Attribute(DeviceConstant.NameA) != null ? config.Attribute(DeviceConstant.NameA).Value : string.Empty;

                // Enable device
                IsEnable = false;
                if (config.Attribute(DeviceConstant.IsEnableA) != null)
                {
                    bool isEnable;
                    bool.TryParse(config.Attribute(DeviceConstant.IsEnableA).Value, out isEnable);
                    IsEnable = isEnable;
                }

                //Clear and add new children
                foreach (var device in Devices)
                {
                    device.Dispose();
                }
                Devices.Clear();

                if (config.Attribute(DeviceConstant.DevicesE) != null)
                {
                    foreach (var chieldConfig in config.Element(DeviceConstant.DevicesE).Elements())
                    {
                        var chield = DeviceBuilder.CreateDevice(chieldConfig);
                        if (chield == null) continue;
                        this.Devices.Add(chield);
                    }
                }

                log.Info("Device {0} initialized", Name);
            }
            catch (XmlException e)
            {
                log.Error("Error in Device configuration section\n{0}", e);
            }
        }
    }
}
