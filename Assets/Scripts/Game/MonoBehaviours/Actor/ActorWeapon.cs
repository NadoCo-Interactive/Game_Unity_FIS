using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { FullAuto, SemiAuto }

public class ActorWeapon : ActorComponent, IActorWeapon
{
    public IWeaponItem ActiveWeapon { get; set; }

    private bool initialized = false;

    public List<WeaponHardpoint> Hardpoints { get; set; }

    protected virtual void Start()
    {
        VerifyInitialize();
    }

    private void VerifyInitialize()
    {
        if (initialized)
            return;

        Hardpoints = GetComponentsInChildren<WeaponHardpoint>().ToList();

        initialized = true;
    }

    void Update()
    {

    }

    public virtual void Equip(IWeaponItem weaponItem)
    {
        ActiveWeapon = weaponItem;
    }

    public void EquipById(int id)
    {
        if (!Actor.Inventory.HasFittingForId(id))
            return;

        var weaponItem = InventoryManager.SelectedInventory.Fittings.FirstOrDefault(f => f.Id == id);

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
