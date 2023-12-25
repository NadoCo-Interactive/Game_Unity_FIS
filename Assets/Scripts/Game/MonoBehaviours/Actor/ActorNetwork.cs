using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActorNetwork : NetworkBehaviour, IActorNetwork
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> Aim = new NetworkVariable<Vector3>();

    private float syncTimer = 0;

    void Start()
    {
        var actor = GetComponent<Actor>().Required();

        if (!IsOwner)
        {
            actor.name = "Player "+OwnerClientId;
            actor.MakeRemote();
        }
        else {
            actor.name = "Local Player";
        }
    }
    void Update()
    {
        if(IsOwner)
        {
            SetRemotePositionServerRpc(gameObject.transform.position);
            SetRemoteAimServerRpc(gameObject.transform.forward);
        }
        else
        {
            transform.position = Position.Value;
            transform.forward = Aim.Value;
        }
    }

    [ServerRpc]
    public void SetRemotePositionServerRpc(Vector3 position)
    {
        Position.Value = position;
    }

    [ServerRpc]
    public void SetRemoteAimServerRpc(Vector3 aim)
    {
        Aim.Value = aim;
    }
}
