using UnityEngine;

public enum BulletType
{
    ParticleBlasterBullet,
    Bullet
}

public enum DamageType
{
    Kinetic,
    Thermal,
    Explosive,
    EMP
}

public interface IBullet
{
    public BulletType Type { get; set; }
    public float Speed { get; set; }
    public float Lifetime { get; set; }

    public float DamageValue { get; set; }
    public DamageType DamageType { get; set; }
}