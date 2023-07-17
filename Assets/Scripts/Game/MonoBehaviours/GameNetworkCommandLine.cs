using UnityEngine;

public class GameNetworkCommandLine : NetworkCommandLine
{
    protected override void Start()
    {
        base.Start();

        var cameraOrbit = Camera.main.GetRequiredComponent<OrbitCam>();
        var cameraPan = Camera.main.GetRequiredComponent<PanCam>();

        if (clientMode == ClientMode.Server)
        {
            cameraOrbit.enabled = false;
            cameraPan.enabled = true;
        }
    }
}
