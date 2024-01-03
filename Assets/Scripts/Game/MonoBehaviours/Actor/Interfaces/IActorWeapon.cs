using System.Collections.Generic;

public interface IActorWeapon : IActorComponent
{
    public IWeaponItem ActiveWeapon { get; set; }

    public List<ActorHardpoint> Hardpoints { get; set; }

    public void VerifyInitialize();
}