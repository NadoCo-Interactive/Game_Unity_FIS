using UnityEngine.SceneManagement;

public class MainMenuNetworkCommandLine : NetworkCommandLine
{
    protected override void Start()
    {
        base.Start();

        if (clientMode == ClientMode.Server)
            SceneManager.LoadScene("Game");
    }
}
