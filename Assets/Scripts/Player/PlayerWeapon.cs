using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon ActiveWeapon = null;

    private Transform shipTransform;
    private bool initialized = false;

    void Start()
    {
        VerifyInitialize();
    }

    void VerifyInitialize()
    {
        if (initialized)
            return;

        shipTransform = transform.Find("Ship")?.GetComponent<Transform>();
        if (shipTransform == null)
            throw new ApplicationException("shipTransform is required");

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (HUD.Instance.IsLocked)
            return;

        if (Input.GetMouseButton(1))
        {
            // .. make the ship turn to face the mouse

            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Calculate the intersection point of the ray with the horizontal plane
            float distance;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
            if (plane.Raycast(ray, out distance))
            {
                // Get the world position of the intersection point
                Vector3 worldPosition = ray.GetPoint(distance);

                Vector3 targetDirection = worldPosition - shipTransform.position;
                shipTransform.forward = Vector3.Lerp(shipTransform.forward, targetDirection.normalized, Time.deltaTime * 10);

            }
        }
    }
}
