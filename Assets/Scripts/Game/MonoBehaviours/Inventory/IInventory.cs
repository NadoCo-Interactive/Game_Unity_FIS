using UnityEngine;
using System.Collections.Generic;
using System;

public interface IInventory : IActorComponent
{
    public string Id { get; set; }
    public List<IItem> Items { get; set; }
    public List<IWeaponItem> Fittings { get; set; }
    public int MaxItems { get; set; }
    public int MaxFittings { get; set; }

    public IItem SelectedItem { get; set; }

    public void InitializeItems(ItemDTO[] items);

    public void AddItem(IItem item, bool localOnly = false);
    public void RemoveItem(IItem item);
    public void TransferItemTo(IItem item, IInventory toInventory);

    public void AddFitting(IItem weapon, ActorHardpoint hardpoint = null);
    public void RemoveFitting(IItem weapon);
    public void RemoveFittingByItemId(ulong itemId);
    public bool HasFittedItem(IItem item);
    public bool HasFittingForSlot(int slotId);
}