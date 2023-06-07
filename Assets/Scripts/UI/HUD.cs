using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private RectTransform rectHUDFade;
    private RectTransform rectEquipped;
    private RectTransform rectInventory;
    private Inventory _inventory;

    public bool InventoryIsVisible = false;
    public bool IsLocked = false;

    private static HUD _instance;
    public static HUD Instance => _instance;

    // Start is called before the first frame update
    void Start()
    {
        rectHUDFade = transform.Find("HUDFade")?.GetComponent<RectTransform>();
        rectEquipped = transform.Find("Equipped")?.GetComponent<RectTransform>();
        rectInventory = transform.Find("Inventory")?.GetComponent<RectTransform>();

        _inventory = transform.Find("Inventory")?.GetComponent<Inventory>();
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryIsVisible = !InventoryIsVisible;
            IsLocked = InventoryIsVisible;

            if (InventoryIsVisible)
            {
                rectHUDFade.gameObject.SetActive(true);
                rectInventory.gameObject.SetActive(true);
                rectEquipped.gameObject.SetActive(false);
                _inventory.Show();
            }
            else
            {
                _inventory.Hide();
                rectHUDFade.gameObject.SetActive(false);
                rectInventory.gameObject.SetActive(false);
                rectEquipped.gameObject.SetActive(true);
            }


        }
    }
}
