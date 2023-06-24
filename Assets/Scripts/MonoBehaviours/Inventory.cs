using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<IItem> Items = new();
    public List<IWeaponItem> Fittings = new();
    public int MaxItems = 15;
    public int MaxFittings = 7;

    void Start()
    {

    }


    void Update()
    {

    }

    public void AddItem(IItem item)
    {
        if (Items.Count >= MaxItems)
            throw new ApplicationException("Inventory Full");

        Items.Add(item);
    }
}
