using UnityEngine;

public class WeaponItem : Item, IWeaponItem
{
    public Weapon Weapon { get; set; }
}