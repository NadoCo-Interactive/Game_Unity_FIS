using UnityEngine;

public class ActorComponent : StrictBehaviour, IActorComponent
{
    private Actor _actor;
    public Actor Actor
    {
        get
        {
            if (_actor == null)
                _actor = GetRequiredComponent<Actor>();

            return _actor;
        }
    }
}