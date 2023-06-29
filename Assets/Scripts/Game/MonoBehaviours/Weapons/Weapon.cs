using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    public GameObject Bullet;
    public AudioClip soundFire;
    [Range(0, 1)]
    public float soundVolume = 1;
    public int fireRate = 10;
    public FireMode fireMode = FireMode.FullAuto;

    private bool canFire = false;

    private List<AudioSource> activeAudioSources = new List<AudioSource>();
    private float fireTimer = 0;

    void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime * fireRate * 100;
        else
            canFire = true;

        cleanOldAudio();
    }

    void cleanOldAudio()
    {
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            var audioSource = activeAudioSources[i];
            if (!audioSource.isPlaying)
            {
                DestroyImmediate(audioSource);
                activeAudioSources.Remove(audioSource);
            }
        }
    }

    public void Fire()
    {
        if (!canFire)
            return;

        var audioFire = gameObject.AddComponent<AudioSource>();
        audioFire.clip = soundFire;
        audioFire.volume = soundVolume;
        audioFire.Play();
        activeAudioSources.Add(audioFire);

        var bullet = GameObject.Instantiate(Bullet);
        bullet.transform.position = transform.position;
        bullet.transform.forward = transform.forward;

        fireTimer = 1000;

        canFire = false;
    }
}
