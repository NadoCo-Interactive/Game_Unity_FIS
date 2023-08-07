using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNetworkCommandLine : NetworkCommandLine
{

    protected override void Start()
    {
        var args = GetCommandlineArgs();
        var hud = GameObject.Find("HUD").Required();
        var hudMainMenu = hud.transform.FindRequired("MainMenu");
        var hudServerMode = hud.transform.FindRequired("ServerMode");

        if (args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    hudServerMode.gameObject.SetActive(true);
                    break;
                case "client":
                    hudMainMenu.gameObject.SetActive(true);
                    break;
            }
        }
        else
            hudMainMenu.gameObject.SetActive(true);

        base.Start();
    }
}
