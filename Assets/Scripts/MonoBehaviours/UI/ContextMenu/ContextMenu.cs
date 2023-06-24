using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenu : Singleton<ContextMenu>, IPointerExitHandler, IPointerEnterHandler
{
    public AudioClip contextOpen;
    private RectTransform _rect;
    private List<GameObject> _children;

    private Button btnEquip, btnDrop;

    private bool _isHover = false;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _children = GetComponentsInChildren<RectTransform>(true).Select(t => t.gameObject).ToList();
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !_isHover)
            Close();
    }

    public static void Open()
    {
        Instance._children.ForEach(c => c.SetActive(true));

        var rectSize = Instance._rect.sizeDelta;
        var rectScreen = Camera.main.ScreenToWorldPoint(new Vector3(rectSize.x, rectSize.y, 0));
        Instance._rect.position = Input.mousePosition - rectScreen;
    }

    public static void Close()
    {
        Instance._children.ForEach(c => c.SetActive(false));
    }

    public void OnPointerEnter(PointerEventData ev)
    {
        _isHover = true;
    }
    public void OnPointerExit(PointerEventData ev)
    {
        _isHover = false;
    }
}
