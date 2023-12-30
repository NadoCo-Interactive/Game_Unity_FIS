using UnityEngine;

public interface IItem
{
    public string Id { get; set;}
    public int SlotId { get; set; }

    public ItemType ItemType { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }

    public ItemDTO ToDTO();
}

public class Item : IItem
{
    public string Id { get; set; }
    public int SlotId { get; set; }

    public ItemType ItemType { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }

    public ItemDTO ToDTO() => new ItemDTO() {
        Id = Id,
        SlotId = SlotId,
        ItemType = ItemType
    };
}

public struct ItemDTO
{
    public string Id { get; set; }
    public int SlotId { get; set; }

    public ItemType ItemType { get; set; }
}