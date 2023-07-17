using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAutoFire : ActorWeapon
{
    private float fireTimer = 1000;

    protected override void Start()
    {
        base.Start();
        var weapon = ItemManager.CreateWeapon(WeaponType.ParticleBlaster);
        weapon.Weapon = GetComponent<Weapon>();
        Equip(weapon);
    }

    void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime * 1000;
        else
        {
            fireTimer = 1000;
            Fire();
        }
    }
}
