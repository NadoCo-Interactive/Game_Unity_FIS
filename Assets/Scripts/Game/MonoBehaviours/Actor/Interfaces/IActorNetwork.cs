using UnityEngine;

public interface IActorNetwork
{
    public void SetRemotePositionServerRpc(Vector3 position);
    public void SetRemoteAimServerRpc(Vector3 rotation);
}