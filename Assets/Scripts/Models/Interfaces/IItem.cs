
using UnityEngine;

public interface IItem
{
    public int Id { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }

    public Sprite Sprite { get; set; }
}