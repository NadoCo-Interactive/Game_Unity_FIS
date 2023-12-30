public class PlayerInventory : ActorInventory
{
    new void Start()
    {
        var particleBlaster = ItemManager.CreateItem(ItemType.ParticleBlaster);
        AddItem(particleBlaster);

        InventoryManager.SetSelectedInventory(this);
        base.Start();
    }
}