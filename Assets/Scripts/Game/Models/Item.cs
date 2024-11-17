using System;
using Unity.Netcode;
using UnityEngine;

public interface IItem
{
    public ulong Id { get; set;}
    public int SlotId { get; set; }
    public ulong HardpointId { get; set; }

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
    public ulong HardpointId { get; set; }

    public ItemType ItemType { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }

    public ItemDTO ToDto() => new ItemDTO()
    {
        Id = Id,
        ItemType = ItemType,
        HardpointId = HardpointId
    };
}

public struct ItemDTO : INetworkSerializable, IEquatable<ItemDTO>
{
    public ulong Id;
    public ulong HardpointId;
    public ItemType ItemType;

    public bool Equals(ItemDTO other)
    {
        if(Id != other.Id) return false;
        if(ItemType != other.ItemType) return false;
        if(HardpointId != other.HardpointId) return false;

        return true;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref ItemType);
        serializer.SerializeValue(ref HardpointId);
    }
}

