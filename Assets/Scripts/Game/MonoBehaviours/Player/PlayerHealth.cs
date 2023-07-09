using UnityEngine;

public class PlayerHealth : ActorHealth
{
    public override void Die()
    {
        base.Die();
        EffectsManager.SpawnPlayerExplosion(transform.position);
    }
}