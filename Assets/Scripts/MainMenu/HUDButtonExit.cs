using UnityEngine.EventSystems;
using UnityEngine;

public class HUDButtonExit : HUDButton, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData data)
    {
        Application.Quit();
        base.OnPointerClick(data);
    }
}