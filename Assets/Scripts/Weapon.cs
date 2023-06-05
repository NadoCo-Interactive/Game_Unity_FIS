using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public enum FireType { FullAuto, SemiAuto }

  public GameObject Bullet;
  public AudioClip soundFire;
  [Range(0, 1)]
  public float soundVolume = 1;
  public int fireRate = 10;
  public FireType fireType = FireType.FullAuto;

  public bool IsActive = false;

  private List<AudioSource> activeAudioSources = new List<AudioSource>();
  private float fireTimer = 0;

  void Start()
  {

  }

  void Update()
  {
    if (fireTimer > 0)
      fireTimer -= Time.deltaTime * fireRate * 100;

    cleanOldAudio();

    if (!IsActive)
      return;

    if (Input.GetMouseButton(0) && fireType == FireType.FullAuto && fireTimer <= 0)
    {
      Fire();
      fireTimer = 1000;
    }

    if (Input.GetMouseButtonDown(0) && fireType == FireType.SemiAuto && fireTimer <= 0)
    {
      Fire();
      fireTimer = 1000;
    }
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
    var audioFire = gameObject.AddComponent<AudioSource>();
    audioFire.clip = soundFire;
    audioFire.volume = soundVolume;
    audioFire.Play();
    activeAudioSources.Add(audioFire);

    var bullet = GameObject.Instantiate(Bullet);
    bullet.transform.position = transform.position;
    bullet.transform.forward = transform.forward;
  }
}
