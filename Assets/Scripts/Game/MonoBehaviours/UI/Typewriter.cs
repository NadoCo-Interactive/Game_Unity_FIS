using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DVoid();
public class Typewriter : MonoBehaviour
{
  // .. Typing
  private string targetText = "";
  private string currentText = "";
  private int currentTextIndex = 0;
  private bool isTyping = false;
  private AudioSource typingAudio;
  private Text typingText;
  private float startTypingRate = 100;
  private float typingRate = 50;
  private float typingTimer = 0;

  // .. Blinking
  private float cursorBlinkRate = 50;
  private float cursorBlinkTimer = 0;
  private bool _cursorIsVisible = false;
  private bool isBlinking = true;

  // .. Fading
  private float fadeRate = 10;
  private float lifetime = 0;
  private bool isAlive = false;
  private bool _fadeOnFinish = true;

  public DVoid OnFinish;
  private bool initialized = false;

  // Start is called before the first frame update
  void Start()
  {
    VerifyInitialize();
  }

  void VerifyInitialize()
  {
    if (initialized)
      return;

    typingAudio = GetComponent<AudioSource>();
    typingText = GetComponent<Text>();
    typingText.text = "";
    typingText.color = new Color(1, 1, 1, 0);
    initialized = true;
  }

  // Update is called once per frame
  void Update()
  {
    if (isTyping)
    {
      if (typingTimer > 0)
        typingTimer -= typingRate * 100 * Time.deltaTime;
      else
      {
        if (currentTextIndex < targetText.Length)
        {
          currentText += targetText[currentTextIndex];
          currentTextIndex++;
        }
        else
        {
          typingAudio.Stop();
          isTyping = false;
          isAlive = _fadeOnFinish;
          OnFinish?.Invoke();
        }

        typingRate = startTypingRate * Random.Range(.5f, 1.5f);
        typingTimer = 1000;
      }
    }

    if (isAlive)
    {
      if (lifetime > 0)
        lifetime -= Time.deltaTime * 100;
      else
      {
        var c = typingText.color;
        typingText.color = Color.Lerp(c, new Color(c.r, c.g, c.b, c.a * .9f), Time.deltaTime * fadeRate);
      }
    }

    if (isBlinking)
    {
      if (cursorBlinkTimer > 0)
        cursorBlinkTimer -= cursorBlinkRate * 100 * Time.deltaTime;
      else
      {
        _cursorIsVisible = !_cursorIsVisible;
        cursorBlinkTimer = 1000;
      }
    }

    typingText.text = currentText + (_cursorIsVisible ? "_" : "");
  }

  public void Type(string text, bool fadeOnFinish = true)
  {
    VerifyInitialize();
    isTyping = true;
    targetText = text;
    currentText = "";
    lifetime = 200;
    typingText.color = new Color(1, 1, 1, 1);
    typingAudio.Play();
    _fadeOnFinish = fadeOnFinish;
  }

  public void StartLifetime()
  {
    isAlive = true;
  }

  public void SetIsBlinking(bool blinking)
  {
    isBlinking = blinking;
  }

  public void SetCursorIsVisible(bool cursorIsVisible)
  {
    _cursorIsVisible = cursorIsVisible;
  }
}
