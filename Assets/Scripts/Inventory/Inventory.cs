using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory
{
    public class OnInventoryChangedEventArgs { public Inventory inventory; public Pickable lastAdded, lastRemoved; }
    public event EventHandler<OnInventoryChangedEventArgs> _onInventoryChanged;

    private Entity _entity;
    private List<Ingestible> _ingestibles;
    private List<Weapon> _weapons;
    private int _ingestibleMaxSize;
    private int _weaponMaxSize;

    private const int _INGESTIBLE_MAX_SIZE = 10;
    private const int _WEAPON_MAX_SIZE = 4;

    public Inventory(Entity entity, int ingestibleMaxSize = _INGESTIBLE_MAX_SIZE, int weaponMaxSize = _WEAPON_MAX_SIZE)
    {
        _entity = entity;
        _weapons = new List<Weapon>();
        _ingestibles = new List<Ingestible>();
        _weaponMaxSize = weaponMaxSize;
        _ingestibleMaxSize = ingestibleMaxSize;
    }

    /**
     * Return the owner of the inventory
     */
    public Entity GetEntity()
    {
        return _entity;
    }

    /**
     * Add the given item to the inventory
     */
    public bool Add(Pickable pickable)
    {
        if (pickable.GetScriptableItem().IsDefault()) return false;

        if (pickable is Weapon) 
        {
            if (_weapons.Count >= _weaponMaxSize) return false;

            _weapons.Add((Weapon) pickable);
            NotifySubscribers(pickable, null);

            return true;
        }
        else if (pickable is Ingestible)
        {
            if (_ingestibles.Count >= _ingestibleMaxSize) return false;

            _ingestibles.Add((Ingestible) pickable);
            NotifySubscribers(pickable, null);

            return true;
        }

        return false;
    }

    /**
     * Remove the given item from the inventory
     */
    public void Remove(Pickable pickable)
    {
        if (pickable is Weapon)
        {
            _weapons.Remove((Weapon) pickable);
            NotifySubscribers(null, pickable);
        }
        else if (pickable is Ingestible)
        {
            _ingestibles.Remove((Ingestible) pickable);
            NotifySubscribers(null, pickable);
        }
    }

    /**
     * Return the number of items in the inventory
     */
    public int GetInventorySize<T>()
    {
        if (typeof(T) == typeof(Weapon)) return _weapons.Count;
        else if (typeof(T) == typeof(Ingestible)) return _ingestibles.Count;
        throw new Exception("Unsupported generic type.");
    }

    /**
     * Return the item at the given index
     */
    public Pickable GetItemAt<T>(int index)
    {
        if (typeof(T) == typeof(Weapon)) return _weapons[index];
        else if (typeof(T) == typeof(Ingestible)) return _ingestibles[index];
        throw new Exception("Unsupported generic type.");
    }

    /**
     * Return the index of the given item
     */
    public int GetIndexOf(Pickable pickable)
    {
        if (pickable is Weapon) if (_weapons.Contains((Weapon) pickable)) return _weapons.IndexOf((Weapon) pickable);
        else if (pickable is Ingestible) if (_ingestibles.Contains((Ingestible) pickable)) return _ingestibles.IndexOf((Ingestible) pickable);
        throw new Exception("Unsupported pickable type.");
    }

    /**
     * Ask the subscribers to update themselves (i.e the inventory UI)
     */
    public void NotifySubscribers(Pickable lastAdded = null, Pickable lastRemoved = null)
    {
        _onInventoryChanged?.Invoke(this, new OnInventoryChangedEventArgs { 
            inventory = this, 
            lastAdded = lastAdded, 
            lastRemoved = lastRemoved 
        });
    }

    /**
     * Return a description of the inventory
     */
    public override string ToString()
    {
        string desc = "Weapons:\n";

        foreach (Weapon w in _weapons) desc += "- " + w.GetScriptableItem().GetName() + "\n";

        desc += "\nIngestibles:\n";

        foreach (Ingestible i in _ingestibles) desc += "- " + i.GetScriptableItem().GetName() + "\n";

        return desc;
    }
}
