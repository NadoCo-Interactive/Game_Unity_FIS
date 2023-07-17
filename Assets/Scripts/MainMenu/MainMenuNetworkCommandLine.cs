using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuNetworkCommandLine : MonoBehaviour
{
    void Start()
    {
        if (CommandLineUtils.GetServerModeFromCLI() == ServerMode.Server)
            SceneManager.LoadScene("Game");
    }
}
