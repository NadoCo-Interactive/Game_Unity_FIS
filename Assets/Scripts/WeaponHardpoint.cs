using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHardpoint : MonoBehaviour
{
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
        _weapon.transform.parent = transform;
        _weapon.transform.localPosition = Vector3.zero;
    }

    public void DetachWeapon()
    {
        _weapon = null;
    }
}
