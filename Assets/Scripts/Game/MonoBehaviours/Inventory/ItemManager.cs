using System;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public GameObject ParticleBlasterPrefab;

    
    public static IItem CreateItem(ItemType type, ulong? Id = null)
    {
        var item = createItem(type);
        item.ItemType = type;
        item.Id = Id ?? Guid.NewGuid().ToUlong();

        return item;
    }

    private static IItem createItem(ItemType type)
    => type switch
    {
        ItemType.ParticleBlaster => new WeaponItem()
          {
              Name = "Particle Blaster",
              Prefab = Instance.ParticleBlasterPrefab,
              Sprite = SpriteLibrary.ParticleBlasterWeaponSprite
          },
        _ => throw new NotImplementedException()
    };

    public static IWeaponItem CreateWeapon(ItemType type)
    {
        var weapon = createWeapon(type);
        weapon.ItemType = type;
        weapon.Id = Guid.NewGuid().ToUlong();

        return weapon;
    }

    private static IWeaponItem createWeapon(ItemType type)
    => type switch
      {
          ItemType.ParticleBlaster => new WeaponItem()
          {
              Name = "Particle Blaster",
              Prefab = Instance.ParticleBlasterPrefab,
              Sprite = SpriteLibrary.ParticleBlasterWeaponSprite
          },
          _ => throw new NotImplementedException()
      };

    public static GameObject SpawnItem(IItem item, Vector3? position = null)
    {
        var itemPrefab = item?.Prefab.Required();

        var itemPrefabInstance = Instantiate(itemPrefab);
        itemPrefabInstance.transform.position = position ?? Vector3.zero;
        item.PrefabInstance = itemPrefabInstance;

        if (item is IWeaponItem)
            (item as IWeaponItem).Weapon = itemPrefabInstance.GetRequiredComponent<Weapon>();

        return itemPrefabInstance;
    }

    public static void DespawnItem(IItem item)
    {
        var itemPrefabInstance = item?.PrefabInstance.Required();

        if (item is IWeaponItem)
            (item as IWeaponItem).Weapon = null;

        Destroy(itemPrefabInstance);
    }

}
