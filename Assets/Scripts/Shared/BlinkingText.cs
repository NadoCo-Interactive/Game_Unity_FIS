using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingText : StrictBehaviour
{
    public bool isActive = false;
    public bool stayVisibleWhenInactive = false;
    public float fadeSpeed = 4;

    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _statusText;
    private float fader = 0;
    private bool initialized = false;

    public virtual void Start()
    {
        verifyInitialize();
    }

    public virtual void verifyInitialize()
    {
        if (initialized)
            return;

        _canvasGroup = GetRequiredComponent<CanvasGroup>();
        _statusText = GetRequiredComponent<TextMeshProUGUI>();

        initialized = true;
    }

    public virtual void Update()
    {
        if (isActive)
        {
            _canvasGroup.alpha = ((Mathf.Sin(fader) * 1) + 1) / 2;
            fader += Time.deltaTime * fadeSpeed;
        }
        else
        {
            var alphaMultiplier = _canvasGroup.alpha * (stayVisibleWhenInactive ? 1.1f : .9f);
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, alphaMultiplier, Time.deltaTime * fadeSpeed * 10);
            fader = 0;
        }
    }

    public void SetActive(bool setActive)
    {
        isActive = setActive;
    }
}
