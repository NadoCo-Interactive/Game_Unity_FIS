using UnityEngine;

public interface IWeaponItem : IItem
{
    public Weapon Weapon { get; set; }
    public WeaponHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}