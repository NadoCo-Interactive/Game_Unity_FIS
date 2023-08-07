using UnityEngine.EventSystems;

public class HUDButtonRespawn : HUDButton, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData data)
    {
        base.OnPointerClick(data);
        HUD.ShowScreen(HUDScreen.GameScreen);
        SessionManager.ActiveSession.RespawnPlayer();
    }
}