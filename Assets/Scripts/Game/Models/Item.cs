using System;
using UnityEngine;

public class Item : IItem
{
    public int Id { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }
}