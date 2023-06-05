using System;
using UnityEngine;

public enum WeaponType
{
  ParticleBlaster
}
public class ItemManager : MonoBehaviour
{
  public GameObject ParticleBlasterPrefab;

  private static ItemManager instance;

  // Start is called before the first frame update
  void Start()
  {
    instance = this;
  }

  // Update is called once per frame
  void Update()
  {

  }


}
