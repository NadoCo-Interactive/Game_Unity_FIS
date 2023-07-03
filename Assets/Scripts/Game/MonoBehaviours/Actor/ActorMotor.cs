using System;
using UnityEngine;

public class ActorMotor : ActorComponent
{
    protected ParticleSystem engine1Particles, engine2Particles;

    protected AudioSource engineAudio;

    protected Vector3 forwardVelocity, transverseVelocity;
    public float Acceleration = 1, Deceleration = 1;

    private bool initialized = false;

    protected virtual void Start()
    {
        VerifyInitialize();
    }

    protected virtual void VerifyInitialize()
    {
        if (initialized)
            return;

        if (Actor == null)
            throw new ApplicationException("Actor is required");

        engine1Particles = Actor.ShipTransform.Find("Engine")?.GetComponent<ParticleSystem>();
        if (engine1Particles == null)
            throw new ApplicationException("engine1Particles is required");

        engine2Particles = Actor.ShipTransform.Find("Engine2")?.GetComponent<ParticleSystem>();
        if (engine2Particles == null)
            throw new ApplicationException("engine2Particles is required");

        engineAudio = Actor.ShipTransform.GetComponent<AudioSource>();
        if (engineAudio == null)
            throw new ApplicationException("engineAudio is required");

        initialized = true;
    }

    protected virtual void Update()
    {
        VerifyInitialize();

        transform.Translate((forwardVelocity + transverseVelocity) * Time.deltaTime);
        engineAudio.pitch = 1 + (forwardVelocity.magnitude / 50);

        if (forwardVelocity.magnitude > 0)
        {
            engine1Particles.Play();
            engine2Particles.Play();
        }
        else
        {
            engine1Particles.Stop();
            engine2Particles.Stop();
        }
    }

    public void Accelerate()
    {
        forwardVelocity += Actor.ShipTransform.forward * Time.deltaTime * Acceleration;
    }

    public void Decelerate()
    {
        forwardVelocity -= Actor.ShipTransform.forward * Time.deltaTime * Acceleration;
    }

    public void Strafe(Direction direction)
    {
        var strafeMultiplier = direction == Direction.Left ? -1 : 1;
        transverseVelocity += Actor.ShipTransform.right * strafeMultiplier * Time.deltaTime * Acceleration;
    }

    public void BrakeActively()
    {
        int forwardMultiplier = Input.GetKey(KeyCode.S) ? -1 : 1;
        float dotProduct = Vector3.Dot(forwardVelocity.normalized, Actor.ShipTransform.forward * forwardMultiplier);
        bool applyBrakingForce = dotProduct < 0f;

        if (applyBrakingForce)
        {
            var brakingForce = forwardVelocity * -1;
            forwardVelocity += brakingForce * Time.deltaTime;
        }
    }

    public void BrakePassively()
    {
        forwardVelocity = Vector3.Lerp(forwardVelocity, forwardVelocity * 0.9f, Time.deltaTime * Deceleration);

        if (forwardVelocity.magnitude <= 0.1)
            forwardVelocity = Vector3.zero;
    }

    public void BrakeStrafe()
    {
        transverseVelocity = Vector3.Lerp(transverseVelocity, transverseVelocity * 0.9f, Time.deltaTime * Deceleration);

        if (transverseVelocity.magnitude <= 0.1)
            transverseVelocity = Vector3.zero;
    }
}
