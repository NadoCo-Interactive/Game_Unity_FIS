using UnityEngine;

public interface IWeaponItem : IItem
{
    public WeaponHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}