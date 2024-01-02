using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IActorNetwork
{
    public bool IsLocalPlayer { get; }

    #region ActorMotor Variables
    public NetworkVariable<Vector3> Position { get; set; }
    public NetworkVariable<Vector3> Heading { get; set; }
    #endregion

    #region ActorWeapon Variables
    public NetworkList<ulong> HardpointIds { get; set; }
    #endregion

    #region ActorMotor Events
    /// <summary>
    /// Broadcast the ship's current position to other players
    /// </summary>
    /// <param name="position"></param>
    public void SetPositionServerRpc(Vector3 position);

    /// <summary>
    /// Broadcast the ship's current heading to other players
    /// </summary>
    /// <param name="ItemType"></param>
    /// <param name="hardpointId"></param>
    public void SetHeadingServerRpc(Vector3 rotation);
    #endregion

    #region ActorInventory Events

    /// <summary>
    /// Broadcast an AddItem event to other players
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="inventoryId"></param>
    public void AddItemServerRpc(ItemType itemType, string itemId, string inventoryId);

    /// <summary>
    /// Broadcast a RemoveItem event to other players
    /// </summary>
    /// <param name="itemId"></param>
    public void RemoveItemServerRpc(string itemId);

    /// <summary>
    /// Broadcast a TransferItem event to other players
    /// </summary>
    /// <param name="itemId"></param>
    public void TransferItemToServerRpc(string itemId, string toInventoryId);

    /// <summary>
    /// Broadcast a Fitting event to other players
    /// </summary>
    /// <param name="ItemType"></param>
    /// <param name="hardpointId"></param>
    public void AddFittingServerRpc(ItemType ItemType, string weaponId, string hardpointId);
    
    /// <summary>
    /// Broadcast an Unfitting event to other players
    /// </summary>
    /// <param name="ItemType"></param>
    /// <param name="hardpointId"></param>
    public void RemoveFittingServerRpc(string weaponId);
    #endregion
}