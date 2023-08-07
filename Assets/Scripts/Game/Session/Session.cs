using UnityEngine;

public class Session : StrictBehaviour
{
    private OrbitCam _camera;

    protected virtual void Start()
    {
        _camera = GameObject.Find("Main Camera").Required().GetRequiredComponent<OrbitCam>();

        var actor = ActorManager.SpawnPlayerActor(Vector3.forward * 50);
        actor.transform.forward = -actor.transform.forward;

        RespawnPlayer();
    }

    protected virtual void Update()
    {

    }

    public void RespawnPlayer()
    {
        var player = ActorManager.SpawnPlayer(Vector3.zero);
        _camera.TrackedObject = player.transform;
    }
}