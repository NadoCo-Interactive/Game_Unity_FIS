using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject ParticleBlasterPrefab;
    private static ItemManager instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {

    }

    public static IItem CreateItem(ItemType type)
      => throw new NotImplementedException();

    public static IWeaponItem CreateWeapon(WeaponType type)
      => type switch
      {
          WeaponType.ParticleBlaster => new WeaponItem()
          {
              Sprite = SpriteLibrary.ParticleBlasterWeaponSprite
          },
          _ => throw new NotImplementedException()
      };

}
