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
    public NetworkList<ItemDTO> Items { get; set; } = new NetworkList<ItemDTO>();


    private bool initialized = false;



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
            GameLog.Log("ignored addItem packet for "+clientId);
            return;
        }
        GameLog.Log("received addItem packet for "+clientId);

        try
        {
            var item = ItemManager.CreateItem(itemType,itemId);
            _actor.Inventory.AddItem(item);
        }
        catch(Exception ex)
        {
            GameLog.Log("[ERR] "+ex.Message);
        }
    }

    [ServerRpc]
    public void AddItemServerRpc(ItemType itemType, ulong itemId)
    {
        Items.Add(new ItemDTO() { ItemType = itemType, Id = itemId });
        addItemClientRpc(itemType,itemId,OwnerClientId);
    }

    [ClientRpc]
    private void removeItemClientRpc(ulong itemId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            GameLog.Log("ignored removeItem packet for "+_actor.name);
            return;
        }
        GameLog.Log("received removeItem packet for "+_actor.name);

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

        GameLog.Log("received transferItem packet on "+_actor.name);
        if(!verifyPacketRelevance(clientId))
        {
            GameLog.Log("ignored packet for "+_actor.name);
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
    private void addFittingClientRpc(ulong itemId, ulong hardpointId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            GameLog.Log("ignored addFitting packet for "+_actor.name);
            return;
        }
        GameLog.Log("received fitting packet for "+_actor.name);
        
        var weapon = _actor.Inventory.Items.FirstOrDefault(i => i.Id == itemId);

        ActorHardpoint hardpoint = _actor.Weapon.Hardpoints.FirstOrDefault(hp => hp.Id == hardpointId);

        if (weapon == null)
        {
            GameLog.LogWarning("Can't find item " + itemId + " in call to AddFittingServerRpc, skipping");
            GameLog.Log(" - available items: " + string.Concat("," + _actor.Inventory.Items.Select(i => i.Id).ToArray()));
            _actor.SetTailColor(Color.yellow);
            return;
        }

        if (hardpoint == null)
        {
            GameLog.LogWarning("Can't find hardpoint " + hardpointId + " in call to AddFittingServerRpc, skipping");

            var hardpointsList = string.Join(",", _actor.Weapon.Hardpoints.Select(i => i.Id.ToString()).ToArray());
            GameLog.Log(" - network hardpoints: " + hardpointsList);
            _actor.SetTailColor(Color.magenta);
            return;
        }

        _actor.Inventory.AddFitting(weapon,hardpoint);
    }

    [ServerRpc]
    public void AddFittingServerRpc(ulong itemId, ulong hardpointId)
    {
        addFittingClientRpc(itemId,hardpointId,OwnerClientId);
    }

    [ClientRpc]
    private void removeFittingClientRpc(ulong itemId, ulong clientId)
    {
        verifyInitialize();

        if(!verifyPacketRelevance(clientId))
        {
            GameLog.Log("ignored removeFitting packet for "+_actor.name);
            return;
        }
        GameLog.Log("received unfitting packet on "+_actor.name);

        var weapon = _actor.Inventory.Fittings.FirstOrDefault(i => i.Id == itemId);

        weapon.Required("The requested fitting " + itemId + " doesn't exist in call to removeFittingClientRpc");

        _actor.Inventory.RemoveFitting(weapon);
    }
    [ServerRpc]
    public void RemoveFittingServerRpc(ulong itemId)
    {
        removeFittingClientRpc(itemId,OwnerClientId);
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
            GameLog.Log("ignored hardpoint setting packet for "+_actor.name);
            return;
        }
        GameLog.Log("received hardpoint setting packet on "+_actor.name);

        foreach(ulong id in HardpointIds)
        {
            var hardpoint = _actor.Weapon.Hardpoints.ElementAt(HardpointIds.IndexOf(id));
            hardpoint.Id = id;
            GameLog.Log("new hardpoint id: "+id);
        }
    }
    [ServerRpc]
    public void SetHardpointIdsServerRpc(ulong[] hardpointIds)
    {
        setHardpointIdsClientRpc(hardpointIds,OwnerClientId);
    }
    #endregion
}
