using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ConnectionStatus { Disconnected, Connecting, Connected, ConnectionFailed }
public class ServerConnection : Singleton<ServerConnection>
{
    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _statusText;
    private float fader = 0;
    private bool isConnecting = false;
    private bool isActive = false;
    private float fadeoutTimer = 1000;
    private bool initialized = false;

    void Start()
    {
        verifyInitialize();
    }

    void verifyInitialize()
    {
        if (initialized)
            return;

        _canvasGroup = GetRequiredComponent<CanvasGroup>();
        _statusText = transform.FindRequired("Status").GetRequiredComponent<TextMeshProUGUI>();

        // DEBUG
        isConnecting = false;
        isActive = false;

        initialized = true;
    }

    void Update()
    {
        if (isActive)
        {
            if (isConnecting)
            {
                _canvasGroup.alpha = ((Mathf.Sin(fader) * 1) + 1) / 2;
                fader += Time.deltaTime * 4;
            }
            else
            {
                _canvasGroup.alpha = 1;
                fader = 0;
            }
        }
        else if (fadeoutTimer > 0)
        {
            fadeoutTimer -= Time.deltaTime * 300;
        }
        else
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _canvasGroup.alpha * .9f, Time.deltaTime * 10);
        }
    }

    public static void SetStatus(ConnectionStatus status, string statusDescription)
    {
        Instance.verifyInitialize();

        if (status == ConnectionStatus.Connecting)
        {
            Instance.isConnecting = true;
            Instance.fadeoutTimer = 1000;
        }
        else if (status == ConnectionStatus.Connected)
        {
            Instance.isConnecting = false;
            Instance.fadeoutTimer = 1000;
            Instance.isActive = false;
        }

        Instance._statusText.text = statusDescription;
    }
}
