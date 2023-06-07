using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    private RectTransform rect;
    private Button button;
    private Image image;
    private AudioSource audioSource;


    private Vector2 endSize, startSize;
    private bool isVisible = false;

    private bool isInitialized = false;

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
        audioSource = GetComponent<AudioSource>();

        endSize = rect.sizeDelta;
        startSize = rect.sizeDelta * 1.15f;

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
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, endSize, Time.deltaTime * 10);
    }

    public void Show()
    {
        VerifyInitialize();
        rect.sizeDelta = startSize;
        image.enabled = true;
        button.interactable = true;
        isVisible = true;
        audioSource.Play();
    }

    public void Hide()
    {
        VerifyInitialize();
        image.enabled = false;
        button.interactable = false;
        isVisible = false;
    }

    public bool GetIsVisible() => isVisible;
}
