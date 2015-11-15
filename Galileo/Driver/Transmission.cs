using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

namespace GalileoDriver
{
    public class Transmission : Driver
    {
        
        
        private I2CConnection connection;

        private bool IsConnected
        {
            get
            {
                return connection.IsConnected;
            }
        }
        
        public override void Initialize(XElement configuration, UnityContainer container)
        {
            base.Initialize(configuration, container);
        }

        public void Move(float lineSpeed, float angularSpeed)
        {
            this.Log.Trace("Move line speed: {0}, angulat speed: {1}", lineSpeed, angularSpeed);
            if (IsConnected)
            {
                connection.Send(new byte[] 
                { 
                    15, 2, 
                    0, 1, (byte)Math.Round(lineSpeed), 
                    1, 1, (byte)Math.Round(lineSpeed), 
                    2, 1, (byte)Math.Round(lineSpeed), 
                    3, 1, (byte)Math.Round(lineSpeed), 
                    200 
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection = null;
                Version = null;
            }

            GC.SuppressFinalize(this);
        }


    }
}