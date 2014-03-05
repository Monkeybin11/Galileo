using System;
using System.ServiceModel;
using GalileoDriver;
using NLog;

namespace Galileo.Connection
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Galileo : IGalileo, IDisposable
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly Driver driver = new Driver();
 
        public bool Start()
        {
            log.Info("Start method invoked");
            driver.Initialize();
            return true;
        }

        public bool Stop(bool immeadeatly)
        {
            log.Info("Stop method invoked");
            return true;
        }

        public bool Shutdown()
        {
            log.Info("Shutdown method invoked");
            return false;
        }

        public bool Restart()
        {
            log.Info("Restart method invoked");
            return false;
        }

        public void ApplyFirmware()
        {
            log.Info("Apply Firmware invoked");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                driver.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
