using System.Xml.Linq;

namespace GalileoDriver
{   
    using Microsoft.Practices.Unity;

    internal interface IConfigurable
    {
        void Initialize(XElement configuration, UnityContainer container);
    }
}
