using System.Xml.Linq;

namespace GalileoDriver
{   
    using Microsoft.Practices.Unity;

    internal interface IConfigured
    {
        void Initialize(XElement configuration, UnityContainer container);
    }
}
