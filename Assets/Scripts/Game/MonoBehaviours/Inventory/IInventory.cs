using UnityEngine;
using System.Collections.Generic;
using System;

public interface IInventory : IActorComponent
{
    public List<IItem> Items { get; set; }
    public List<IWeaponItem> Fittings { get; set; }
    public int MaxItems { get; set; }
    public int MaxFittings { get; set; }

    public IItem SelectedItem { get; set; }

    public void AddItem(IItem item);
    public void RemoveItem(IItem item);
    public void AddFitting(IItem weapon, ActorHardpoint hardpoint = null);
    public void AddFitting(IItem weaponItem, int hardpointId);
    public void RemoveFitting(IItem weapon);
    public void RemoveFittingByItemId(string itemId);
    public bool HasFittedItem(IItem item);
    public bool HasFittingForSlot(int slotId);
}