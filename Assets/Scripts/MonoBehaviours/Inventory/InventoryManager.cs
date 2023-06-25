using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public static Inventory ActiveInventory { get; private set; }
    public static IItem SelectedItem;

    public static void SetActiveInventory(Inventory inventory)
    {
        ActiveInventory = inventory;
        InventoryRenderer.UpdateUI();
    }
}