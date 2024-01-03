using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public enum FireMode { FullAuto, SemiAuto }

public class ActorWeapon : ActorComponent, IActorWeapon
{
    public IWeaponItem ActiveWeapon { get; set; }

    private bool initialized = false;

    public List<ActorHardpoint> Hardpoints { get; set; } = new List<ActorHardpoint>();

    protected virtual void Start()
    {
        VerifyInitialize();
    }

    public void VerifyInitialize()
    {
        if (initialized)
            return;

        Hardpoints = GetComponentsInChildren<ActorHardpoint>().ToList();

        foreach(var hardpoint in Hardpoints)
        {
            var idGuid = Guid.NewGuid().ToByteArray();
            var idULong = BitConverter.ToUInt64(idGuid);
            hardpoint.Id = idULong;
        }

        if(CanUseNetwork)
        {
            Debug.Log("sent hardpoint setting packet with ids "+string.Join(",",Hardpoints.Select(hp => hp.Id)));
            Actor.Network.SetHardpointIdsServerRpc(Hardpoints.Select(hp => hp.Id).ToArray());
        }


        initialized = true;
    }

    public virtual void Equip(IWeaponItem weaponItem)
    {
        ActiveWeapon = weaponItem;
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
