using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          EntityManager class
 * ------------------------------------------------
 */

public class EntityManager : MonoBehaviour
{
    public class OnPlayerCountChangedEventArgs : EventArgs { public float playerCount; }
    public static event EventHandler<OnPlayerCountChangedEventArgs> _onPlayerCountChanged;

    [HideInInspector]
    private static EntityManager _instance;

    private Entity _focusedEntity;
    private List<Entity> _entities;

    private void Awake()
    {
        _instance = this;
        _instance._focusedEntity = null;
        _instance._entities = new List<Entity>();
    }

    private EntityManager() { }

    /**
     * Return the focused entity
     */
    public static Entity GetFocusedEntity()
    {
        return _instance._focusedEntity;
    }

    /**
     * Set the focused entity
     */
    public static void SetFocusedEntity(Entity entity)
    {
        _instance._focusedEntity = entity;
    }

    /**
     * Return the number of recorded entities
     */
    public static int Count()
    {
        return _instance._entities.Count;
    }

    /**
     * Record a new entity
     */
    public static void Add(Entity entity)
    {
        _instance._entities.Add(entity);
        NotifySubscribers();
    }

    /**
     * Remove the given entity
     */
    public static void Remove(Entity entity)
    {
        _instance._entities.Remove(entity);
        NotifySubscribers();
    }

    /**
     * Return the entity at the specified index
     */
    public static Entity Get(int index)
    {
        return _instance._entities[index];
    }

    /**
     * Ask the subscribers to update themselves (i.e the scoring UI)
     */
    private static void NotifySubscribers()
    {
        _onPlayerCountChanged?.Invoke(_instance, new OnPlayerCountChangedEventArgs
        {
            playerCount = _instance._entities.Count
        });
    }
}
