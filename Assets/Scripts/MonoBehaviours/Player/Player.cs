using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StrictBehaviour
{
    public IInventory Inventory;
    // Start is called before the first frame update
    void Start()
    {
        Inventory = GetRequiredComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
