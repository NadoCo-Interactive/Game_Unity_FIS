using System;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public GameObject ParticleBlasterPrefab;

    
    public static IItem CreateItem(ItemType type)
    {
        var item = createItem(type);
        item.ItemType = type;
        item.Id = Guid.NewGuid().ToString();

        return item;
    }

    private static IItem createItem(ItemType type)
    => type switch
    {
        _ => throw new NotImplementedException()
    };

    public static IWeaponItem CreateWeapon(WeaponType type)
    {
        var weapon = createWeapon(type);
        weapon.WeaponType = type;
        weapon.Id = Guid.NewGuid().ToString();

        return weapon;
    }

    private static IWeaponItem createWeapon(WeaponType type)
    => type switch
      {
          WeaponType.ParticleBlaster => new WeaponItem()
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
