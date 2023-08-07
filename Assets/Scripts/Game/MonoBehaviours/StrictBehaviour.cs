using UnityEngine;
using System;

///
/// <summary>"StrictBehaviour" adds a host of extension methods, such as requiring a particular component to be attached to the object</summary>
///
public abstract class StrictBehaviour : MonoBehaviour
{
    const string requiredErrorMessage = "\"{0}\" is required on {1}";

    public T GetRequiredComponent<T>(string errorMessage = null) where T : Component
    {
        var component = GetComponent<T>();
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new ApplicationException(errorMessage ?? string.Format(requiredErrorMessage, compName, gameObject.name));

        return component;
    }

    public T GetRequiredComponentInChildren<T>(string errorMessage = null) where T : Component
    {
        var component = GetComponentInChildren<T>();
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new ApplicationException(errorMessage ?? string.Format(requiredErrorMessage, compName, gameObject.name));

        return component;
    }
}