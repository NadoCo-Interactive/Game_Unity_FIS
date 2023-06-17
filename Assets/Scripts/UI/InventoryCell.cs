using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    private RectTransform rect;
    private Button button;
    private Image image, itemImage;
    private AudioSource audioSource;
    private Vector2 endSize, startSize;
    private bool isVisible = false;
    private bool isInitialized = false;

    public IItem Item { get; private set; }

    void Start()
    {
        VerifyInitialize();
    }

    void VerifyInitialize()
    {
        if (isInitialized) return;

        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        itemImage = transform.Find("itemImage").GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        endSize = rect.localScale;
        startSize = rect.localScale * 1.15f;

        isInitialized = true;
        Hide();
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
    }

    public void SetItem(IItem item)
    {
        item = Item;
        itemImage.enabled = Item != null;
    }

    public void Show()
    {
        VerifyInitialize();
        rect.localScale = startSize;
        image.enabled = true;
        itemImage.enabled = Item != null;
        button.interactable = true;
        isVisible = true;
        audioSource.Play();
    }

    public void Hide()
    {
        VerifyInitialize();
        image.enabled = false;
        itemImage.enabled = false;
        button.interactable = false;
        isVisible = false;
    }

    public bool GetIsVisible() => isVisible;
}
