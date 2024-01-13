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
        verifyInitialize();

        Id = Guid.NewGuid().ToString();
        
    }

    void verifyInitialize()
    {
        if (_initialized)
            return;

        _ActorWeapon = GetRequiredComponent<ActorWeapon>();

        _initialized = true;
    }

    public void InitializeItems(ItemDTO[] items)
    {
        foreach(ItemDTO dto in items)
        {
            var item = ItemManager.CreateItem(dto.ItemType,dto.Id);
            AddItem(item,true);
        }
    }

    public void AddItem(ItemDTO dto)
    {
        var item = ItemManager.CreateItem(dto.ItemType,dto.Id);
        AddItem(item,true);
    }

    public void AddItem(IItem item, bool localOnly = false)
    {
        if (Items.Count >= MaxItems)
            throw new ApplicationException("Inventory Full");

        item.Required().SlotId = Items.Count + 1;
        Items.Add(item);

        if(CanUseNetwork && !localOnly)
        {
            GameLog.Log("sent addItem packet for "+item.ItemType);
            Actor.Network.AddItemServerRpc(item.ItemType,item.Id);
        }
    }

    public void RemoveItem(IItem item)
    {
        Items.Remove(item);

        if (CanUseNetwork)
        {
            GameLog.Log("Removed item " + item.ItemType + " from " + Id + " (" + Actor.gameObject.name + ")");
            GameLog.Log("sent removeItem packet");
            Actor.Network.RemoveItemServerRpc(item.Id);
        }
    }

    public void TransferItemTo(IItem item, IInventory inventoryTo)
    {
        inventoryTo.AddItem(item);
        RemoveItem(item);

        if(CanUseNetwork)
            Actor.Network.TransferItemServerRpc(item.Id,inventoryTo.Id);

        GameLog.Log("Transfered item "+item.ItemType+" to "+inventoryTo.Id+" ("+inventoryTo.Actor.gameObject.name+")");
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
        GameLog.Log("added "+weaponItem.Name+" as a fitting");

        var weaponPrefabInstance = ItemManager.SpawnItem(weaponItem);
        var weapon = weaponPrefabInstance.GetRequiredComponent<Weapon>();

        if (hardpoint == null)
        {
            var randHardpointIndex = Random.Range(0, _ActorWeapon.Hardpoints.Count - 1);
            var randomHardpoint = _ActorWeapon.Hardpoints.ElementAt(randHardpointIndex);

            if (randomHardpoint == null)
            {
                GameLog.LogWarning("No available hardpoint to attach weapon");
                RemoveFitting(weaponItem);
                return;
            }

            hardpoint = randomHardpoint;
        }

        weaponItem.HardpointId = hardpoint.Id;
        hardpoint.Attach(weapon);

        if(CanUseNetwork)
        {
            GameLog.Log("sending fitting packet");
            Actor.Network.AddFittingServerRpc(weaponItem.ToDto());
        }
    }

    public void RemoveFitting(IItem weapon)
    {
        Fittings.Remove(weapon as IWeaponItem);
        ItemManager.DespawnItem(weapon);

        if (HUDEquipped.ActiveWeapon == weapon)
            HUDEquipped.SetWeapon(null);

        if (Actor.Weapon != null)
            Actor.Weapon.ActiveWeapon = null;

        if(CanUseNetwork)
            Actor.Network.RemoveFittingServerRpc(weapon.Id);
    }

    public void RemoveFittingByItemId(ulong itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        RemoveFitting(item);
    }
}
