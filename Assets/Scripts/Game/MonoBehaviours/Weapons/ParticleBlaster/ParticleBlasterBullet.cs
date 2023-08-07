using UnityEngine;
using System.Linq;

public class ParticleBlasterBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        Type = BulletType.ParticleBlasterBullet;
    }

    void OnCollisionEnter(Collision col)
    {
        if (ColliderIsOwner(col.collider))
            return;

        DoDeath();
    }

    protected override void DoDeath()
    {
        EffectsManager.SpawnBulletExplosion(BulletType.ParticleBlasterBullet, transform.position);
        base.DoDeath();
    }
}