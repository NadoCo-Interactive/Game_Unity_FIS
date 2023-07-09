using System.Collections.Generic;

public interface IActorWeapon : IActorComponent
{
    public IWeaponItem ActiveWeapon { get; set; }

    public List<WeaponHardpoint> Hardpoints { get; set; }
}