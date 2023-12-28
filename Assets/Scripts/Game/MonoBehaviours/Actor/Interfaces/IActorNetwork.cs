using System;
using Unity.Netcode;
using UnityEngine;

public interface IActorNetwork
{
    public NetworkVariable<Vector3> Position { get; set; }
    public NetworkVariable<Vector3> Aim { get; set; }

    public void SetRemotePositionServerRpc(Vector3 position);
    public void SetRemoteAimServerRpc(Vector3 rotation);
    public void AddFittingServerRpc(WeaponType weaponType, int hardpointId);
    public void RemoveFittingServerRpc(string itemId);
}