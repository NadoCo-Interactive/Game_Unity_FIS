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

    public NetworkList<ulong> HardpointIds { get; set; } = new NetworkList<ulong>();

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
        Debug.Log("received addItem packet");
        verifyInitialize();

        var item = ItemManager.CreateItem(itemType,itemId);
        _actor.Inventory.AddItem(item);
    }

    [ServerRpc]
    public void RemoveItemServerRpc(string itemId)
    {
        if(IsOwner) return;
        Debug.Log("received removeItem packet");
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

        if (item != null && toInventory != null)
            _actor.Inventory.TransferItemTo(item,toInventory);
    }

    [ServerRpc]
    public void AddFittingServerRpc(ItemType ItemType, string itemId, string hardpointId)
    {
        if(IsOwner) return;
        Debug.Log("received fitting packet on "+_actor.name);
        verifyInitialize();

        var weapon = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);

        ActorHardpoint hardpoint = _actor.Weapon.Hardpoints.FirstOrDefault(hp => hp.Id.ToString() == hardpointId);

        if (weapon == null)
        {
            Debug.LogWarning("Can't find item " + itemId + " in call to AddFittingServerRpc, skipping");
            Debug.Log(" - available items: " + string.Concat("," + _actor.Inventory.Items.Select(i => i.Id).ToArray()));
            return;
        }

        if (hardpoint == null)
        {
            Debug.LogWarning("Can't find hardpoint " + hardpointId + " in call to AddFittingServerRpc, skipping");

            var hardpointsList = string.Join(",", _actor.Weapon.Hardpoints.Select(i => i.Id.ToString()).ToArray());
            Debug.Log(" - network hardpoints: " + hardpointsList);
            return;
        }

        _actor.Inventory.AddFitting(weapon,hardpoint);
    }

    [ServerRpc]
    public void RemoveFittingServerRpc(string itemId)
    {
        if(IsOwner) return;
        Debug.Log("received unfitting packet on " + _actor.name);
        verifyInitialize();

        var weapon = _actor.Inventory.Fittings.FirstOrDefault(i => i.Id == itemId);

        weapon.Required("The requested fitting " + itemId + " doesn't exist in call to RemoveFittingServerRpc");

        _actor.Inventory.RemoveFitting(weapon);
    }

    
    #endregion

    #region ActorWeapon Events
    [ServerRpc]
    public void SetHardpointIdsServerRpc(ulong[] hardpointIds)
    {
        if(IsOwner) return;
        Debug.Log("received hardpoint setting packet");

        verifyInitialize();

        HardpointIds = new NetworkList<ulong>(hardpointIds);

        foreach(ulong id in HardpointIds)
        {
            var hardpoint = _actor.Weapon.Hardpoints.ElementAt(HardpointIds.IndexOf(id));
            hardpoint.Id = id;
            Debug.Log("new hardpoint id: "+id);
        }
    }
    #endregion
}
