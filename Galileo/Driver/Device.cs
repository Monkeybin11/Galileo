using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    public abstract class Device : IDisposable
    {
        protected Logger log = LogManager.GetCurrentClassLogger();
        public string Name { get; protected set; }
        public abstract DeviceType DeviceType { get; }
        public bool IsEnable { get; protected set; }
        protected List<Device> Devices = new List<Device>();

        internal abstract void Initialize(XElement configuration);

        #region IDisposal

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsEnable = false;

                foreach (var device in Devices)
                {
                    device.Dispose();
                }
                Devices.Clear();
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}