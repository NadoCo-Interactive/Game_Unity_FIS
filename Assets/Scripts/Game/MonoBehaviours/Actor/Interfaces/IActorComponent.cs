using UnityEngine;

public interface IActorComponent
{
    public Actor Actor { get; }
    public bool enabled { get; set; }
}