using System;
using System.Linq;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;

public enum ServerMode
{
    Client,
    Server,
    Host
}

[CustomEditor(typeof(FISNetworkManager))]
public class FISNetworkManager : Editor
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

    void OnEnable()
    {
        verifyInitialize();
    }

    void OnInspectorGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Start Host CUSTOM"))
            connectHost();
    }

    void verifyInitialize()
    {
        if (initialized) return;

        _networkManager = GetRequiredComponent<NetworkManager>();
        _networkTransport = GetRequiredComponent<UnityTransport>();

        initialized = true;
    }

    void doConnection()
    {
        var serverMode = CommandLineUtils.GetServerModeFromCLI();

        _networkManager.StartClient();
        _networkManager.StartHost();
        return;

        ServerConnection.SetStatus(ConnectionStatus.Connecting, "Connecting...");
        Debug.Log("attempting to connect as " + serverMode);

        if (serverMode == ServerMode.Server)
        {
            _networkManager.StartServer();
            ServerConnection.SetStatus(ConnectionStatus.Connected, "Connected as Server");
        }
        else if (serverMode == ServerMode.Host)
            _networkManager.StartHost();
        else
        {
            var connectionData = _networkTransport.ConnectionData;
            var addrPort = connectionData.Address + ":" + connectionData.Port;

            try
            {
                Debug.Log("connecting to " + connectionData.Address + " on " + connectionData.Port);
                _networkManager.StartClient();

                if (!_networkManager.IsConnectedClient)
                {
                    _networkManager.StartHost();
                    serverMode = ServerMode.Host;
                    ServerConnection.SetStatus(ConnectionStatus.Connected, "Connected as Host");
                } 
                else
                {
                    ServerConnection.SetStatus(ConnectionStatus.Connected, "Connected as Client");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                _networkManager.StartHost();
                serverMode = ServerMode.Host;
                ServerConnection.SetStatus(ConnectionStatus.Connected, "Connected as Host");
                Debug.Log("server is not running or failed to connect, starting as host");
            }

            var localPlayer = GetLocalPlayer();
            var orbitCam = Camera.main.GetRequiredComponent<OrbitCam>();
            //orbitCam.TrackedObject = localPlayer.gameObject.transform;

        }

        Toast.Show("serverMode=" + serverMode);
    }

    void connectHost()
    {
        _networkManager.StartHost();
        var localPlayer = GetLocalPlayer();
        var orbitCam = Camera.main.GetRequiredComponent<OrbitCam>();
        orbitCam.TrackedObject = localPlayer.gameObject.transform;
    }

    public static NetworkObject GetLocalPlayer()
    {
        var networkObjects = FindObjectsOfType<NetworkObject>();
        var playerObject = networkObjects.FirstOrDefault(o => o.IsLocalPlayer);

        return playerObject;
    }


}
