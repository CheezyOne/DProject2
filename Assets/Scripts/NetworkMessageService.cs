using System;
using System.Collections.Generic;
using Mirror;

public class NetworkMessageService
{
    private Dictionary<string, HashSet<NetworkConnectionToClient>> _subscriptions = new();
    private bool _serverInitialized;

    public void Initialize()
    {
        if (_serverInitialized || !NetworkServer.active)
            return;

        NetworkServer.RegisterHandler<SubscriptionMessage>(OnSubscriptionMessage);
        _serverInitialized = true;
    }

    private void OnSubscriptionMessage(NetworkConnectionToClient conn, SubscriptionMessage msg)
    {
        if (!_subscriptions.TryGetValue(msg.MessageType, out HashSet<NetworkConnectionToClient> connections))
        {
            connections = new HashSet<NetworkConnectionToClient>();
            _subscriptions[msg.MessageType] = connections;
        }

        connections.Add(conn);

        if (msg.MessageType == nameof(HelloMessage))
            conn.Send(new HelloMessage { Text = "Hello Client!" });
    }

    public void Subscribe<T>(Action<T> handler) where T : struct, NetworkMessage
    {
        NetworkClient.RegisterHandler(handler);
        NetworkClient.Send(new SubscriptionMessage { MessageType = typeof(T).Name });
    }

    public void SendToAll<T>(T message) where T : struct, NetworkMessage
    {
        if (_subscriptions.TryGetValue(typeof(T).Name, out HashSet<NetworkConnectionToClient> connections))
        {
            foreach (NetworkConnectionToClient connection in connections)
            {
                connection.Send(message);
            }
        }
    }
}