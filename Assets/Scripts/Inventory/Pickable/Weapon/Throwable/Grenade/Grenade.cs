using UnityEngine;
using System;
using JoaDev;

public class Grenade : Throwable
{
    private const float _DEFAULT_MASS = 1f;

    protected override void Start()
    {
        base.Start();
        Setup();
    }

    /**
     * Setup the grenade
     */
    protected override void Setup()
    {
        transform.localScale = transform.localScale;
        _throwSpeed = 10f;
        _damages = 12.5f;
        base.Setup();
    }

    /**
     * Shoot the target position
     */
    public override bool Throw(Func<Vector3> GetTargetPosition)
    {
        if (!base.Throw(GetTargetPosition)) return false;

        ICanThrow thrower = (ICanThrow) _focuser;
        string fctPeriodicName = "GrenadeThrowing";

        FunctionPeriodic.Create(
            () =>
            {
                _isThrown = true;
                transform.SetParent(null);

                Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.mass = _DEFAULT_MASS;
                rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rigidbody.AddForce((GetTargetPosition() - transform.position + (Vector3.up) * 10f).normalized * _impulseForce, ForceMode.Impulse);

                if (_focuser is Entity e)
                {
                    e.GetInventory().Remove(this);
                    e.Unequip(this);
                }

                // Trigger throw sound
                SoundManager.PlayThrowSound(transform.position, .6f);
            },
            fctPeriodicName,
            0f,
            0f,
            () =>
            {
                Destroy(GameObject.Find(fctPeriodicName));
            }
        );

        return true;
    }
}
