using System;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public GameObject ParticleBlasterPrefab;

    public static IItem CreateItem(ItemType type)
      => throw new NotImplementedException();

    public static IWeaponItem CreateWeapon(WeaponType type)
      => type switch
      {
          WeaponType.ParticleBlaster => new WeaponItem()
          {
              Prefab = Instance.ParticleBlasterPrefab,
              Sprite = SpriteLibrary.ParticleBlasterWeaponSprite
          },
          _ => throw new NotImplementedException()
      };

    public static GameObject SpawnItem(IItem item, Vector3? position = null)
    {
        var itemPrefab = item?.Prefab.Required();

        var itemPrefabInstance = GameObject.Instantiate(itemPrefab);
        itemPrefabInstance.transform.position = position ?? Vector3.zero;
        item.PrefabInstance = itemPrefabInstance;

        return itemPrefabInstance;
    }

    public static void DespawnItem(IItem item)
    {
        var itemPrefabInstance = item?.PrefabInstance.Required();

        GameObject.Destroy(itemPrefabInstance);
    }

}
