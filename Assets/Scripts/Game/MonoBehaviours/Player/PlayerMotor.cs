using UnityEngine;

public class PlayerMotor : ActorMotor
{
    private ParticleSystem dustParticles;

    protected override void Start()
    {
        base.Start();
        VerifyInitialize();
    }

    protected override void VerifyInitialize()
    {
        dustParticles = transform.Find("Dust").Required().GetRequiredComponent<ParticleSystem>();
        base.VerifyInitialize();
    }

    protected override void Update()
    {
        DoParticles();

        if (!Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || HUD.Instance.IsLocked)
            BrakePassively();

        if (HUD.Instance.IsLocked)
        {
            BrakeStrafe();
            return;
        }

        if (Input.GetKey(KeyCode.W) && forwardVelocity.magnitude < 50)
            Accelerate();
        else if (Input.GetKey(KeyCode.S) && forwardVelocity.magnitude > -50)
            Decelerate();

        if (Input.GetKey(KeyCode.A))
            Strafe(Direction.Left);
        else if (Input.GetKey(KeyCode.D))
            Strafe(Direction.Right);
        else
            BrakeStrafe();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            BrakeActively();

        base.Update();
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
}