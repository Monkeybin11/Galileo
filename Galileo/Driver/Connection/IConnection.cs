namespace GalileoDriver
{
    /// <summary>
    /// Describe connection protocol.
    /// </summary>
    internal interface IConnection : IConfigured
    {
        bool IsConnected { get; }

        ConnectionProtocolType ConnectionProtocol { get; }
    }
}
