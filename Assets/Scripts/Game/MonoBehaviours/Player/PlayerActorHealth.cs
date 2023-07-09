public class PlayerActorHealth : ActorHealth
{
    public override void Die()
    {
        base.Die();
        EffectsManager.SpawnPlayerExplosion(transform.position);
    }
}