using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActorManager : Singleton<ActorManager>
{
    public GameObject PlayerActorPrefab;
    public GameObject PlayerPrefab;

    public static Actor SpawnPlayer(Vector3 position)
    {
        if (Instance.PlayerPrefab == null)
            throw new ApplicationException("A player prefab is required");

        var player = GameObject.Instantiate(Instance.PlayerPrefab);
        player.transform.position = position;

        var actor = player.GetRequiredComponent<Actor>();

        return actor;
    }

    public static Actor SpawnPlayerActor(Vector3 position)
    {
        if (Instance.PlayerActorPrefab == null)
            throw new ApplicationException("A player actor prefab is required");

        var player = GameObject.Instantiate(Instance.PlayerActorPrefab);
        player.transform.position = position;

        var actor = player.GetRequiredComponent<Actor>();

        return actor;
    }
}
