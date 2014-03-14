﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Mono.Unix;
using Mono.Unix.Native;

namespace GalileoDriver.RaspberryPi2
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

    class I2CBus : IDisposable
    {        
            private int busHandle;

            /// <summary>
            /// .ctor
            /// </summary>
            /// <param name="busPath"></param>
            protected I2CBus(string busPath)
            {
                int res = I2CNativeLib.OpenBus(busPath);
                if (res < 0)
                    throw new IOException(String.Format("Error opening bus '{0}': {1}", busPath, UnixMarshal.GetErrorDescription(Stdlib.GetLastError())));

                busHandle = res;
            }


            /// <summary>
            /// Creates new instance of I2CBus.
            /// </summary>
            /// <param name="busPath">Path to system file associated with I2C bus.<br/>
            /// For RPi rev.1 it's usually "/dev/i2c-0",<br/>
            /// For rev.2 it's "/dev/i2c-1".</param>
            /// <returns></returns>
            public static I2CBus Open(string busPath)
            {
                return new I2CBus(busPath);
            }



            public void Finalyze()
            {
                Dispose(false);
            }



            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }



            protected virtual void Dispose(bool disposing)
            {
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


            /// <summary>
            /// Writes single byte.
            /// </summary>
            /// <param name="address">Address of a destination device</param>
            /// <param name="b"></param>
            public void WriteByte(int address, byte b)
            {
                var bytes = new byte[1];
                bytes[0] = b;
                WriteBytes(address, bytes);
            }


            /// <summary>
            /// Writes array of bytes.
            /// </summary>
            /// <remarks>Do not write more than 3 bytes at once, RPi drivers don't support this currently.</remarks>
            /// <param name="address">Address of a destination device</param>
            /// <param name="bytes"></param>
            public void WriteBytes(int address, byte[] bytes)
            {
                var res = I2CNativeLib.WriteBytes(busHandle, address, bytes, bytes.Length);
                if (res == -1)
                    throw new IOException(String.Format("Error accessing address '{0}': {1}", address, UnixMarshal.GetErrorDescription(Stdlib.GetLastError())));
                if (res == -2)
                    throw new IOException(String.Format("Error writing to address '{0}': I2C transaction failed", address));
            }
        
            /// <summary>
            /// Reads bytes from device with passed address.
            /// </summary>
            /// <param name="address"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public byte[] ReadBytes(int address, int count)
            {
                var buf = new byte[count];
                var res = I2CNativeLib.ReadBytes(busHandle, address, buf, buf.Length);
                if (res == -1)
                    throw new IOException(String.Format("Error accessing address '{0}': {1}", address, UnixMarshal.GetErrorDescription(Stdlib.GetLastError())));
                if (res <= 0)
                    throw new IOException(String.Format("Error reading from address '{0}': I2C transaction failed", address));

                if (res < count)
                    Array.Resize(ref buf, res);

                return buf;
            }

    }
}
