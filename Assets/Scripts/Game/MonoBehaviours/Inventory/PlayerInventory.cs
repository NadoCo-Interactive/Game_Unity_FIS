public class PlayerInventory : ActorInventory
{
    new void Start()
    {
        var particleBlaster = ItemManager.CreateWeapon(WeaponType.ParticleBlaster);
        AddItem(particleBlaster);

        InventoryManager.SetSelectedInventory(this);
        base.Start();
    }
}