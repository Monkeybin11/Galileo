using System.Xml.Linq;

namespace GalileoDriver
{
    using System.Deployment.Internal;

    internal interface IConfigured
    {
        void Initialize(XElement configuration);
    }
}
