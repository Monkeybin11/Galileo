namespace GalileoDriver
{
    /// <summary>
    /// Describe connection protocol
    /// </summary>
    internal interface IConnection
    {
        bool IsConnected { get; }
        ConnectionProtocolType ConnectionProtocol { get; }
    }
}
