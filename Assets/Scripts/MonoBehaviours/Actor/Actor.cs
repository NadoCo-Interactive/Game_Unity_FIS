using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : StrictBehaviour
{
    public IInventory Inventory;
    public IActorWeapon Weapon;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = GetRequiredComponent<PlayerInventory>();
        Weapon = GetComponent<ActorWeapon>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
