using UnityEngine;

public interface IActor
{
    public IInventory Inventory { get; set; }
    public IActorWeapon Weapon { get; set; }
    public PlayerMotor Motor { get; set; }
    public ParticleSystem Dust { get; set; }
    public Transform ShipTransform { get; set; }
    public IActorModel Model { get; set; }

    public IActorNetwork Network { get; set; }
}