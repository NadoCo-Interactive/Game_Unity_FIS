using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private RectTransform _rectHUDFade;
    private RectTransform _rectEquipped;
    private RectTransform _rectInventory;
    private RectTransform _rectContextMenu;
    private InventoryRenderer _inventory;

    public bool InventoryIsVisible = false;
    public bool IsLocked = false;

    private static HUD _instance;
    public static HUD Instance => _instance;

    // Start is called before the first frame update
    void Start()
    {
        _rectHUDFade = transform.Find("HUDFade")?.GetComponent<RectTransform>();
        _rectEquipped = transform.Find("Equipped")?.GetComponent<RectTransform>();
        _rectInventory = transform.Find("Inventory")?.GetComponent<RectTransform>();
        _rectContextMenu = transform.Find("ContextMenu")?.GetComponent<RectTransform>();

        _inventory = transform.Find("Inventory")?.GetComponent<InventoryRenderer>();
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
                _rectHUDFade.gameObject.SetActive(true);
                _rectInventory.gameObject.SetActive(true);
                _rectEquipped.gameObject.SetActive(false);
                _inventory.Show();
            }
            else
            {
                _inventory.Hide();
                _rectHUDFade.gameObject.SetActive(false);
                _rectInventory.gameObject.SetActive(false);
                _rectEquipped.gameObject.SetActive(true);
            }


        }
    }

    public void OpenContextMenu()
    {
        _rectContextMenu.gameObject.SetActive(true);
        _rectContextMenu.position = Input.mousePosition;
        Debug.Log("open context");
    }
}
