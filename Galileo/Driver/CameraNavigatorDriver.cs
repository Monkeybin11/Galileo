namespace GalileoDriver
{
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;

    public class CameraNavigatorDriver : Driver
    {
        public CameraNavigatorDriver()
        {
            this.Log.Info("Camera motion driver");
        }

        public override void Initialize(XElement configuration, UnityContainer container)
        {
            this.Log.Info("Initialization");
        }
    }
}
