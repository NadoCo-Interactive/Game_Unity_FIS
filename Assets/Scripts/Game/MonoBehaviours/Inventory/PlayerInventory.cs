public class PlayerInventory : ActorInventory
{
    new void Start()
    {
        if(!Actor.IsLocalPlayer)
            return;

        var particleBlaster = ItemManager.CreateItem(ItemType.ParticleBlaster);
        AddItem(particleBlaster);

        InventoryManager.SetSelectedInventory(this);
        base.Start();
    }
}