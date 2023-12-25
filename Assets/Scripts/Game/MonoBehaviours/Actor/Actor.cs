using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : StrictBehaviour, IActor
{
    public IInventory Inventory { get; set; }
    public IActorWeapon Weapon { get; set; }
    public IActorComponent Motor { get; set; }
    public ParticleSystem Dust { get; set; }
    public Transform ShipTransform { get; set; }
    public IActorModel Model { get; set; }

    public IActorNetwork Network { get; set; }

    protected virtual void Start()
    {
        VerifyInitialize();
    }

    public void VerifyInitialize()
    {
        ShipTransform = transform.Find("Ship").Required().transform;

        Inventory = GetRequiredComponent<ActorInventory>();
        Weapon = GetComponent<ActorWeapon>();
        Motor = GetComponent<ActorMotor>();
        Model = GetRequiredComponentInChildren<ActorModel>();
        Network = GetComponent<ActorNetwork>();
    }

    public void MakeRemote()
    {
        VerifyInitialize();

        Motor.enabled = false;
        Weapon.enabled = false;
        Inventory.enabled = false;
    }
}
