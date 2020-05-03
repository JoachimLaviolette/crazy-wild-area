using UnityEngine;
using System.Collections.Generic;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          EnvironmentManager class
 * ------------------------------------------------
 */

public class EnvironmentManager : MonoBehaviour
{
    [HideInInspector]
    private static EnvironmentManager _instance;

    private List<EnvironmentComponent> _environmentComponents;

    private void Awake()
    {
        _instance = this;
        _instance._environmentComponents = new List<EnvironmentComponent>();
    }

    private EnvironmentManager() { }

    /**
     * Return the count of recorded T environment components
     */
    public static int ComponentCount<T>()
    {
        int sum = 0;

        foreach (EnvironmentComponent i in _instance._environmentComponents)
        {
            if (i is T)
            {
                sum++;
            }
        }

        return sum;
    }

    /**
     * Add the given environment component to the record list
     */
    public static void AddComponent(EnvironmentComponent i)
    {
        _instance._environmentComponents.Add(i);
    }

    /**
     * Remove the given environment component from the record list
     */
    public static void RemoveComponent(EnvironmentComponent ec)
    {
        _instance._environmentComponents.Remove(ec);
    }

    /**
     * Return the environment component at the given index
     */
    public static EnvironmentComponent GetComponent(int i)
    {
        return _instance._environmentComponents[i];
    }
}
