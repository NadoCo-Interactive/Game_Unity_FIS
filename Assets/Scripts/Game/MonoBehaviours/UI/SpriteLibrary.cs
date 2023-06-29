using UnityEngine;
using System;

public class SpriteLibrary : MonoBehaviour
{
    private static SpriteLibrary _instance;
    [SerializeField]
    private Sprite _particleBlasterWeaponSprite;
    public static Sprite ParticleBlasterWeaponSprite => _instance._particleBlasterWeaponSprite;

    void Start()
    {
        if (_instance != null)
            throw new ApplicationException("Only one SpriteLibrary is allowed");

        _instance = this;
    }
}