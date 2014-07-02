namespace GalileoDriver
{
    using System;

    internal interface II2CBus : IConfigured, IDisposable
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="busPath"></param>
        void Open(string busPath);

        void Finalyze();

        /// <summary>
        /// Writes single byte.
        /// </summary>
        /// <param name="address">Address of a destination device</param>
        /// <param name="b"></param>
        void WriteByte(int address, byte b);

        /// <summary>
        /// Writes array of bytes.
        /// </summary>
        /// <remarks>Do not write more than 3 bytes at once, RPi drivers don't support this currently.</remarks>
        /// <param name="address">Address of a destination device</param>
        /// <param name="bytes"></param>
        void WriteBytes(int address, byte[] bytes);

        /// <summary>
        /// Reads bytes from device with passed address.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        byte[] ReadBytes(int address, int count);
    }
}