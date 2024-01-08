using System;
using UnityEngine;

public interface IItem
{
    public ulong Id { get; set;}
    public int SlotId { get; set; }

    public ItemType ItemType { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }

    public ItemDTO ToDto();
}

public class Item : IItem
{
    public ulong Id { get; set; }
    public int SlotId { get; set; }

    public ItemType ItemType { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }

    public ItemDTO ToDto() => new ItemDTO()
    {
        Id = Id,
        ItemType = ItemType
    };
}

public struct ItemDTO : IEquatable<ItemDTO>
{
    public ulong Id { get; set; }
    public ItemType ItemType { get; set; }

    public bool Equals(ItemDTO other)
    {
        if(other.Id != Id) return false;
        if(other.ItemType != other.ItemType) return false;

        return true;
    }
}

