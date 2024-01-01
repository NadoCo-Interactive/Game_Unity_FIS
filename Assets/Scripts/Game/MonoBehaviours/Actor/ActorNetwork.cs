using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActorNetwork : NetworkBehaviour, IActorNetwork
{
    #region ActorMotor Variables
    private Actor _actor;
    public NetworkVariable<Vector3> Position { get; set; } = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> Heading { get; set; } = new NetworkVariable<Vector3>(Vector3.forward);
    #endregion

    private bool initialized = false;

    void Start()
    {
        verifyInitialize();
    }

    void verifyInitialize()
    {
        if(initialized)
            return;

        _actor = GetComponent<Actor>().Required();

        if (!IsOwner)
        {
            _actor.name = "Player "+OwnerClientId;
            _actor.MakeRemote();
        }
        else {
            _actor.name = "Local Player";
        }

        initialized = true;
    }

    #region ActorMotor Events
    [ServerRpc]
    public void SetPositionServerRpc(Vector3 position)
    {
        Position.Value = position;
    }

    [ServerRpc]
    public void SetHeadingServerRpc(Vector3 aim)
    {
        Heading.Value = aim;
    }
    #endregion

    #region ActorInventory Events
    [ServerRpc]
    public void AddItemServerRpc(ItemType itemType, string itemId, string inventoryId)
    {
        if(IsOwner) return;
        verifyInitialize();

        var item = ItemManager.CreateItem(itemType,itemId);
        _actor.Inventory.AddItem(item);
    }

    [ServerRpc]
    public void RemoveItemServerRpc(string itemId)
    {
        if(IsOwner) return;
        verifyInitialize();

        var item = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);

        if(item != null)
            _actor.Inventory.RemoveItem(item);
    }

    [ServerRpc]
    public void TransferItemToServerRpc(string itemId, string toInventoryId)
    {
        if(IsOwner) return;
        verifyInitialize();

        var item = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);
        var toInventory = FindObjectsOfType<ActorInventory>().FirstOrDefault(inv => inv.Id == toInventoryId);

        if(item != null && toInventory != null)
            _actor.Inventory.TransferItemTo(item,toInventory);
    }

    [ServerRpc]
    public void AddFittingServerRpc(ItemType ItemType, string itemId, string hardpointId)
    {
        if(IsOwner) return;
        Debug.Log("received fitting packet");
        verifyInitialize();

        var weapon = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);
        var hardpoint = _actor.Weapon.Hardpoints.FirstOrDefault(hp => hp.Id == hardpointId);

        if(hardpoint == null || weapon == null)
            return;

        _actor.Inventory.AddFitting(weapon,hardpoint);
    }

    [ServerRpc]
    public void RemoveFittingServerRpc(string itemId)
    {
        if(IsOwner) return;
        verifyInitialize();

        var weapon = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);

        if(weapon == null)
            return;

        _actor.Inventory.RemoveFitting(weapon);
    }

    
    #endregion
}
