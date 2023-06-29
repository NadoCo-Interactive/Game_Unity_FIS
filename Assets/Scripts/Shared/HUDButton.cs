using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip contextButtonHover;
    public AudioClip contextButtonClick;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        HUD.PlaySound(contextButtonHover);
    }

    public virtual void OnPointerClick(PointerEventData data)
    {
        HUD.PlaySound(contextButtonClick);
    }


}
