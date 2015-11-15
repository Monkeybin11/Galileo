using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GalileoDriver.Exceptions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NLog;

namespace GalileoDriver
{
    public class GalileoDriver : IDisposable
    {
        private const string ConfigurationElement = @"Configuration";
        private const string NameAttribute = @"name";
        private const string DefaultConfigFileName = @"DriverConfiguration.xml";
        private const string DefaultConfigSectionName = @"Main";
        
        private readonly UnityContainer container;
        private ConnectionsPool connectionsPool = new ConnectionsPool();

        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public GalileoDriver()
        {
            container = new UnityContainer();
            container.LoadConfiguration();
            this.Initialize();
        }

        public void Initialize()
        {
            Initialize(DefaultConfigFileName, DefaultConfigSectionName);
        }

        public void Initialize(string configSection)
        {
            Initialize(DefaultConfigFileName, configSection);
        }

        public void Initialize(string configFileName, string configSection)
        {
            log.Info("Strat Galileo Driver initialization");
            log.Debug("ConfigFile - {0}", configFileName);
            log.Debug("ConfigSection - {0}.", configSection);
            
            if (!ReadConfiguration(configFileName, configSection))
            {
                throw new InvalidDriverConfiguration("Can't load driver configurations. Application should be stopped.");
            }

            log.Info("Galileo Driver initialized");
        }

        public void Move(float linearSpeed, float angularSpeed)
        {
            var transmission = (Transmission)container.Resolve<Driver>("Transmission");
            if (transmission == null)
            {
                log.Error("Transmission not resolved");
                return;
            }

            transmission.Move(linearSpeed, angularSpeed);
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

        private bool ReadConfiguration(string configFileName, string configSection)
        {
            try
            {
                var xDoc = XDocument.Load(configFileName);

                var configElements = xDoc.Root.Elements(ConfigurationElement);

                if (!configElements.Any())
                {
                    log.Error("Driver Configurations not found");
                    return false;
                }

                if (string.IsNullOrEmpty(configSection))
                {
                    configSection = DefaultConfigSectionName;
                }

                XElement configuration = configElements.SingleOrDefault(e => e.Attribute(NameAttribute) != null &&
                                                                             e.Attribute(NameAttribute).Value == configSection);

                if (configuration == default(XElement))
                {
                    log.Info("Can't find configuration with name - '{0}'. First configuration will be loaded", configSection);
                    configuration = configElements.First();
                }

                InitializeConnectionsPool(configuration);
                InitializeConfiguredItems(configuration, DriverConfigurationConstant.DriversElementName, typeof(Driver));

            }
            catch (XmlException e)
            {
                log.Error("Can't parse xml configuration.", e);
                return false;
            }
            return true;
        }

        private void InitializeConnectionsPool(XElement configuration)
        {
            connectionsPool = new ConnectionsPool();
            if (configuration.Element(DriverConfigurationConstant.ConnectionSectionName) == null)
            {
                log.Error("No connections in the current configuration");
                return;
            }
            
            connectionsPool.Initialize(configuration.Element(DriverConfigurationConstant.ConnectionSectionName)); 
        }

        private void InitializeConfiguredItems(XElement configuration, string collectionName, Type itemType)
        {
            if (configuration.Element(collectionName) == null)
            {
                return;
            }

            foreach (var element in configuration.Element(collectionName).Elements())
            {
                var itemName = element.Attribute(NameAttribute);

                var name = itemName != null ? itemName.Value : element.Name.LocalName;

                log.Info("Start resolving '{0}'", name);
                if (string.IsNullOrEmpty(name))
                {
                    log.Error("Driver name not retrived. \n{0}", element.ToString());
                }

                var result = (IConfigurable)container.Resolve(itemType, name);
                result.Initialize(element, container);
            }
        }
    }
}
