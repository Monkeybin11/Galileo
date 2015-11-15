namespace GalileoDriver
{
    using System;

    internal interface II2CBus : IConfigurable, IDisposable
    {
        string BusPath { get; }

        void Open(string busPath);
        
        void WriteByte(int address, byte b);

        void WriteBytes(int address, byte[] bytes);

        byte[] ReadBytes(int address, int count);
    }
}