﻿using System;
using System.Linq;
using System.Xml.Linq;
using GalileoDriver;
using NLog;

namespace GalileoDriver
{
    public class Driver : IDisposable
    {
        private const string DriverConfigurationElement = @"DriverConfigurations";
        private const string ConfigurationElement = @"Configuration";
        private const string NameAttribute = @"name";

        private const string defaultConfigFileName = @"DriverConfiguration.xml";
        private const string defaultConfigSectionName = @"Main";

        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public void Initialize()
        {
            Initialize("");
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

            XDocument xDoc = XDocument.Load(configFileName);
            XElement configuration;
            var configElements = xDoc.Root.Elements(ConfigurationElement);

            if (!configElements.Any())
            {
                log.Error("Driver Configurations not found");
            }

            if (string.IsNullOrEmpty(configSection))
            {
                configSection = defaultConfigSectionName;
            }

            if (configElements.Any(c => c.Name.LocalName == configSection))
            {
                configuration = configElements.First(c => c.Name.LocalName == configSection);
            }
            else
            {
                log.Info("Default configuration will be loaded");
                configuration = configElements.First();
            }

            if (configuration.Element(DeviceConstant.DevicesE) != null)
            {
                foreach (var deviceConfig in configuration.Element(DeviceConstant.DevicesE).Elements())
                {
                    var device = DeviceBuilder.CreateDevice(deviceConfig);
                }
            }

            log.Info("Galileo Driver initialized");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
            GC.SuppressFinalize(this);
        }
    }
}
