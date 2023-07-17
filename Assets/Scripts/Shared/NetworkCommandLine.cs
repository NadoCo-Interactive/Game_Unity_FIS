using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum ClientMode
{
    Client,
    Server,
    Host
}
public class NetworkCommandLine : StrictBehaviour
{
    protected ClientMode clientMode;
    private NetworkManager netManager;

    protected virtual void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();

        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    netManager.StartServer();
                    clientMode = ClientMode.Server;
                    break;
                case "host":
                    netManager.StartHost();
                    clientMode = ClientMode.Host;
                    break;
                default: //client
                    netManager.StartClient();
                    clientMode = ClientMode.Client;
                    break;
            }
        }
    }

    protected Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}