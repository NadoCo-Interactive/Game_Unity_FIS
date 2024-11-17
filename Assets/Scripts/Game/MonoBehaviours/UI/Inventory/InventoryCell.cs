using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : StrictBehaviour
{
    private RectTransform rect;
    private Button button;
    private Image image, itemImage;
    private TextMeshProUGUI idText, ammoText;
    private AudioSource audioSource;
    private Vector2 endSize, startSize;
    private UIHover _uiHover;

    private bool isVisible = false;
    private bool isInitialized = false;

    public int Id = 0;
    public IItem Item { get; private set; }

    void Start()
    {
        VerifyInitialize();
        Hide();
    }

    void VerifyInitialize()
    {
        if (isInitialized) return;

        rect = GetRequiredComponent<RectTransform>();
        button = GetRequiredComponent<Button>();
        image = GetRequiredComponent<Image>();
        itemImage = transform.FindRequired("itemImage").GetComponent<Image>();
        audioSource = GetRequiredComponent<AudioSource>();

        idText = transform.FindRequired("id").GetRequiredComponent<TextMeshProUGUI>();
        ammoText = transform.FindRequired("ammo").GetRequiredComponent<TextMeshProUGUI>();

        _uiHover = GetRequiredComponent<UIHover>();

        endSize = rect.localScale;
        startSize = rect.localScale * 1.15f;

        isInitialized = true;
    }

    public void OnClick()
    {
        Show();
        button.OnDeselect(null);
    }

    void Update()
    {
        if (isVisible)
            rect.localScale = Vector2.Lerp(rect.localScale, endSize, Time.deltaTime * 10);

        if (Input.GetMouseButtonDown(1) && _uiHover.IsHover && Item != null)
        {
            if (InventoryManager.SelectedInventory.HasFittedItem(Item))
                HUDContextMenu.Open(ContextMenuMode.Unfit);
            else if (!(Item is IWeaponItem))
                HUDContextMenu.Open(ContextMenuMode.NoFit);
            else
                HUDContextMenu.Open();

            InventoryManager.SelectedItem = Item;
        }
    }

    public void SetItem(IItem item)
    {
        VerifyInitialize();

        itemImage.enabled = item != null && isVisible;
        itemImage.sprite = item?.Sprite.Required();

        var itemIsWeapon = item is WeaponItem;
        idText.enabled = item != null && itemIsWeapon && isVisible;
        ammoText.enabled = item != null && itemIsWeapon && isVisible;

        if (itemIsWeapon)
        {
            var itemAsWeapon = item as WeaponItem;
            var itemIsFitted = InventoryManager.SelectedInventory.HasFittedItem(item);
            idText.text = itemIsFitted ? itemAsWeapon?.SlotId.ToString() ?? "" : "";
            ammoText.text = "∞";
        }

        Item = item;
    }

    public void Show()
    {
        VerifyInitialize();

        image.enabled = true;
        itemImage.enabled = Item != null;
        idText.enabled = Item != null && Item is WeaponItem;
        ammoText.enabled = Item != null && Item is WeaponItem;

        rect.localScale = startSize;
        button.interactable = true;
        isVisible = true;

        audioSource.Play();
    }

    public void Hide()
    {
        VerifyInitialize();

        image.enabled = false;
        itemImage.enabled = false;
        idText.enabled = false;
        ammoText.enabled = false;

        button.interactable = false;
        isVisible = false;
    }

    public bool GetIsVisible() => isVisible;


}
