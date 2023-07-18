using UnityEngine;

public interface IActorNetwork
{
    public void SetRemotePositionServerRpc(Vector3 position);
    public void SetRemoteRotationServerRpc(Quaternion rotation);
}