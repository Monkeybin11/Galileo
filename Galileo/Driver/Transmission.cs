using System.Xml.Linq;

namespace GalileoDriver
{
    public class Transmission : Driver
    {
        public Transmission()
        {
            log.Warn("Transmission constructor not implemented.");
        }

        public override void Initialize(XElement configuration)
        {
            log.Trace("Transmission driver initialization");
        }
    }
}
