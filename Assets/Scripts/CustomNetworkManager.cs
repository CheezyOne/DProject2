using Mirror;
using System;

public class CustomNetworkManager : NetworkManager
{
    public event Action OnClientConnectedEvent;
    public event Action OnClientDisconnectedEvent;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnectedEvent?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnectedEvent?.Invoke();
    }
}