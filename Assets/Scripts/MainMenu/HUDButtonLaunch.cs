using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDButtonLaunch : HUDButton, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Username=" + PlayerPrefs.GetString("username"));
        SceneManager.LoadScene("Game");
        base.OnPointerClick(data);
    }
}
