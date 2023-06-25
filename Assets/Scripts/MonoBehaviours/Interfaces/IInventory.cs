using UnityEngine;
using System.Collections.Generic;

public interface IInventory
{
    public List<IItem> Items { get; set; }
    public List<IWeaponItem> Fittings { get; set; }
    public int MaxItems { get; set; }
    public int MaxFittings { get; set; }

    public IItem SelectedItem { get; set; }

    public void AddItem(IItem item);
    public void RemoveItem(IItem item);
    public void AddFitting(IItem weapon, WeaponHardpoint hardpoint = null);
    public void RemoveFitting(IItem weapon);
}