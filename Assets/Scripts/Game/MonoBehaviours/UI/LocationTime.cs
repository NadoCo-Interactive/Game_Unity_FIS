using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LocationTime : MonoBehaviour
{
    Typewriter locationName;
    Typewriter time;

    void Start()
    {
        locationName = transform.FindRequired("LocationName").GetRequiredComponent<Typewriter>();
        time = transform.FindRequired("Time").GetRequiredComponent<Typewriter>();

        var currentTime = DateTime.Now.ToString("HH:mm");
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
}
