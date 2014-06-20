namespace GalileoDriver
{
    using System.Xml.Linq;

    public class CameraNavigatorDriver : Driver
    {
        public CameraNavigatorDriver()
        {
            log.Info("Camera motion driver");
        }

        internal override void Initialize(XElement configuration)
        {
            log.Info("Initialization");
        }
    }
}
