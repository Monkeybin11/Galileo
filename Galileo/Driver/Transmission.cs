using System.Xml.Linq;

namespace GalileoDriver
{
    public class Transmission : Driver
    {
        public Transmission()
        {
            log.Error("Transmission constructor not implemented.");
        }

        internal override void Initialize(XElement configuration)
        {
            log.Trace("Transmission driver initialization");
        }
    }
}
