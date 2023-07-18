using UnityEngine;

public class Session : StrictBehaviour
{
    protected virtual void Start()
    {
        var cameraOrbit = Camera.main.GetRequiredComponent<OrbitCam>();
        var cameraPan = Camera.main.GetRequiredComponent<PanCam>();

        if (FISNetworkManager.ServerMode == ServerMode.Server)
        {
            cameraOrbit.enabled = false;
            cameraPan.enabled = true;
        }
        else
        {
            var actor = ActorManager.SpawnPlayerActor(Vector3.forward * 50);
            actor.transform.forward = -actor.transform.forward;

            // RespawnPlayer();
        }
    }

    protected virtual void Update()
    {

    }

    public void RespawnPlayer()
    {
        var cameraOrbit = Camera.main.GetRequiredComponent<OrbitCam>();
        var player = ActorManager.SpawnPlayer(Vector3.zero);
        cameraOrbit.TrackedObject = player.transform;
    }
}