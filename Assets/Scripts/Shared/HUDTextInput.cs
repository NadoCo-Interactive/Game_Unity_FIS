using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDTextInput : StrictBehaviour
{
    public AudioClip typingAudioClip;
    private AudioSource _audioSource;
    private TMP_InputField _inputField;
    private bool _isTyping;
    private float _typingTimer = 0;

    void Start()
    {
        _inputField = GetRequiredComponent<TMP_InputField>();
        _audioSource = GetRequiredComponent<AudioSource>();

        // Hook up events
        _inputField.onValueChanged.AddListener(OnValueChanged);
        _inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        DoTimer();
        DoSoundFade();
    }

    void DoTimer()
    {
        if (_typingTimer > 0)
            _typingTimer -= Time.deltaTime * 100;
        else
            OnEndEdit(null);
    }

    void DoSoundFade()
    {
        if (!_isTyping)
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, _audioSource.volume * .9f, Time.deltaTime * 5000);
        else
            _audioSource.volume = 1;
    }

    protected virtual void OnValueChanged(string value)
    {
        if (!_isTyping && value != string.Empty)
        {
            _audioSource.clip = typingAudioClip;
            _audioSource.Play();
            _isTyping = true;
        }

        _typingTimer = 25;
    }

    private void OnEndEdit(string value)
    {
        _isTyping = false;
    }
}
