using UnityEngine;

public class HUD : Singleton<HUD>
{
    private RectTransform _rectHUDFade;
    private RectTransform _rectEquipped;

    private AudioSource _audio;

    public bool InventoryIsVisible = false;
    public bool IsLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();

        _rectHUDFade = transform.Find("HUDFade")?.GetComponent<RectTransform>();
        _rectEquipped = transform.Find("Equipped")?.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
                ContextMenu.Close();
                _rectHUDFade.gameObject.SetActive(false);
                _rectEquipped.gameObject.SetActive(true);
            }
        }
    }

    public static void PlaySound(AudioClip clip)
    {
        Instance._audio.clip = clip;
        Instance._audio.Play();
    }
}
