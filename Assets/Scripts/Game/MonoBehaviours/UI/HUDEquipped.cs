using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDEquipped : Singleton<HUDEquipped>
{
    private Image _image;
    private Text _text;
    private IWeaponItem _activeWeapon;
    public static IWeaponItem ActiveWeapon => Instance._activeWeapon;

    void Start()
    {
        _image = transform.Find("Image").Required().GetRequiredComponent<Image>();
        _text = transform.Find("Text").Required().GetRequiredComponent<Text>();

        SetWeapon(null);
    }

    public static void SetWeapon(IWeaponItem weaponItem)
    {
        Instance._image.enabled = weaponItem != null;
        Instance._text.text = weaponItem?.Name ?? "No Weapon";
        Instance._image.sprite = weaponItem?.Sprite;

        Instance._activeWeapon = weaponItem;
    }
}
