using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLog : Singleton<GameLog>
{
    private TextMeshProUGUI logText;
    private bool _initialized = false;

    void Start()
    {
        verifyInitialize();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            logText.text = "";
    }

    void verifyInitialize()
    {
        if(_initialized)
            return;

        logText = transform.Find("logText").GetRequiredComponent<TextMeshProUGUI>();

        _initialized = true;
    }

    public static void Log(string message)
    {
        Instance.verifyInitialize();
        Instance.logText.text = Instance.logText.text + "\n" + message;
    }

    public static void LogWarning(string message)
    {
        Log("[WARN] "+message);
    }

    public static void LogError(string message)
    {
        Log("[ERR] "+message);
    }
    
}