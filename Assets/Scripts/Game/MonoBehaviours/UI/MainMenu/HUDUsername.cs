using UnityEngine;

public class HUDUsername : HUDTextInput
{
    protected override void OnValueChanged(string value)
    {
        PlayerPrefs.SetString("username", value);
        base.OnValueChanged(value);
    }
}