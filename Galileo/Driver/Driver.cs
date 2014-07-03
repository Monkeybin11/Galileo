using System;
using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    using Microsoft.Practices.Unity;

    public class Driver : IConfigured, IDisposable
    {
        protected Logger log = LogManager.GetCurrentClassLogger();

        public string Name { get; protected set; }

        public Version Version { get; private set; }

        public bool IsEnable { get; protected set; }

        public virtual void Initialize(XElement configuration, UnityContainer container)
        {
            throw new NotImplementedException("Derived class does not have implementation");
        }

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