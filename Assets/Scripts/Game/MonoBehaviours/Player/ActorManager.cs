using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActorManager : Singleton<ActorManager>
{
    public GameObject PlayerPrefab;

    void Start()
    {

    }

    public static Actor SpawnPlayer(Vector3 position)
    {
        if(Instance.PlayerPrefab == null)
            throw new ApplicationException("A player prefab is required");

        var player = GameObject.Instiate(Instance.PlayerPrefab);
        player.transform.position = position;

        var actor = player.GetRequiredComponent<Actor>();

        return actor;
    }
}
