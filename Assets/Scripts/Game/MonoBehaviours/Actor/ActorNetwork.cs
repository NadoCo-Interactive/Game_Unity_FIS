using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActorNetwork : NetworkBehaviour, IActorNetwork
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

    void Start()
    {
        var actor = GetComponent<Actor>().Required();

        if (!IsOwner)
            actor.MakeRemote();
    }
    void Update()
    {
        transform.position = Position.Value;
        transform.rotation = Rotation.Value;
    }

    [ServerRpc]
    public void SetRemotePositionServerRpc(Vector3 position)
    {
        Position.Value = position;
    }

    [ServerRpc]
    public void SetRemoteRotationServerRpc(Quaternion rotation)
    {
        Rotation.Value = rotation;
    }
}
