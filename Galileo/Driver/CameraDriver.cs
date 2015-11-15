namespace GalileoDriver
{
    using System;
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;

    internal class CameraDriver : Driver
    {
        public CameraDriver()
        {
            this.Log.Info("CameraDriver ctor");
        }

        public override void Initialize(XElement configuration, UnityContainer container)
        {
            this.Log.Trace("Camera driver start initialization");

            this.Log.Trace("Camera driver initialized");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //connection = null;
                Version = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
