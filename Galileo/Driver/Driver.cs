using System;
using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    public abstract class Driver : IDisposable
    {
        protected Logger log = LogManager.GetCurrentClassLogger();
        public string Name { get; protected set; }
        public Version Version { get; private set; }
        public bool IsEnable { get; protected set; }

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
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}