using System.Collections;
using System.Collections.Generic;
using UI.Dialogs;
using UnityEngine;

public class Toast : Singleton<Toast>
{
    private uDialog_NotificationPanel notificationPanel;

    private bool initialized = false;

    void Start()
    {

    }

    void verifyInitialize()
    {
        if (initialized)
            return;

        notificationPanel = FindObjectOfType<uDialog_NotificationPanel>();

        initialized = true;
    }

    void Update()
    {

    }

    public static void Show(string message)
    {
        Instance.verifyInitialize();

        if(Instance.notificationPanel != null)
            Instance.notificationPanel.Required().AddNotification(message);
        else {
            Debug.LogWarning("No uDialog Notification Panel found - logging to the console instead");
            Debug.Log(message);
        }
    }
}
