using System;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
  private ParticleSystem engine1, engine2;
  private ParticleSystem dustParticles;
  private Transform shipTransform;
  private Vector3 forwardVelocity, transverseVelocity;
  public float Acceleration = 1, Deceleration = 1;

  public Weapon ActiveWeapon = null;

  void Start()
  {

  }

  void VerifyInitialize()
  {
    shipTransform = transform.Find("Ship")?.GetComponent<Transform>();
    if (shipTransform == null)
      throw new ApplicationException("shipTransform is required");

    engine1 = shipTransform.Find("Engine")?.GetComponent<ParticleSystem>();
    if (engine1 == null)
      throw new ApplicationException("engine1 is required");

    engine2 = shipTransform.Find("Engine2")?.GetComponent<ParticleSystem>();
    if (engine2 == null)
      throw new ApplicationException("engine2 is required");

    dustParticles = transform.Find("Dust")?.GetComponent<ParticleSystem>();
    if (dustParticles == null)
      throw new ApplicationException("dustParticles is required");
  }

  void Update()
  {
    VerifyInitialize();

    transform.Translate((forwardVelocity + transverseVelocity) * Time.deltaTime);

    if (Input.GetKey(KeyCode.W))
    {
      engine1.Play();
      engine2.Play();
      forwardVelocity += shipTransform.forward * Time.deltaTime * Acceleration;
    }
    else if (Input.GetKey(KeyCode.S))
      forwardVelocity -= shipTransform.forward * Time.deltaTime * Acceleration;

    if (Input.GetKey(KeyCode.A))
    {
      transverseVelocity += -shipTransform.right * Time.deltaTime * Acceleration;
    }
    else if (Input.GetKey(KeyCode.D))
    {
      transverseVelocity += shipTransform.right * Time.deltaTime * Acceleration;
    }
    else
    {
      transverseVelocity = Vector3.Lerp(transverseVelocity, transverseVelocity * 0.9f, Time.deltaTime * Deceleration);

      if (transverseVelocity.magnitude <= 0.1)
        transverseVelocity = Vector3.zero;
    }

    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
    {
      int forwardMultiplier = Input.GetKey(KeyCode.S) ? -1 : 1;
      float dotProduct = Vector3.Dot(forwardVelocity.normalized, shipTransform.forward * forwardMultiplier);
      bool applyBrakingForce = dotProduct < 0f;

      if (applyBrakingForce)
      {
        var brakingForce = forwardVelocity * -1;
        forwardVelocity += brakingForce * Time.deltaTime;
      }
    }
    else
    {
      forwardVelocity = Vector3.Lerp(forwardVelocity, forwardVelocity * 0.9f, Time.deltaTime * Deceleration);

      if (forwardVelocity.magnitude <= 0.1)
        forwardVelocity = Vector3.zero;

      engine1.Stop();
      engine2.Stop();
    }

    var dpVel = dustParticles.velocityOverLifetime;
    dpVel.enabled = true;
    dpVel.x = forwardVelocity.x + transverseVelocity.x;
    dpVel.y = forwardVelocity.y + transverseVelocity.y;
    dpVel.z = forwardVelocity.z + transverseVelocity.z;

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
