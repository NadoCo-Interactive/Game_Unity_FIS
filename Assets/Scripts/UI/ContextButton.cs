using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextButton : MonoBehaviour, IPointerEnterHandler
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
        Debug.Log("hello");
        _audioSource.clip = contextButtonHover;
        _audioSource.Play();

    }

    public void OnPointerDown(PointerEventData data)
    {
        _audioSource.clip = contextButtonClick;
        _audioSource.Play();
    }


}
