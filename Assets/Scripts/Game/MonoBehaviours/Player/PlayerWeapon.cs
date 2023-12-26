using UnityEngine;

public class PlayerWeapon : ActorWeapon
{
    void Update()
    {
        if (HUD.Instance.IsLocked || Actor.IsRemote)
            return;

        DoEquip();
        DoAim();
        DoFire();
    }

    public override void Equip(IWeaponItem weaponItem)
    {
        base.Equip(weaponItem);
        HUDEquipped.SetWeapon(weaponItem);
    }

    void DoEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            Equip(null);

        if (Input.GetKeyDown(KeyCode.Alpha1) && InventoryManager.SelectedInventory.HasFittingForSlot(1))
            EquipBySlotId(1);
    }

    void DoFire()
    {
        if (Input.GetMouseButton(0) && ActiveWeapon?.Weapon.fireMode == FireMode.FullAuto)
            Fire();

        if (Input.GetMouseButtonDown(0) && ActiveWeapon?.Weapon.fireMode == FireMode.SemiAuto)
            Fire();
    }

    void DoAim()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
            if (plane.Raycast(ray, out distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                AimTo(worldPosition);
            }
        }
    }
}