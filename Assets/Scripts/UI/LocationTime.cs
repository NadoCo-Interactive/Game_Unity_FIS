using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LocationTime : MonoBehaviour
{
  Typewriter locationName;
  Typewriter time;
  // Start is called before the first frame update
  void Start()
  {
    locationName = transform.Find("LocationName")?.GetComponent<Typewriter>();
    time = transform.Find("Time")?.GetComponent<Typewriter>();

    var currentTime = DateTime.Now.ToString("HH:MM");
    locationName.OnFinish += () =>
    {
      locationName.SetIsBlinking(false);
      locationName.SetCursorIsVisible(false);
      time.Type(currentTime + " Hours");
    };
    ShowTimeAndLocation();
  }

  void ShowTimeAndLocation()
  {
    locationName.Type("Sector 01", false);
    time.OnFinish += () =>
    {
      locationName.StartLifetime();
    };
  }

  // Update is called once per frame
  void Update()
  {

  }
}
