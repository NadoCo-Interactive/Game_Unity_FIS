using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    public BulletType Type { get; set; }
    public float Speed { get; set; } = 100;
    public float Lifetime { get; set; } = 100;

    public DamageType DamageType { get; set; } = DamageType.Kinetic;

    [SerializeField]
    private float damageValue = 1;
    public float DamageValue { get => damageValue; set => damageValue = value; }

    private GameObject _owner;
    private Collider[] _ownerColliders;

    protected virtual void Start()
    {
        Type = BulletType.Bullet;
    }

    void Update()
    {
        transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);

        if (Lifetime > 0)
            Lifetime -= Time.deltaTime * 100;
        else
            DoDeath();
    }

    public void SetOwner(GameObject gameObject)
    {
        _ownerColliders = gameObject.GetComponentsInChildren<Collider>();
    }

    public bool ColliderIsOwner(Collider col)
      => _ownerColliders.Required("Bullets must belong to a game object").Any(c => c == col);

    void OnCollisionEnter(Collision col)
    {
        DoDeath();
    }

    protected virtual void DoDeath()
    {
        Destroy(gameObject);
    }
}
