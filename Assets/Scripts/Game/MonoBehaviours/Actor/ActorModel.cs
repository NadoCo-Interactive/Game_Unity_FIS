using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActorModel : ActorComponent, IActorModel
{
    private Collider[] _childColliders;
    private Rigidbody[] _childRigidbodies;

    private bool isBroken = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Break();
    }

    public void Break()
    {
        if (isBroken)
            return;

        var children = GetComponentsInChildren<Transform>();
        _childColliders = children.Select(c =>
        {
            var collider = c.gameObject.AddComponent<BoxCollider>();
            collider.center = Vector3.zero;
            collider.size = Vector3.one;
            return collider;
        }).ToArray();
        _childRigidbodies = children.Select(c =>
        {
            var rigidbody = c.gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            var localNormal = rigidbody.transform.localPosition.normalized;
            rigidbody.AddForce(localNormal, ForceMode.Impulse);
            return rigidbody;
        }).ToArray();

        foreach (Transform t in children)
            t.parent = null;

        isBroken = true;
    }
}
