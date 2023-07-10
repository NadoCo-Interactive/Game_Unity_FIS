using UnityEngine;

public enum HUDScreen
{
    GameScreen,
    DeathScreen
}

public class HUD : Singleton<HUD>
{
    private HUDScreen _activeScreen;

    private RectTransform _rectHUDFade;
    private RectTransform _rectGameScreen;
    private RectTransform _rectDeathScreen;
    private RectTransform _rectEquipped;

    private DeathScreen _deathScreen;

    public bool InventoryIsVisible = false;
    public bool IsLocked = false;
    public bool DisableInventory = false;

    void Start()
    {
        _rectHUDFade = transform.Find("HUDFade")?.GetRequiredComponent<RectTransform>();
        _rectGameScreen = transform.Find("GameScreen")?.GetRequiredComponent<RectTransform>();
        _rectEquipped = _rectGameScreen?.FindRequired("Equipped").GetRequiredComponent<RectTransform>();
        _rectDeathScreen = transform.Find("DeathScreen").GetRequiredComponent<RectTransform>();
        _deathScreen = _rectDeathScreen?.GetRequiredComponent<DeathScreen>();

        if (_rectGameScreen != null)
            ShowScreen(HUDScreen.GameScreen);
    }

    void Update()
    {
        if (_activeScreen == HUDScreen.GameScreen)
            DoInventory();
    }

    void DoInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !DisableInventory)
        {
            InventoryIsVisible = !InventoryIsVisible;
            IsLocked = InventoryIsVisible;

            if (InventoryIsVisible)
            {
                _rectHUDFade.gameObject.SetActive(true);
                _rectEquipped.gameObject.SetActive(false);
                InventoryRenderer.Show();
            }
            else
            {
                InventoryRenderer.Hide();
                HUDContextMenu.Close();
                _rectHUDFade.gameObject.SetActive(false);
                _rectEquipped.gameObject.SetActive(true);
            }
        }

        if (InventoryIsVisible && !_rectHUDFade.gameObject.activeSelf)
            _rectHUDFade.gameObject.SetActive(true);
    }

    public static void ShowScreen(HUDScreen screen)
    {
        if (screen == HUDScreen.GameScreen)
        {
            Instance._rectGameScreen.gameObject.SetActive(true);
            Instance._rectDeathScreen.gameObject.SetActive(false);
            Instance._rectHUDFade.gameObject.SetActive(Instance.InventoryIsVisible);
        }
        else if (screen == HUDScreen.DeathScreen)
        {
            Instance._rectGameScreen.gameObject.SetActive(false);
            Instance._rectDeathScreen.gameObject.SetActive(true);
            Instance._rectHUDFade.gameObject.SetActive(true);
            Instance._deathScreen.Play();
        }

        Instance._activeScreen = screen;
    }


}
