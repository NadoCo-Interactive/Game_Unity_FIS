using System;

public enum SessionType
{
    Fightclub,
    Conquest
}
public class SessionManager : Singleton<SessionManager>
{
    public static Session ActiveSession;

    void Start()
    {
        LoadSession(SessionType.Fightclub);
    }

    void Update()
    {

    }

    public static void LoadSession(SessionType sessionType)
    {
        if (sessionType == SessionType.Fightclub)
            ActiveSession = Instance.gameObject.AddComponent<FightclubSession>();
        else if (sessionType == SessionType.Conquest)
            throw new NotImplementedException();
    }
}
