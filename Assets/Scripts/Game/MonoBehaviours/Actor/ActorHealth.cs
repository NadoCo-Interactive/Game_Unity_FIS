using UnityEngine;

public class ActorHealth : ActorComponent
{
    protected float health = 100;

    public void OnCollisionEnter(Collision col)
    {
        var bullet = col.gameObject.GetComponent<Bullet>();
        if (bullet == null || bullet.ColliderIsOwner(GetRequiredComponent<Collider>()))
            return;

        DoDamage(bullet.DamageValue);
    }

    public void DoDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public virtual void Die()
    {
        Actor.Model.Break();
        GameObject.Destroy(Actor.gameObject);
    }
}