using System;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
  private ParticleSystem engineParticles;
  private ParticleSystem dustParticles;
  private Vector3 forwardVelocity, transverseVelocity;
  public float Acceleration = 1, Deceleration = 1;

  void Start()
  {
    engineParticles = transform.Find("Engine")?.GetComponent<ParticleSystem>();
    if (engineParticles == null)
      throw new ApplicationException("engineParticles is required");

    dustParticles = transform.Find("Dust")?.GetComponent<ParticleSystem>();
    if (dustParticles == null)
      throw new ApplicationException("dustParticles is required");
  }

  void Update()
  {
    transform.Translate((forwardVelocity + transverseVelocity) * Time.deltaTime);

    if (Input.GetKey(KeyCode.W))
    {
      forwardVelocity += transform.forward * Time.deltaTime * Acceleration;
      engineParticles.Play();
    }
    else
    {
      forwardVelocity = Vector3.Lerp(forwardVelocity, forwardVelocity * 0.9f, Time.deltaTime * Deceleration);

      if (forwardVelocity.magnitude <= 0.1)
        forwardVelocity = Vector3.zero;

      engineParticles.Stop();
    }

    Debug.Log("forwardVelocity=" + forwardVelocity);

    var dpMain = dustParticles.main;

    if (forwardVelocity.magnitude > 0)
    {
      dpMain.startSpeed = 10 * forwardVelocity.magnitude;
      dustParticles.Play();
    }
    else
      dustParticles.Stop();

    if (Input.GetMouseButton(1))
    {
      // .. make the ship turn to face the mouse
    }

  }
}
