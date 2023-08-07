
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public void Play()
    {
        var typewriter = transform.FindRequired("Text").GetRequiredComponent<Typewriter>();
        typewriter.Type("You Died", false);
    }
}
