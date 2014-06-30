namespace GalileoDriver
{
    using System.Xml.Linq;

    public class CameraNavigatorDriver : Driver
    {
        public CameraNavigatorDriver()
        {
            log.Info("Camera motion driver");
        }

        public override void Initialize(XElement configuration)
        {
            log.Info("Initialization");
        }
    }
}
