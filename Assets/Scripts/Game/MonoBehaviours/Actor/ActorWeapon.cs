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
        verifyInitialize();
    }

    private void verifyInitialize()
    {
        if (initialized)
            return;

        initializeHardpoints();

        initialized = true;
    }

    private void initializeHardpoints()
    {
        Hardpoints = GetComponentsInChildren<ActorHardpoint>().ToList();

        if (Actor.Network == null)
            return;
        
        if(Actor.Network.IsLocalPlayer)
        {
            foreach(var hardpoint in Hardpoints)
                hardpoint.Id = Guid.NewGuid().ToUlong();

            GameLog.Log("sent hardpoint setting packet with ids "+string.Join(",",Hardpoints.Select(hp => hp.Id)));
            Actor.Network.SetHardpointIdsServerRpc(Hardpoints.Select(hp => hp.Id).ToArray());
        }
        else
        {
            foreach(var hardpoint in Hardpoints)
            {
                var hardpointId = Actor.Network.HardpointIds[Hardpoints.IndexOf(hardpoint)];
                hardpoint.Id = hardpointId;
            }
        }
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
