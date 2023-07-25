using System;
using System.Linq;
using System.Net.Sockets;
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

    private NetworkManager _networkManager;
    private UnityTransport _networkTransport;

    void Start()
    {
        verifyInitialize();
    }

    void verifyInitialize()
    {
        if (initialized) return;

        _networkManager = GetRequiredComponent<NetworkManager>();
        _networkTransport = GetRequiredComponent<UnityTransport>();

        DoConnection();

        initialized = true;
    }

    void DoConnection()
    {
        var serverMode = CommandLineUtils.GetServerModeFromCLI();

        if (serverMode == ServerMode.Server)
            _networkManager.StartServer();
        else if (serverMode == ServerMode.Host)
            _networkManager.StartHost();
        else
        {
            var connectionData = _networkTransport.ConnectionData;
            var addrPort = connectionData.Address + ":" + connectionData.Port;

            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    Debug.Log("connecting to " + connectionData.Address + " on " + connectionData.Port);
                    tcpClient.Connect(connectionData.Address, connectionData.Port);
                    _networkManager.StartClient();
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError(ex.Message);
                _networkManager.StartHost();
                serverMode = ServerMode.Host;
                Debug.Log("server is not running or failed to connect, starting as host");
            }

            var localPlayer = GetLocalPlayer();
            var orbitCam = Camera.main.GetRequiredComponent<OrbitCam>();
            orbitCam.TrackedObject = localPlayer.gameObject.transform;
        }

        Debug.Log("serverMode=" + serverMode);
    }

    public static NetworkObject GetLocalPlayer()
    {
        var networkObjects = FindObjectsOfType<NetworkObject>();
        var playerObject = networkObjects.FirstOrDefault(o => o.IsLocalPlayer);

        return playerObject;
    }


}
