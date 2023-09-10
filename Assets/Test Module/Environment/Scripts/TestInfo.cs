using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TestStatus
{
    None, Running, Passed, Failed
}
public class TestInfo : Singleton<TestInfo>
{
    private TextMeshProUGUI txTestStatus;
    private Blink testStatusBlinker;
    private TextMeshProUGUI txTestName;
    private Image imgResult;
    private bool initialized = false;

    public Sprite sprPending, sprSuccess, sprFail;

    void Start()
    {
        verifyInitialize();
    }

    void verifyInitialize()
    {
        if(initialized)
            return;

        var tfTestStatus = transform.FindRequired("txStatus");
        testStatusBlinker = tfTestStatus.GetRequiredComponent<Blink>();
        txTestStatus = tfTestStatus.GetRequiredComponent<TextMeshProUGUI>();
        txTestName = transform.FindRequired("txName").GetRequiredComponent<TextMeshProUGUI>();
        imgResult = transform.FindRequired("icResult").GetRequiredComponent<Image>();

        initialized = true;
    }

    private static string statusToString(TestStatus status) =>
    status switch {
        TestStatus.Running => "Running Test...",
        TestStatus.Passed => "Test PASSED",
        TestStatus.Failed => "Test FAILED",
        _ => throw new NotImplementedException()
    };

    private static Sprite statusToSprite(TestStatus status) =>
    status switch {
        TestStatus.Running => Instance.sprPending.Required(),
        TestStatus.Passed => Instance.sprSuccess.Required(),
        TestStatus.Failed => Instance.sprFail.Required(),
        TestStatus.None => Instance.sprPending.Required(),
        _ => throw new NotImplementedException()
    };

    public static void SetName(string name)
    {
        verifyInitialize();
        Instance.txTestName.text = name;
    } 

    public static void SetStatus(TestStatus status)
    {
        verifyInitialize();

        Instance.txTestStatus.text = statusToString(status);
        Instance.imgResult.sprite = statusToSprite(status);
    }
}
