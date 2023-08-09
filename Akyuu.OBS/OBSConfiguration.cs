namespace Akyuu.OBS;

public record OBSConfiguration
{
    /// <summary>
    /// Configuration for connecting to an OBS WebSocket.
    /// </summary>
    /// <param name="host">The hostname/IP of the OBS WebSocket instance.</param>
    /// <param name="password">The password for the WebSocket instance.</param>
    /// <param name="port">The port of the WebSocket instance.</param>
    /// <param name="reconnectTimeout">Reconnection timeout for the WebSocket, in milliseconds.</param>
    public OBSConfiguration(string host, string? password = null, ushort port = 4455, uint? reconnectTimeout = 30000)
    {
        Host = host;
        Password = password;
        Port = port;
        ReconnectTimeout = reconnectTimeout == null ? null : TimeSpan.FromMilliseconds(reconnectTimeout.Value);
    }

    public string Host { get; }
    public string? Password { get; }
    public ushort Port { get; }
    public TimeSpan? ReconnectTimeout { get; }
    public Uri Uri => new($"ws://{Host}:{Port}");
}