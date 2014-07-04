using System;
using System.ServiceModel;
using NLog;

namespace Galileo.Connection
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Galileo : IGalileo, IDisposable
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly GalileoDriver.GalileoDriver galileoDriver = new GalileoDriver.GalileoDriver();

        public bool Start()
        {
            log.Info("Start method invoked");
            galileoDriver.Initialize();
            return true;
        }

        public void Move()
        {
            galileoDriver.Move(0, 0);
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
                galileoDriver.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
