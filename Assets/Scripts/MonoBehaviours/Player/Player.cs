using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public IInventory Inventory;
    // Start is called before the first frame update
    void Start()
    {
        Inventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
