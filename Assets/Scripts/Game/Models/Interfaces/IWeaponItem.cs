public interface IWeaponItem : IItem
{
    public WeaponType WeaponType { get; set; }
    public Weapon Weapon { get; set; }
    public ActorHardpoint Hardpoint { get; set; }
    public int Ammo { get; set; }
}