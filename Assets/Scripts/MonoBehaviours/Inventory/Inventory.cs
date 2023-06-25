using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour, IInventory
{
    public List<IItem> Items { get; set; } = new();
    public List<IWeaponItem> Fittings { get; set; } = new();
    public int MaxItems { get; set; } = 15;
    public int MaxFittings { get; set; } = 7;

    public IItem SelectedItem { get; set; }

    public void AddItem(IItem item)
    {
        if (Items.Count >= MaxItems)
            throw new ApplicationException("Inventory Full");

        item.Id = Items.Count + 1;
        Items.Add(item);
    }

    public void RemoveItem(IItem item)
    {
        Items.Remove(item);
    }

    public bool HasFittedItem(IItem item)
    {
        var fittingsAsItems = Fittings.Cast<IItem>();

        return fittingsAsItems.Contains(item);
    }

    public bool HasFittingForId(int id)
        => Fittings.ElementAt(id) != null;

    public void AddFitting(IItem weaponItem, WeaponHardpoint hardpoint = null)
    {
        if (!(weaponItem is IWeaponItem))
            throw new ArgumentException("Weapon must be of type \"Weapon\" to be fitted");

        if (Fittings.Count >= MaxFittings)
            throw new ApplicationException("No more space for fittings");

        weaponItem.Id = Fittings.Count + 1;
        Fittings.Add(weaponItem as IWeaponItem);

        if (hardpoint != null)
        {
            throw new NotImplementedException();
        }
        else
        {
            var randHardpointIndex = Random.Range(0, PlayerWeapon.Instance.Hardpoints.Count - 1);
            var randomHardpoint = PlayerWeapon.Instance.Hardpoints.ElementAt(randHardpointIndex);

            if (randomHardpoint == null)
            {
                Debug.LogWarning("No available hardpoint to attach weapon");
                return;
            }

            var weaponPrefabInstance = ItemManager.SpawnItem(weaponItem);
            var weapon = weaponPrefabInstance.GetRequiredComponent<Weapon>();
            randomHardpoint.Attach(weapon);
        }
    }

    public void RemoveFitting(IItem weapon)
    {
        Fittings.Remove(weapon as IWeaponItem);
    }
}
