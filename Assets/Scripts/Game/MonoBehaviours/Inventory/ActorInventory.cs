using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActorInventory : ActorComponent, IInventory
{
    public string Id { get; set; }
    public List<IItem> Items { get; set; } = new();
    public List<IWeaponItem> Fittings { get; set; } = new();
    public int MaxItems { get; set; } = 15;
    public int MaxFittings { get; set; } = 7;

    public IItem SelectedItem { get; set; }

    private ActorWeapon _ActorWeapon;
    private bool _initialized = false;

    protected void Start()
    {
        VerifyInitialize();

        Id = Guid.NewGuid().ToString();
    }

    void VerifyInitialize()
    {
        if (_initialized)
            return;

        _ActorWeapon = GetRequiredComponent<ActorWeapon>();

        _initialized = true;
    }

    public void AddItem(IItem item, string itemId = null)
    {
        if (Items.Count >= MaxItems)
            throw new ApplicationException("Inventory Full");

        if(itemId != null)
            item.Id = itemId;

        item.Required().SlotId = Items.Count + 1;
        Items.Add(item);

        if(IsNetwork)
            Actor.Network.AddItemServerRpc(item.ItemType,item.Id,Id);

        Debug.Log("Added item "+item.ItemType+" to "+Id+" ("+Actor.gameObject.name+")");
    }

    public void RemoveItem(IItem item)
    {
        Items.Remove(item);

        if(IsNetwork)
            Actor.Network.RemoveItemServerRpc(item.Id);

        Debug.Log("Added item "+item.ItemType+" from "+Id+" ("+Actor.gameObject.name+")");
    }

    public void TransferItemTo(IItem item, IInventory inventoryTo)
    {
        inventoryTo.AddItem(item);
        RemoveItem(item);

        if(IsNetwork)
            Actor.Network.TransferItemToServerRpc(item.Id,inventoryTo.Id);

        Debug.Log("Transfered item "+item.ItemType+" to "+inventoryTo.Id+" ("+inventoryTo.Actor.gameObject.name+")");
    }

    public bool HasFittedItem(IItem item)
    {
        var fittingsAsItems = Fittings.Cast<IItem>();

        return fittingsAsItems.Contains(item);
    }

    public bool HasFittingForSlot(int id)
        => Fittings.FirstOrDefault(f => f.SlotId == id) != null;

    public virtual void AddFitting(IItem weaponItem, ActorHardpoint hardpoint = null)
    {
        if (weaponItem is not IWeaponItem)
            throw new ArgumentException("Weapon must be of type \"Weapon\" to be fitted");

        if (Fittings.Count >= MaxFittings)
            throw new ApplicationException("No more space for fittings");

        weaponItem.SlotId = Fittings.Count + 1;
        Fittings.Add(weaponItem as IWeaponItem);

        var weaponPrefabInstance = ItemManager.SpawnItem(weaponItem);
        var weapon = weaponPrefabInstance.GetRequiredComponent<Weapon>();

        if (hardpoint == null)
        {
            var randHardpointIndex = Random.Range(0, _ActorWeapon.Hardpoints.Count - 1);
            var randomHardpoint = _ActorWeapon.Hardpoints.ElementAt(randHardpointIndex);

            if (randomHardpoint == null)
            {
                Debug.LogWarning("No available hardpoint to attach weapon");
                RemoveFitting(weaponItem);
                return;
            }

            hardpoint = randomHardpoint;
        }


        Debug.Log("attach to " + hardpoint.gameObject.name);
        hardpoint.Attach(weapon);

        if(IsNetwork)
          Actor.Network.AddFittingServerRpc((weaponItem as IWeaponItem).ItemType,weaponItem.Id,hardpoint.Id);
    }

    public void RemoveFitting(IItem weapon)
    {
        Fittings.Remove(weapon as IWeaponItem);
        ItemManager.DespawnItem(weapon);

        if (HUDEquipped.ActiveWeapon == weapon)
            HUDEquipped.SetWeapon(null);

        if (Actor.Weapon != null)
            Actor.Weapon.ActiveWeapon = null;

        if(IsNetwork)
            Actor.Network.RemoveFittingServerRpc(weapon.Id);
    }

    public void RemoveFittingByItemId(string itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        RemoveFitting(item);
    }
}
