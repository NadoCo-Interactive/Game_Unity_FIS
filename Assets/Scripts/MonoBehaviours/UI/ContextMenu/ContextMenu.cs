using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum ContextMenuMode
{
    Fit, Unfit, NoFit
}
public class ContextMenu : Singleton<ContextMenu>, IPointerExitHandler, IPointerEnterHandler
{
    public AudioClip contextOpen;
    private RectTransform _rect;
    private List<GameObject> _children;

    private Button _btnEquip, _btnDrop;

    private bool _isHover = false;
    private bool _initialized = false;

    public static ContextMenuMode Mode { get; private set; }

    void Start()
    {
        VerifyInitialize();
    }

    void VerifyInitialize()
    {
        if (_initialized)
            return;

        _rect = GetRequiredComponent<RectTransform>();
        _children = GetComponentsInChildren<RectTransform>(true).Select(t => t.gameObject).ToList();
        _btnEquip = transform.Find("btnEquip").GetRequiredComponent<Button>();
        _btnDrop = transform.Find("btnDrop").GetRequiredComponent<Button>();

        _initialized = true;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !_isHover)
            Close();
    }

    public static void Open(ContextMenuMode mode = ContextMenuMode.Fit)
    {
        Instance._children.ForEach(c => c.SetActive(true));

        var rectSize = Instance._rect.sizeDelta;
        var rectScreen = Camera.main.ScreenToWorldPoint(new Vector3(rectSize.x, rectSize.y, 0));
        Instance._rect.position = Input.mousePosition - rectScreen;

        var btnEquipText = Instance._btnEquip.GetRequiredChildComponent<TextMeshProUGUI>();

        if (mode == ContextMenuMode.Fit)
            btnEquipText.text = "Equip";
        else if (mode == ContextMenuMode.Unfit)
            btnEquipText.text = "Un-Equip";
        else if (mode == ContextMenuMode.NoFit)
            Instance._btnEquip.interactable = false;

        Mode = mode;
    }

    public void DoEquip()
    {
        if (Mode == ContextMenuMode.Fit)
        {
            InventoryManager.ActiveInventory.AddFitting(InventoryManager.SelectedItem);
            InventoryManager.ActiveInventory.RemoveItem(InventoryManager.SelectedItem);
        }
        else if (Mode == ContextMenuMode.Unfit)
        {
            InventoryManager.ActiveInventory.RemoveFitting(InventoryManager.SelectedItem);
            InventoryManager.ActiveInventory.AddItem(InventoryManager.SelectedItem);
        }

        InventoryRenderer.UpdateUI();
        InventoryManager.SelectedItem = null;
        Close();
    }

    public void DoDrop()
    {
        Debug.Log("dropped item");
        Close();
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
