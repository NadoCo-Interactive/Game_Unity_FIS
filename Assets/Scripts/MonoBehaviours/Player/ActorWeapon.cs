using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { FullAuto, SemiAuto }

public class ActorWeapon : MonoBehaviour
{
    public IWeaponItem ActiveWeapon = null;

    private Transform shipTransform;
    private bool initialized = false;

    public List<WeaponHardpoint> Hardpoints { get; private set; }

    void Start()
    {
        VerifyInitialize();
    }

    void VerifyInitialize()
    {
        if (initialized)
            return;

        shipTransform = transform.Find("Ship")?.GetRequiredComponent<Transform>();

        Hardpoints = GetComponentsInChildren<WeaponHardpoint>().ToList();

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (HUD.Instance.IsLocked)
            return;

        DoEquip();
        DoAim();
        DoFire();

    }

    void DoEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ActiveWeapon = null;
            HUDEquipped.SetWeapon(null);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && InventoryManager.SelectedInventory.HasFittingForId(1))
        {
            var weaponItem = InventoryManager.SelectedInventory.Fittings.FirstOrDefault(f => f.Id == 1);

            if (weaponItem == null)
                return;

            ActiveWeapon = weaponItem;
            HUDEquipped.SetWeapon(weaponItem);
        }
    }

    void DoFire()
    {
        if (Input.GetMouseButton(0) && ActiveWeapon?.Weapon.fireMode == FireMode.FullAuto)
            ActiveWeapon?.Weapon.Fire();

        if (Input.GetMouseButtonDown(0) && ActiveWeapon?.Weapon.fireMode == FireMode.SemiAuto)
            ActiveWeapon?.Weapon.Fire();
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

                Vector3 targetDirection = worldPosition - shipTransform.position;
                shipTransform.forward = Vector3.Lerp(shipTransform.forward, targetDirection.normalized, Time.deltaTime * 10);

            }
        }
    }
}
