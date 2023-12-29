using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { FullAuto, SemiAuto }

public class ActorWeapon : ActorComponent, IActorWeapon
{
    public IWeaponItem ActiveWeapon { get; set; }

    private bool initialized = false;

    public List<ActorHardpoint> Hardpoints { get; set; }

    protected virtual void Start()
    {
        VerifyInitialize();
    }

    private void VerifyInitialize()
    {
        if (initialized)
            return;

        Hardpoints = GetComponentsInChildren<ActorHardpoint>().ToList();

        if(Actor.Network != null)
            Actor.Network.Hardpoints.Value = Hardpoints;

        initialized = true;
    }

    public virtual void Equip(IWeaponItem weaponItem)
    {
        ActiveWeapon = weaponItem;

        if(Actor.Network != null)
            Actor.Network.Hardpoints.Value = Hardpoints;
    }

    public void EquipBySlotId(int id)
    {
        if (!Actor.Inventory.HasFittingForSlot(id))
            return;

        var weaponItem = InventoryManager.SelectedInventory.Fittings.FirstOrDefault(f => f.SlotId == id);

        if (weaponItem == null)
            return;

        Equip(weaponItem);
    }

    public void Fire()
    {
        var weapon = ActiveWeapon?.Weapon.Required();
        weapon.Fire();
    }

    public void AimTo(Vector3 position)
    {
        var shipTransform = Actor.ShipTransform;
        Vector3 targetDirection = position - shipTransform.position;
        shipTransform.forward = Vector3.Lerp(shipTransform.forward, targetDirection.normalized, Time.deltaTime * 10);
    }
}
