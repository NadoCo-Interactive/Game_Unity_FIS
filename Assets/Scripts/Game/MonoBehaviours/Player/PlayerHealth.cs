
public class PlayerHealth : PlayerActorHealth
{
    public override void Die()
    {
        base.Die();
        HUD.ShowScreen(HUDScreen.DeathScreen);
    }
}