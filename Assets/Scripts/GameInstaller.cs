using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CustomNetworkManager _networkManager;

    public override void InstallBindings()
    {
        Container.Bind<NetworkMessageService>().AsSingle();
        Container.Bind<CustomNetworkManager>().FromInstance(_networkManager).AsSingle();
    }
}