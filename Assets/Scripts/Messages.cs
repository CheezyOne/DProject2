using Mirror;

public struct HelloMessage : NetworkMessage
{
    public string Text;
}

public struct SubscriptionMessage : NetworkMessage
{
    public string MessageType; 
}