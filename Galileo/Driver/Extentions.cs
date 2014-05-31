using System;
using GalileoDriver.Interfaces;
using Microsoft.Practices.Unity;

namespace GalileoDriver
{
    internal static class Extentions
    {
        public static void Register(this UnityContainer container, Device device)
        {
            Type type;
            switch (device.DeviceType)
            {
                case DeviceType.Motherboard:
                    type = typeof(IMotherboard);
                    break;
                case DeviceType.ExtentionBoard:
                    type = typeof(IExtentionBoard);
                    break;
                case DeviceType.Transmission:
                    type = typeof(ITransmission);
                    break;
                case DeviceType.Accelerometer:
                  type = typeof (IAccelerometer);
                  break;
                case DeviceType.Distometer:
                    type = typeof(IDistometer);
                    break;
                default:
                    throw new NotImplementedException("Add regiter for unknown device type");
                    break;
            }
            container.RegisterInstance(type, device);
        }
    }
}
