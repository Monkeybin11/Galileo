using System;
using System.IO;
using System.Runtime.InteropServices;
using Mono.Unix;
using Mono.Unix.Native;
using System.Xml.Linq;
using Microsoft.Practices.Unity;
using NLog;



namespace GalileoDriver
{
    internal static class I2CNativeLib
    {
        [DllImport("libnativei2c.so", EntryPoint = "openBus", SetLastError = true)]
        public static extern int OpenBus(string busFileName);

        [DllImport("libnativei2c.so", EntryPoint = "closeBus", SetLastError = true)]
        public static extern int CloseBus(int busHandle);

        [DllImport("libnativei2c.so", EntryPoint = "readBytes", SetLastError = true)]
        public static extern int ReadBytes(int busHandle, int addr, byte[] buf, int len);

        [DllImport("libnativei2c.so", EntryPoint = "writeBytes", SetLastError = true)]
        public static extern int WriteBytes(int busHandle, int addr, byte[] buf, int len);
    }

    internal class I2CBus : II2CBus
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private int busHandle;

        public string BusPath { get; private set; }

        public void Initialize(XElement configuration, UnityContainer container)
        {
            log.Trace("I2CBus initialization.");
            if (configuration == null)
            {
                log.Error("Invalid configuration.");
                throw new NullReferenceException();
            }

            var addressAttribute = configuration.Attribute("address");
            if (addressAttribute == null)
            {
                log.Error("Can't parse address configuration");
                throw new InvalidDataException();
            }
            BusPath = addressAttribute.Value;
            log.Trace("I2C address = {0}", BusPath);
            Open(BusPath);
        }

        public void Open(string busPath)
        {
            BusPath = busPath;
            int res = I2CNativeLib.OpenBus(busPath);
            if (res < 0)
            {
                throw new IOException( String.Format("Error opening bus '{0}': {1}", busPath, UnixMarshal.GetErrorDescription(Stdlib.GetLastError())));
            }

            busHandle = res;
        }

        public void WriteByte(int address, byte b)
        {
            var bytes = new byte[1];
            bytes[0] = b;
            WriteBytes(address, bytes);
        }
        
        public void WriteBytes(int address, byte[] bytes)
        {
            var res = I2CNativeLib.WriteBytes(busHandle, address, bytes, bytes.Length);
            if (res == -1)
            {
                string message = String.Format(
                    "Error accessing address '{0}': {1}",
                    address,
                    UnixMarshal.GetErrorDescription(Stdlib.GetLastError()));

                log.Error(message);
                throw new IOException(message);
            }

            if (res == -2)
            {
                var message = String.Format("Error writing to address '{0}': I2C transaction failed", address);
                log.Error(message);
                throw new IOException(message);
            }
        }

        public byte[] ReadBytes(int address, int count)
        {
            var buf = new byte[count];
            var res = I2CNativeLib.ReadBytes(busHandle, address, buf, buf.Length);
            if (res == -1)
            {
                var message = String.Format(
                    "Error accessing address '{0}': {1}",
                    address,
                    UnixMarshal.GetErrorDescription(Stdlib.GetLastError()));
                log.Error(message);
                throw new IOException(message);
            }

            if (res <= 0)
            {
                var message = String.Format("Error reading from address '{0}': I2C transaction failed", address);
                log.Error(message);
                throw new IOException(message);
            }

            if (res < count)
            {
                Array.Resize(ref buf, res);
            }

            return buf;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
	    log.Trace("Disposing");
            if (disposing)
            {
                // disposing managed resouces
            }

            if (busHandle != 0)
            {
                I2CNativeLib.CloseBus(busHandle);
                busHandle = 0;
            }
        }
    }
} 