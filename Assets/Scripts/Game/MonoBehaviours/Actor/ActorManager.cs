using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActorManager : Singleton<ActorManager>
{
    public GameObject PlayerActorPrefab;

    void Start()
    {
        // .. spawn a test player to shoot at
        var player = SpawnPlayerActor(Vector3.forward * 50);
        player.transform.forward = -player.transform.forward;
    }

    public static Actor SpawnPlayerActor(Vector3 position)
    {
        if (Instance.PlayerActorPrefab == null)
            throw new ApplicationException("A player prefab is required");

        var player = GameObject.Instantiate(Instance.PlayerActorPrefab);
        player.transform.position = position;

        var actor = player.GetRequiredComponent<Actor>();

        return actor;
    }
}
