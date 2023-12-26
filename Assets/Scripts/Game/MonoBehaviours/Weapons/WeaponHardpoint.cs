using System;
using UnityEngine;

public class WeaponHardpoint : MonoBehaviour
{
    public string Id { get; private set; }
    private Weapon _weapon;

    // Start is called before the first frame update
    void Start()
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
            meshRenderer.enabled = false;

        Id = Guid.NewGuid().ToString();
    }

    public void Attach(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.transform.rotation = transform.rotation;
        _weapon.transform.parent = transform;
        _weapon.transform.localPosition = Vector3.zero;
    }
}
