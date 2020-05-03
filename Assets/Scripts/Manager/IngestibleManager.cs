using UnityEngine;
using System.Collections.Generic;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          IngestibleManager class
 * ------------------------------------------------
 */

public class IngestibleManager : MonoBehaviour
{
    [HideInInspector]
    private static IngestibleManager _instance;

    private List<Ingestible> _ingestibles;

    private void Awake()
    {
        _instance = this;
        _instance._ingestibles = new List<Ingestible>();
    }

    private IngestibleManager() { }

    /**
     * Return the count of recorded T ingestibles
     */
    public static int IngestibleCount<T>()
    {
        int sum = 0;

        foreach (Ingestible i in _instance._ingestibles)
        {
            if (i is T)
            {
                sum++;
            }
        }

        return sum;
    }

    /**
     * Add the given ingestible to the record list
     */
    public static void AddIngestible(Ingestible i)
    {
        _instance._ingestibles.Add(i);
    }

    /**
     * Remove the given ingestible from the record list
     */
    public static void RemoveIngestible(Ingestible i)
    {
        _instance._ingestibles.Remove(i);
    }

    /**
     * Return the ingestible at the given index
     */
    public static Ingestible GetIngestible(int i)
    {
        return _instance._ingestibles[i];
    }
}
