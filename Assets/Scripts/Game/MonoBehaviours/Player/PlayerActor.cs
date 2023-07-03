using UnityEngine;

public class PlayerActor : Actor
{
    protected override void Start()
    {
        base.Start();

        Dust = transform.Find("Dust").Required().GetRequiredComponent<ParticleSystem>();
        Inventory = GetRequiredComponent<PlayerInventory>();
        Motor = GetRequiredComponent<PlayerMotor>();
        Weapon = GetRequiredComponent<PlayerWeapon>();
    }


}