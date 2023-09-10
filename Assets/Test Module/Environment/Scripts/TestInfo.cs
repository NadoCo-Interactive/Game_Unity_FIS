using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public enum TestStatus
{
    None, Running, Passed, Failed
}
public class TestInfo : Singleton<TestInfo>
{
    private TextMeshProUGUI txTestStatus;
    private Blink testStatusBlinker;
    public TextMeshProUGUI txTestName;

    void Start()
    {
        var tfTestStatus = transform.FindRequired("txStatus");
        testStatusBlinker = tfTestStatus.GetRequiredComponent<TextMeshProUGUI>();
        txTestStatus = tfTestStatus.GetRequiredComponent<TextMeshProUGUI>();
        txTestName = transform.FindRequired("txName").GetRequiredComponent<TextMeshProUGUI>();
    }

    public static void SetName(string name)
    {
        Instance.txTextName.text = name;
    } 

    private static string statusToString(TestStatus status) =>
    status switch {
        _ => throw new NotImplementedException()
    };

    public static void SetStatus(TestStatus status)
    {

    }
}
