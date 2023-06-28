public class ActorComponent : StrictBehaviour, IActorComponent
{
    public Actor Actor { get; set; }

    protected void Start()
    {
        Actor = GetRequiredComponent<Actor>();
    }
}