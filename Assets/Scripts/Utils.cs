using UnityEngine;
using System;

public static class Utils
{
    const string requiredErrorMessage = "\"{0}\" is required";

    public static T GetRequiredComponent<T>(this Component parent, string errorMessage = null) where T : Component
    {
        var component = parent.GetComponent<T>();
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, compName));

        return component;
    }

    public static T GetRequiredChildComponent<T>(this Component parent, string errorMessage = null) where T : Component
    {
        var component = parent.GetComponentInChildren<T>();
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, compName));

        return component;
    }

    public static Transform FindRequired(this Transform parent, string n, string errorMessage = null)
    {
        var transform = parent.Find(n);

        if (transform == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, n));

        return transform;
    }

    public static T Required<T>(this T obj, string errorMessage = null)
    {
        var name = (typeof(T)).FullName;

        if (obj == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, name));

        return obj;
    }
}