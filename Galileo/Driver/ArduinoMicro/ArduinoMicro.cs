using System.Xml.Linq;
using GalileoDriver;
using NLog;

namespace GalileoDriver.ArduinoMicro
{
    public class ArduinoMicro : Device
    {
        private new Logger log = LogManager.GetCurrentClassLogger();
        public override DeviceType DeviceType { get { return DeviceType.ExtentionBoard; } }
        
        internal override void Initialize(XElement configuration)
        {
            log.Error("Initialize method in ArduinoMicro not implemented");
        }
    }
}
