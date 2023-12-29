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
    public NetworkVariable<Vector3> Heading { get; set; } = new NetworkVariable<Vector3>();
    #endregion

    #region ActorInventory Variables
    public NetworkVariable<List<IItem>> Items { get; set; } = new NetworkVariable<List<IItem>>();
    #endregion

    #region ActorWeapon Variables
    public NetworkVariable<IWeaponItem> ActiveWeapon { get;set; } = new NetworkVariable<IWeaponItem>();
    public NetworkVariable<List<ActorHardpoint>> Hardpoints { get;set; } = new NetworkVariable<List<ActorHardpoint>>();
    #endregion

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
        var item = ItemManager.CreateItem(itemType);
        _actor.Inventory.AddItem(item,itemId);
    }

    [ServerRpc]
    public void RemoveItemServerRpc(string itemId)
    {
        var item = Items.Value.FirstOrDefault(i => i.Id == itemId);

        if(item != null)
            _actor.Inventory.RemoveItem(item);
    }

    [ServerRpc]
    public void TransferItemToServerRpc(string itemId, string toInventoryId)
    {
        var item = Items.Value.FirstOrDefault(i => i.Id == itemId);
        var toInventory = FindObjectsOfType<ActorInventory>().FirstOrDefault(inv => inv.Id == toInventoryId);

        if(item != null && toInventory != null)
            _actor.Inventory.TransferItemTo(item,toInventory);
    }

    [ServerRpc]
    public void AddFittingServerRpc(WeaponType weaponType, string itemId, string hardpointId)
    {
        var weapon = Items.Value.FirstOrDefault(i => i.Id == itemId);
        var hardpoint = Hardpoints.Value.FirstOrDefault(hp => hp.Id == hardpointId);

        if(hardpoint == null || weapon == null)
            return;

        _actor.Inventory.AddFitting(weapon,hardpoint);
    }

    [ServerRpc]
    public void RemoveFittingServerRpc(string itemId)
    {
        var weapon = Items.Value.FirstOrDefault(i => i.Id == itemId);

        if(weapon == null)
            return;

        _actor.Inventory.RemoveFitting(weapon);
    }

    
    #endregion
}
