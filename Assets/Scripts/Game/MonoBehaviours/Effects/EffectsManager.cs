using UnityEngine;
using System;

public class EffectsManager : Singleton<EffectsManager>
{
    public Explosion ParticleBlasterExplosionPrefab;
    public Explosion PlayerExplosionPrefab;

    public static Explosion SpawnBulletExplosion(BulletType bulletType, Vector3 position)
    {
        if (bulletType == BulletType.ParticleBlasterBullet)
        {
            var explosionObject = spawnExplosion(Instance.ParticleBlasterExplosionPrefab.Required(), position);
            return explosionObject.GetRequiredComponent<Explosion>();
        }
        else
            throw new NotImplementedException();
    }

    public static Explosion SpawnPlayerExplosion(Vector3 position)
    {
        var explosionObject = spawnExplosion(Instance.PlayerExplosionPrefab.Required(), position);
        return explosionObject.GetRequiredComponent<Explosion>();
    }

    private static Explosion spawnExplosion(Explosion prefab, Vector3 position)
    {
        var explosionObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
        return explosionObject;
    }
}