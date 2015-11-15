//using System.Xml;
//using System.Xml.Linq;
//using GalileoDriver;
//using NLog;
//
//namespace GalileoDriver.RaspberryPi2
//{
//    public class RaspberryPi2 : Driver
//    {
//        private new readonly Logger log = LogManager.GetCurrentClassLogger();
//
//        public override DeviceType DeviceType
//        {
//            get { return DeviceType.Motherboard; }
//        }
//
//        internal override void Initialize(XElement configuration)
//        {
//            log.Info("RaspberryPi2 initializing");
//            if (configuration == null)
//            {
//                log.Error("Empty device configuration.");
//                return;
//            }
//            log.Debug("Configuration: \n{0}", configuration.ToString());
//            ApplyConfiguration(configuration);
//        }
//        
//        //<RaspberryPi2 IsEnable="true">
//        //    <Wireless/>
//        //    <RemoteControl/>
//        //    <Devices/>
//        //</RaspberryPi2>
//        /// <summary>
//        /// Configuration device
//        /// </summary>
//        /// <param name="config">device configuration</param>
//        private void ApplyConfiguration(XElement config)
//        {
//            if (config == null) return;
//            try
//            {
//
//                Name = config.Attribute(DriverConfigurationConstant.NameAttribute) != null ? config.Attribute(DriverConfigurationConstant.NameAttribute).Value : string.Empty;
//
//                // Enable device
//                IsEnable = false;
//                if (config.Attribute(DriverConfigurationConstant.IsEnableA) != null)
//                {
//                    bool isEnable;
//                    bool.TryParse(config.Attribute(DriverConfigurationConstant.IsEnableA).Value, out isEnable);
//                    IsEnable = isEnable;
//                }
//
//                //Clear and add new children
//                foreach (var device in Devices)
//                {
//                    device.Dispose();
//                }
//                Devices.Clear();
//
//                if (config.Attribute(DriverConfigurationConstant.DriversElementName) != null)
//                {
//                    foreach (var chieldConfig in config.Element(DriverConfigurationConstant.DriversElementName).Elements())
//                    {
//                        var chield = DriverBuilder.CreateDriver(chieldConfig);
//                        if (chield == null) continue;
//                        this.Devices.Add(chield);
//                    }
//                }
//
//                log.Info("Device {0} initialized", Name);
//            }
//            catch (XmlException e)
//            {
//                log.Error("Error in Device configuration section\n{0}", e);
//            }
//        }
//    }
//}
