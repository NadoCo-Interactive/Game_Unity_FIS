using System.Collections.Generic;

public interface IActorWeapon
{
    public IWeaponItem ActiveWeapon { get; set; }

    public List<WeaponHardpoint> Hardpoints { get; set; }
}