using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Actor : StrictBehaviour, IActor
{
    public IInventory Inventory { get; set; }
    public IActorWeapon Weapon { get; set; }
    public PlayerMotor Motor { get; set; }
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
        Motor = GetComponent<PlayerMotor>();
        Model = GetRequiredComponentInChildren<ActorModel>();
        Network = GetRequiredComponent<ActorNetwork>();
    }

    public void MakeRemote()
    {
        VerifyInitialize();
        Motor.DustParticles.gameObject.SetActive(false);
    }
}
