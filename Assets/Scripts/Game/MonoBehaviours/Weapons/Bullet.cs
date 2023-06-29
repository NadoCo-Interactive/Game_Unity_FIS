using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float Speed = 100;
  public float Lifetime = 100;

  void Update()
  {
    transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);

    if (Lifetime > 0)
      Lifetime -= Time.deltaTime * 100;
    else
      Destroy(gameObject);
  }
}
