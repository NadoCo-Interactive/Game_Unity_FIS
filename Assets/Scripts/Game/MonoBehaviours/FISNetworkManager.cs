using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public enum ServerMode
{
    Client,
    Server,
    Host
}

public class FISNetworkManager : Singleton<FISNetworkManager>
{
    private static ServerMode _serverMode;
    public static ServerMode ServerMode
    {
        get
        {
            Instance.verifyInitialize();
            return _serverMode;
        }
        private set
        {
            _serverMode = value;
        }
    }
    private bool initialized = false;

    public bool IsHost = false;

    private NetworkManager _networkManager;

    void Start()
    {
        verifyInitialize();
    }

    void verifyInitialize()
    {
        if (initialized) return;

        _networkManager = GetRequiredComponent<NetworkManager>();

        StartConnection();

        initialized = true;
    }

    

    void StartConnection()
    {
        // _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        _networkManager.OnClientConnectedCallback += OnClientConnected;

        // .. TODO: Eventually, I'll need to make this attempt a client connection
        // first, and then automatically start as host if the connection fails
        
        if(IsHost)
            _networkManager.StartHost();
        else
            _networkManager.StartClient();
    }

    private void OnClientConnected(ulong obj)
    {
        var localPlayer = GetLocalPlayer();
        var orbitCam = Camera.main.GetRequiredComponent<OrbitCam>();
        orbitCam.TrackedObject = localPlayer.gameObject.transform;


    }

    private void OnClientDisconnected(ulong obj)
    {
        if(obj == 0)
        {
            _networkManager.StartHost();
        }
    } 

    public static NetworkObject GetLocalPlayer()
    {
        var networkObjects = FindObjectsOfType<NetworkObject>();
        var playerObject = networkObjects.FirstOrDefault(o => o.IsLocalPlayer);

        return playerObject;
    }


}
