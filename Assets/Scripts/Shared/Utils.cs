using UnityEngine;
using System;

public static class Utils
{
    const string requiredErrorMessage = "\"{0}\" is required on {1}";

    public static T GetRequiredComponent<T>(this Component parent, string errorMessage = null) where T : Component
    {
        var component = parent?.GetComponent<T>();
        var objName = component.gameObject.name;
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, compName, objName));

        return component;
    }

    public static T GetRequiredComponent<T>(this GameObject parent, string errorMessage = null) where T : Component
    {
        var component = parent?.GetComponent<T>();
        var objName = component.gameObject.name;
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, compName, objName));

        return component;
    }

    public static T GetRequiredChildComponent<T>(this Component parent, string errorMessage = null) where T : Component
    {
        var component = parent?.GetComponentInChildren<T>();
        var objName = component.gameObject.name;
        var compName = (typeof(T)).FullName;

        if (component == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, compName, objName));

        return component;
    }

    public static Transform FindRequired(this Transform parent, string n, string errorMessage = null)
    {
        var transform = parent?.Find(n);
        var objName = parent.gameObject.name;

        if (transform == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, n, objName));

        return transform;
    }

    public static T Required<T>(this T obj, string errorMessage = null)
    {
        var name = (typeof(T)).FullName;

        if (obj == null)
            throw new NullReferenceException(errorMessage ?? string.Format(requiredErrorMessage, name));

        return obj;
    }

    public static Transform GetRootParent(this Transform transform)
    {
        const int maxRepeats = 10;
        int repeats = 0;
        var currentTransform = transform;

        while (currentTransform.parent != null && repeats < maxRepeats)
        {
            currentTransform = currentTransform.parent;
            repeats++;
        }

        return currentTransform;
    }
}