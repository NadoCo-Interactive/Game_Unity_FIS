using UnityEngine;
using System.Linq;

public class Singleton<T> : MonoBehaviour where T : Object
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();

            var count = FindObjectsOfType<T>().Count();

            if (count > 1)
                Debug.LogWarning($"Only one object of type {(typeof(T)).FullName} should exist, found ({count})");

            return _instance;
        }
    }
    private static T _instance;
}