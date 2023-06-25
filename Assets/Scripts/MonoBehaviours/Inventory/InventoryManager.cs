using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public static Inventory SelectedInventory { get; private set; }
    public static IItem SelectedItem;

    public static void SetSelectedInventory(Inventory inventory)
    {
        SelectedInventory = inventory;
        InventoryRenderer.UpdateUI();
    }
}