using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    void Start()
    {
        verifyInitialize();
    }

    void verifyInitialize()
    {
        if (initialized) return;

        _networkManager = GetRequiredComponent<NetworkManager>();

        var serverMode = CommandLineUtils.GetServerModeFromCLI();

        if (serverMode == ServerMode.Server)
            _networkManager.StartServer();
        else if (serverMode == ServerMode.Host)
            _networkManager.StartHost();
        else
            _networkManager.StartClient();

        Debug.Log("serverMode=" + serverMode);

        initialized = true;
    }


}
