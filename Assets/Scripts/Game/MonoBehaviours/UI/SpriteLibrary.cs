using UnityEngine;
using System;

public class SpriteLibrary : Singleton<SpriteLibrary>
{
    [SerializeField]
    private Sprite _particleBlasterWeaponSprite;

    public static Sprite ParticleBlasterWeaponSprite => Instance._particleBlasterWeaponSprite;
}