using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public static ActorInventory SelectedInventory { get; private set; }
    public static IItem SelectedItem;

    public static void SetSelectedInventory(ActorInventory inventory)
    {
        SelectedInventory = inventory;
        InventoryRenderer.UpdateUI();
    }
}