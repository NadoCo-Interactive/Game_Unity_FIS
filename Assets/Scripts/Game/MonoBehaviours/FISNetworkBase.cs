using UnityEngine;

public class FISNetworkBase : MonoBehaviour
{
    public bool IsOwner { get; set; }
    public bool IsLocalPlayer { get; set; }
    public ulong OwnerClientId { get; set; }
}