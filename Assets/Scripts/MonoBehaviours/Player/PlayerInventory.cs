using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : Inventory
{
    void Start()
    {
        var particleBlaster = ItemManager.CreateWeapon(WeaponType.ParticleBlaster);
        AddItem(particleBlaster);

        InventoryManager.SetActiveInventory(this);
    }

    void Update()
    {

    }
}