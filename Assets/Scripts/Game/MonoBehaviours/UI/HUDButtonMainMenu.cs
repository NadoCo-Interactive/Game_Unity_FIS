using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HUDButtonMainMenu : HUDButton, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData data)
    {
        SceneManager.LoadScene("MainMenu");
        base.OnPointerClick(data);
    }
}
