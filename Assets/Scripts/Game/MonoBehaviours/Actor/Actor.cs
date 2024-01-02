using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Actor : StrictBehaviour, IActor
{
    public IInventory Inventory { get; set; }

    private IActorWeapon _weapon;
    public IActorWeapon Weapon
    {
        get
        {
            if(_weapon == null)
                _weapon = GetComponent<ActorWeapon>();

            return _weapon;
        }
        set { _weapon = value; }
    }

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
        _weapon = GetComponent<ActorWeapon>();
        Motor = GetComponent<PlayerMotor>();
        Model = GetRequiredComponentInChildren<ActorModel>();
        Network = GetRequiredComponent<ActorNetwork>();
    }

    public void MakeRemote()
    {
        VerifyInitialize();

        if(Motor != null)
            Motor.DustParticles.gameObject.SetActive(false);
    }
}
