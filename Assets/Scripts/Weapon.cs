using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public enum FireType { FullAuto, SemiAuto }
  public AudioClip soundFire;
  private List<AudioSource> activeAudioSources = new List<AudioSource>();
  public int fireRate = 10;
  public FireType fireType = FireType.FullAuto;
  private float fireTimer = 0;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (fireTimer > 0)
    {
      fireTimer = Mathf.Lerp(fireTimer, fireTimer * .9f, Time.deltaTime * fireRate * 100);
    }

    if (Input.GetMouseButton(0) && fireType == FireType.FullAuto && fireTimer == 0)
    {
      Fire();
      fireTimer = 1000;
    }

    for (int i = 0; i < activeAudioSources.Count; i++)
    {
      var audio = activeAudioSources[i];
      if (!audio.isPlaying)
      {
        //DestroyImmediate(audio);
        //activeAudioSources.Remove(audio);
      }
    }

    Debug.Log("fireTimer=" + fireTimer);
  }

  public void Fire()
  {
    var audioFire = gameObject.AddComponent<AudioSource>();
    audioFire.clip = soundFire;
    audioFire.Play();
    activeAudioSources.Add(audioFire);
  }
}
