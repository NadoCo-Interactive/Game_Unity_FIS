using System;
using UnityEngine;

public class ActorHardpoint : ActorComponent
{
    public ulong Id { get; set; }
    private Weapon _weapon;

    // Start is called before the first frame update
    void Start()
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }

    public void Attach(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.transform.rotation = transform.rotation;
        _weapon.transform.parent = transform;
        _weapon.transform.localPosition = Vector3.zero;
    }
}
