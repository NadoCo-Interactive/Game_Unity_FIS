
using System;
using UnityEngine;

public interface IItem
{
    public string Id {get;set;}
    public int SlotId { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject PrefabInstance { get; set; }
    public Sprite Sprite { get; set; }
    public string Name { get; set; }
}