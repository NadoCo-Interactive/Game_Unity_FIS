using UnityEngine;

public class Explosion : StrictBehaviour
{
    protected AudioSource _audio;
    protected ParticleSystem _particles;
    public AudioClip audioClip;

    private bool isExploded = false;

    protected virtual void Start()
    {
        _audio = GetRequiredComponent<AudioSource>();
        _audio.clip = audioClip;
        _particles = GetRequiredComponent<ParticleSystem>();
        DoExplosion();
    }

    void Update()
    {
        if (isExploded && !_particles.isPlaying)
            GameObject.DestroyImmediate(gameObject);
    }

    public void DoExplosion()
    {
        _audio.Play();
        _particles.Play();
        isExploded = true;
    }
}
