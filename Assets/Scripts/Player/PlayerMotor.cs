using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private ParticleSystem engine1, engine2;
    private ParticleSystem dustParticles;
    private AudioSource engineAudio;
    private Transform shipTransform;
    private Vector3 forwardVelocity, transverseVelocity;
    public float Acceleration = 1, Deceleration = 1;

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

        engine1 = shipTransform.Find("Engine")?.GetComponent<ParticleSystem>();
        if (engine1 == null)
            throw new ApplicationException("engine1 is required");

        engine2 = shipTransform.Find("Engine2")?.GetComponent<ParticleSystem>();
        if (engine2 == null)
            throw new ApplicationException("engine2 is required");

        dustParticles = transform.Find("Dust")?.GetComponent<ParticleSystem>();
        if (dustParticles == null)
            throw new ApplicationException("dustParticles is required");

        engineAudio = shipTransform.GetComponent<AudioSource>();
        if (engineAudio == null)
            throw new ApplicationException("engineAudio is required");

        initialized = true;
    }

    void Update()
    {
        VerifyInitialize();

        transform.Translate((forwardVelocity + transverseVelocity) * Time.deltaTime);
        engineAudio.pitch = 1 + (forwardVelocity.magnitude / 50);

        DoParticles();
        DoPassiveBraking();

        if (HUD.Instance.IsLocked)
            return;

        DoForwardBackward();
        DoStrafing();
        DoActiveBraking();
    }

    private void DoParticles()
    {
        var dpVel = dustParticles.velocityOverLifetime;
        dpVel.enabled = true;
        dpVel.x = forwardVelocity.x + transverseVelocity.x;
        dpVel.y = forwardVelocity.y + transverseVelocity.y;
        dpVel.z = forwardVelocity.z + transverseVelocity.z;

        var dpEmission = dustParticles.emission;
        dpEmission.rateOverTime = 10 + ((forwardVelocity.magnitude / 25) * 90);
    }

    private void DoPassiveBraking()
    {
        if (!Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || HUD.Instance.IsLocked)
        {
            forwardVelocity = Vector3.Lerp(forwardVelocity, forwardVelocity * 0.9f, Time.deltaTime * Deceleration);

            if (forwardVelocity.magnitude <= 0.1)
                forwardVelocity = Vector3.zero;

            engine1.Stop();
            engine2.Stop();
        }

        if (HUD.Instance.IsLocked)
            DoStrafeBraking();
    }

    private void DoActiveBraking()
    {
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
    }

    private void DoForwardBackward()
    {
        if (Input.GetKey(KeyCode.W))
        {
            engine1.Play();
            engine2.Play();

            if (forwardVelocity.magnitude < 50)
                forwardVelocity += shipTransform.forward * Time.deltaTime * Acceleration;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (forwardVelocity.magnitude > -50)
                forwardVelocity -= shipTransform.forward * Time.deltaTime * Acceleration;
        }
    }

    private void DoStrafing()
    {
        if (Input.GetKey(KeyCode.A))
            transverseVelocity += -shipTransform.right * Time.deltaTime * Acceleration;
        else if (Input.GetKey(KeyCode.D))
            transverseVelocity += shipTransform.right * Time.deltaTime * Acceleration;
        else
            DoStrafeBraking();
    }

    private void DoStrafeBraking()
    {
        transverseVelocity = Vector3.Lerp(transverseVelocity, transverseVelocity * 0.9f, Time.deltaTime * Deceleration);

        if (transverseVelocity.magnitude <= 0.1)
            transverseVelocity = Vector3.zero;
    }
}
