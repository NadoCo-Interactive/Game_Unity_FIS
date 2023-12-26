using UnityEngine;
using Unity.Netcode;
using System;

public class ActorNetwork : NetworkBehaviour, IActorNetwork
{
    private Actor _actor;
    public NetworkVariable<Vector3> Position { get; set; } = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> Aim { get; set; } = new NetworkVariable<Vector3>();

    void Start()
    {
        _actor = GetComponent<Actor>().Required();

        if (!IsOwner)
        {
            _actor.name = "Player "+OwnerClientId;
            _actor.MakeRemote();
        }
        else {
            _actor.name = "Local Player";
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

    /* [ServerRpc]
    public void AddFittingServerRpc(WeaponType weaponType, string hardpointId)
    {
        Debug.Log("ADDED A FITTING");
        var weapon = ItemManager.CreateWeapon(weaponType);
        _actor.Inventory.AddFitting(weapon,hardpointId);
    }

    [ServerRpc]
    public void RemoveFittingServerRpc(string itemId)
    {
        _actor.Inventory.RemoveFittingByItemId(itemId);
    } */
}
