using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class CommandLineUtils : StrictBehaviour
{
    public static ServerMode GetServerModeFromCLI()
    {
        if (Application.isEditor) return ServerMode.Client;

        var args = GetCommandlineArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    return ServerMode.Server;
                case "host":
                    return ServerMode.Host;
                default: //client
                    return ServerMode.Client;
            }
        }
        else
            return ServerMode.Client;
    }

    public static Dictionary<string, string> GetCommandlineArgs()
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