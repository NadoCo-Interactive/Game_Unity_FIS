using UnityEngine;

public class WeaponItem : Item, IWeaponItem
{
    public WeaponHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}