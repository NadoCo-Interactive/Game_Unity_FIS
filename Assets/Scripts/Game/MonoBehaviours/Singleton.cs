using UnityEngine;
using System.Linq;

///
/// <summary>Adding "Singleton" as a base class will allow one instance of a class to be accessible globally</summary>
///
public class Singleton<T> : StrictBehaviour where T : Object
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();

            var count = FindObjectsOfType<T>().Count();
            
            return _instance;
        }
    }
    private static T _instance;
}