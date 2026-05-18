using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private NetworkMessageService _messageService;
    [Inject] private CustomNetworkManager _networkManager;

    [SerializeField] private TMP_Text logText;
    [SerializeField] private Button subscribeButton;
    [SerializeField] private Button serverSendButton;

    private bool _isSubscribed;
    private bool _clientConnected;

    private void Awake()
    {
        subscribeButton.interactable = false;
        serverSendButton.interactable = false;
    }

    private void OnDestroy()
    {
        _networkManager.OnClientConnectedEvent -= OnClientConnect;
        _networkManager.OnClientDisconnectedEvent -= OnClientDisconnect;
    }

    public void OnStartServerButton()
    {
        _networkManager.OnClientConnectedEvent -= OnClientConnect;
        _networkManager.OnClientConnectedEvent += OnClientConnect;
        _networkManager.OnClientDisconnectedEvent -= OnClientDisconnect;
        _networkManager.OnClientDisconnectedEvent += OnClientDisconnect;
        _networkManager.StartServer();
        _messageService.Initialize();
        Log("Server started.");
        serverSendButton.interactable = true;
    }

    public void OnStartClientButton()
    {
        _networkManager.OnClientConnectedEvent -= OnClientConnect;
        _networkManager.OnClientConnectedEvent += OnClientConnect;
        _networkManager.StartClient();
        Log("Client starting...");
    }

    public void OnSubscribeButton()
    {
        if (!NetworkClient.isConnected)
        {
            Log("Cannot subscribe: not connected to server.");
            return;
        }

        if (_isSubscribed)
        {
            Log("Already subscribed to HelloMessage.");
            return;
        }

        Log("Subscribing to HelloMessage...");
        _messageService.Subscribe<HelloMessage>(OnHelloMessage);
        _isSubscribed = true;
    }

    public void OnSendMessageButton()
    {
        _messageService.SendToAll(new HelloMessage { Text = "Hello from server again!" });
        Log("Server sent message to all subscribed clients.");
    }

    private void OnClientConnect()
    {
        if (_clientConnected) 
            return;

        _clientConnected = true;
        Log("Client connected.");

        if (!_isSubscribed)
            subscribeButton.interactable = true;
    }

    private void OnClientDisconnect()
    {
        _clientConnected = false;
        _isSubscribed = false;
        Log("Client disconnected.");
        subscribeButton.interactable = false;
    }

    private void OnHelloMessage(HelloMessage message)
    {
        string msg = $"Received: {message.Text}";
        Log(msg);
    }

    private void Log(string message)
    {
        logText.text += $"{System.DateTime.Now:T} - {message}\n";
    }
}