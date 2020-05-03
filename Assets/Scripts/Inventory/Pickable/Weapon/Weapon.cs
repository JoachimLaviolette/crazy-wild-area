using UnityEngine;
using System;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Weapon class
 * ------------------------------------------------
 */

public abstract class Weapon : Pickable
{
    public class OnDataChangedEventArgs : EventArgs { public Weapon weapon; }
    public event EventHandler<OnDataChangedEventArgs> _onDataChanged;

    protected float _damages;
    protected bool _isCarried;
    protected bool _isCurrent;

    protected virtual void Start()
    {
        _isCarried = false;
        _isCurrent = false;
    }

    private void FixedUpdate()
    {
        if (_isCarried)
        {
            Destroy(GetComponent<Floatable>());
            Destroy(GetComponent<BoxCollider>());
        }
    }

    /**
     * Add the picked weapon to the interacting entity inventory
     */
    protected override void Pickup(Focuser focuser)
    {
        if (_isCarried) return;
        if (!_focuserInventory.Add(this)) return;

        gameObject.SetActive(false);
        _isCarried = true;
        base.Pickup(focuser);
    }

    /**
     * Set if the weapon is carried
     */
    public void SetCarried(bool isCarried)
    {
        _isCarried = isCarried;
    }

    /**
     * Set if the weapon owner is currently equipped of the weapon
     */
    public void SetCurrent(bool isCurrent)
    {
        _isCurrent = isCurrent;
    }

    /**
     * Return if the weapon owner is currently equipped of the weapon
     */
    public bool IsCurrent()
    {
        return _isCurrent;
    }

    /**
     * Use the weapon
     */
    public abstract void Use(Func<Vector3> GetTargetPosition);

    /**
     * Ask the subscribers to update themselves (i.e the inventory UI)
     */
    public void NotifySubscribers()
    {
        _onDataChanged?.Invoke(this, new OnDataChangedEventArgs
        {
            weapon = this
        });
    }
}
