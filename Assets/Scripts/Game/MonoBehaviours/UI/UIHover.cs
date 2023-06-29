using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsHover { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHover = false;
    }
}