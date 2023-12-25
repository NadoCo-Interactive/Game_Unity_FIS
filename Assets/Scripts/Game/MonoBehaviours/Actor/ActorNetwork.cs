using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActorNetwork : NetworkBehaviour, IActorNetwork
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

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
        if(Vector3.Distance(transform.position,Position.Value) > 4)
            transform.position = Position.Value;
        
        Debug.Log(Vector3.Dot(transform.position,Position.Value));

        if(IsOwner)
        {
            SetRemotePositionServerRpc(gameObject.transform.position);
            SetRemoteRotationServerRpc(gameObject.transform.rotation);
        }
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
