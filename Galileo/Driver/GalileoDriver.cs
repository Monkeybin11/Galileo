using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GalileoDriver.Exceptions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using NLog;

namespace GalileoDriver
{
    public class GalileoDriver : IDisposable
    {
        private readonly UnityContainer container = new UnityContainer();

        private const string ConfigurationElement = @"Configuration";
        private const string NameAttribute = @"name";
        private const string defaultConfigFileName = @"DriverConfiguration.xml";
        private const string defaultConfigSectionName = @"Main";

        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public void Initialize()
        {
            Initialize(defaultConfigFileName, defaultConfigSectionName);
        }

        public void Initialize(string configSection)
        {
            Initialize(defaultConfigFileName, configSection);
        }

        public void Initialize(string configFileName, string configSection)
        {
            log.Info("Strat Galileo Driver initialization");
            log.Debug("ConfigFile - {0}", configFileName);
            log.Debug("ConfigSection - {0}.", configSection);
            
//            log.Info("I2C test");
//            int address = 0x2a;
//            I2CBus bus = I2CBus.Open(@"/dev/i2c-1");
//            log.Info("Opened");
//            Thread.Sleep(10);
//            log.Info("1");
//            bus.WriteByte(address, 1);
//            Thread.Sleep(10);
//            log.Info("2");
//            bus.WriteBytes(address, new byte[] { 1, 2, 3 });
//            Thread.Sleep(10);
//            log.Info("3");

            if (!ReadConfiguration(configFileName, configSection))
            {
                throw new InvalidDriverConfiguration("Can't load driver configurations. Application should be stopped.");
            }

            log.Info("Galileo Driver initialized");
        }

        private bool ReadConfiguration(string configFileName, string configSection)
        {
            try
            {
                XDocument xDoc = XDocument.Load(configFileName);
                XElement configuration;
                var configElements = xDoc.Root.Elements(ConfigurationElement);

                if (!configElements.Any())
                {
                    log.Error("Driver Configurations not found");
                    return false;
                }

                if (string.IsNullOrEmpty(configSection))
                {
                    configSection = defaultConfigSectionName;
                }

                configuration = configElements.SingleOrDefault(e => e.Attribute(NameAttribute) != null &&
                                                                               e.Attribute(NameAttribute).Value == configSection);
                
                if(configuration == default(XElement))
                {
                    log.Info("Can't find configuration with name - '{0}'. First configuration will be loaded", configSection);
                    configuration = configElements.First();
                }

                if (configuration.Element(DriverConfigurationConstant.DriversElementName) != null)
                {
                    foreach (var driverElement in configuration.Element(DriverConfigurationConstant.DriversElementName).Elements())
                    {
                        var drivers = DriverBuilder.CreateDriver(driverElement);
                        container.RegisterInstance<Driver>(drivers.Name, drivers);
                    }
                }
            }
            catch (XmlException e)
            {
                log.Error("Can't parse xml configuration.", e);
                return false;
            }
            return true;
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var items = container.ResolveAll<Driver>();
                items.ForEach(i => i.Dispose());
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
