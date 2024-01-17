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
    public NetworkList<ItemDTO> Fittings { get; set; } = new NetworkList<ItemDTO>();

    private bool initialized = false;

    void Start()
    {
        verifyInitialize();
        _actor.VerifyInitialize();
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

    private bool verifyPacketRelevance(ulong clientId)
    {
        if(clientId != OwnerClientId)
            return false;

        if(IsOwner)
            return false;

        return true;
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

    
    [ClientRpc]
    private void addItemClientRpc(ItemType itemType, ulong itemId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            return;
        }
        GameLog.Log("["+_actor.name+"] received addItem packet for "+clientId);

        try
        {
            var item = ItemManager.CreateItem(itemType,itemId);
            _actor.Inventory.AddItem(item);
        }
        catch(Exception ex)
        {
            GameLog.Log("["+_actor.name+"] [ERR] "+ex.Message);
        }
    }

    [ServerRpc]
    public void AddItemServerRpc(ItemType itemType, ulong itemId)
    {
        // addItemClientRpc(itemType,itemId,OwnerClientId);
    }

    [ClientRpc]
    private void removeItemClientRpc(ulong itemId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            return;
        }
        GameLog.Log("["+_actor.name+"] received removeItem packet for "+_actor.name);

        var item = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);

        if(item != null)
            _actor.Inventory.RemoveItem(item);
    }

    [ServerRpc]
    public void RemoveItemServerRpc(ulong itemId)
    {
        removeItemClientRpc(itemId,OwnerClientId);
    }

    [ClientRpc]
    private void transferItemToClientRpc(ulong itemId, string toInventoryId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            return;
        }

        var item = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);
        var toInventory = FindObjectsOfType<ActorInventory>().FirstOrDefault(inv => inv.Id == toInventoryId);

        if (item != null && toInventory != null)
            _actor.Inventory.TransferItemTo(item,toInventory);
    }
    [ServerRpc]
    public void TransferItemServerRpc(ulong itemId, string toInventoryId)
    {
        transferItemToClientRpc(itemId,toInventoryId,OwnerClientId);
    }

    [ClientRpc]
    private void addFittingClientRpc(ItemDTO itemDto, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
            return;

        GameLog.Log("["+_actor.name+"] received fitting packet with hardpoint id "+itemDto.HardpointId);

        var weapon = ItemManager.CreateItem(itemDto.ItemType, itemDto.Id);

        ActorHardpoint hardpoint = _actor.Weapon.Hardpoints.FirstOrDefault(hp => hp.Id == itemDto.HardpointId);

        if (hardpoint == null)
        {
            var hardpointIndex = HardpointIds.IndexOf(itemDto.HardpointId);

            if (hardpointIndex != -1)
            {
                GameLog.LogWarning("[" + _actor.name + "] Found hardpoint by index " + hardpointIndex);
                hardpoint = _actor.Weapon.Hardpoints[hardpointIndex];
                hardpoint.Id = itemDto.HardpointId;
            }
        }

        if(hardpoint == null)
        {
            GameLog.LogWarning("[" + _actor.name + "] Can't find hardpoint " + itemDto.HardpointId);
            return;
        }

        _actor.Inventory.AddFitting(weapon,hardpoint);
    }

    [ServerRpc]
    public void AddFittingServerRpc(ItemDTO itemDto)
    {
        addFittingClientRpc(itemDto,OwnerClientId);

        var fittingIndex = _actor.Inventory.Fittings.FindIndex(w => w.Id == itemDto.Id);
        Fittings.Insert(fittingIndex,itemDto);
    }

    [ClientRpc]
    private void removeFittingClientRpc(ulong itemId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            GameLog.Log("["+_actor.name+"] ignored removeFitting packet for "+_actor.name);
            return;
        }
        GameLog.Log("["+_actor.name+"] received unfitting packet on "+_actor.name);

        var weapon = _actor.Inventory.Fittings.FirstOrDefault(i => i.Id == itemId);

        weapon.Required("["+_actor.name+"] The requested fitting " + itemId + " doesn't exist in call to removeFittingClientRpc");

        _actor.Inventory.RemoveFitting(weapon);
    }

    [ServerRpc]
    public void RemoveFittingServerRpc(ulong itemId)
    {
        verifyInitialize();
        removeFittingClientRpc(itemId, OwnerClientId);

        var fittingIndex = _actor.Inventory.Fittings.FindIndex(w => w.Id == itemId);
        Fittings.RemoveAt(fittingIndex);
    }

    
    #endregion

    #region ActorWeapon Events
    [ClientRpc]
    public void setHardpointIdsClientRpc(ulong[] hardpointIds,ulong clientId)
    {
        verifyInitialize();

        HardpointIds = new NetworkList<ulong>(hardpointIds);

        if(!verifyPacketRelevance(clientId))
        {
            return;
        }
        GameLog.Log("["+_actor.name+"] received hardpoint setting packet on "+_actor.name);

        foreach(ulong id in HardpointIds)
        {
            var hardpoint = _actor.Weapon.Hardpoints.ElementAt(HardpointIds.IndexOf(id));
            hardpoint.Id = id;
            GameLog.Log(" - new hardpoint id: "+id);
        }
    }
    [ServerRpc]
    public void SetHardpointIdsServerRpc(ulong[] hardpointIds)
    {
        setHardpointIdsClientRpc(hardpointIds,OwnerClientId);
    }
    #endregion
}
