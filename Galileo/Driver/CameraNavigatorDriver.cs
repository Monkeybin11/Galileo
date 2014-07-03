namespace GalileoDriver
{
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;

    public class CameraNavigatorDriver : Driver
    {
        public CameraNavigatorDriver()
        {
            log.Info("Camera motion driver");
        }

        public override void Initialize(XElement configuration, UnityContainer container)
        {
            log.Info("Initialization");
        }
    }
}
