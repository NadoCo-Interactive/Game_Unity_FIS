using UnityEngine;

public class WeaponItem : Item, IWeaponItem
{
    public Weapon Weapon { get; set; }
    public WeaponType WeaponType { get; set;}
    public ActorHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}