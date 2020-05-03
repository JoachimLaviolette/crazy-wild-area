using UnityEngine;
using System;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Throwable class
 * ------------------------------------------------
 */

public abstract class Throwable : Weapon
{
    protected float _throwSpeed;
    protected float _impulseForce;
    protected bool _isThrown;
    
    /**
     * Setup the shooting weapon
     */
    protected virtual void Setup()
    {
        _isThrown = false;        
    }

    /**
     * Add the picked weapon to the interacting entity inventory
     */
    protected override void Pickup(Focuser focuser)
    {
        if (!_isThrown && !_isCarried)
        {
            base.Pickup(focuser);
            Destroy(GetComponent<Rigidbody>());
        }
    }

    /**
     * Use the weapon
     */
    public override void Use(Func<Vector3> GetTargetPosition)
    {
        if (_isCurrent) Throw(GetTargetPosition);
    }

    /**
     * Get thrown to the target position
     */
    public virtual bool Throw(Func<Vector3> GetTargetPosition)
    {
        if (_focuser is ICanThrow == false) return false;

        _impulseForce = ((ICanThrow) _focuser).GetImpulseForce();

        return true;
    }
}
