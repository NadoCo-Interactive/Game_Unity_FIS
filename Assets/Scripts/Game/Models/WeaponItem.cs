public interface IWeaponItem : IItem
{
    public Weapon Weapon { get; set; }
    public ActorHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}

public class WeaponItem : Item, IWeaponItem
{
    public Weapon Weapon { get; set; }
    public ActorHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}