using System;
using System.Xml.Linq;
using NLog;

namespace GalileoDriver
{
    using System.IO;

    using Microsoft.Practices.Unity;

    public class Driver : IConfigurable, IDisposable
    {
        private const string VersionAttributeName = "version";
        private const string ConnectionElementName = @"Connection";
        private const string ConnectionNameAttributeName = @"name";
        private const string ConnectionPortAttributeName = @"port";

        protected Logger Log = LogManager.GetCurrentClassLogger();

        protected Version version;

        public string Name { get; protected set; }

        public Version Version
        {
            get
            {
                return this.version;
            }
            protected set
            {
                this.version = value;
            }
        }

        public bool IsEnable { get; protected set; }

        public virtual void Initialize(XElement configuration, UnityContainer container)
        {
            this.Log.Trace("Driver initialization");
            if (configuration == null)
            {
                this.Log.Error("Empty configuration.");
                throw new InvalidDataException("Empty configuration.");
            }
            LoadVersion(configuration);

            SetConnection(configuration);
        }

        private void SetConnection(XElement configuration)
        {
            Log.Trace("Parsing connection");
            if (configuration == null)
            {
                return;
            }
            Log.Trace(configuration);

        }

        private void LoadVersion(XElement configuration)
        {
            version = new Version(0, 0, 0, 0);
            var versionAttribute = configuration.Attribute(VersionAttributeName);
            if (versionAttribute != null || Version.TryParse(versionAttribute.Value, out this.version))
            {
                return;
            }

            this.Log.Warn("Can't loas Transmsission version. Default {0} was be set", this.Version);
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