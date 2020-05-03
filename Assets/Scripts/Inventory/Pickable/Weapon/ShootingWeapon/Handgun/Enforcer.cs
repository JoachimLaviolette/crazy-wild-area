using UnityEngine;
using System;
using JoaDev;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Enforcer class
 * ------------------------------------------------
 */

public class Enforcer : Handgun
{
    protected override void Start()
    {
        base.Start();
        Setup();
    }
    
    /**
     * Setup the enforcer
     */
    protected override void Setup()
    {
        transform.localScale = transform.localScale * 4f;
        _bulletSpeed = 110f;
        _shootFrequency = 1;
        _bulletShotDeltaTime = 0f;
        _shootDeltaTime = .1f;
        _damages = 33.3f;
        _slotCount = 4;
        _slotCapacity = 5;
        _reloadTime = 1f;
        base.Setup();
    }

    /**
     * Shoot the target position
     */
    public override bool Shoot(Func<Vector3> GetTargetPosition)
    {
        if (!base.Shoot(GetTargetPosition)) return false;
        string fctPeriodicName = "EnforcerShot";

        FunctionPeriodic.Create(
            () =>
            {
                _isShooting = true;
                Bullet enforcerBullet = Instantiate(
                    AssetManager.Get_Prefab_EnforcerBullet(),
                    _shootingZone.position,
                    transform.rotation
                );

                enforcerBullet.Setup(GetTargetPosition(), _bulletSpeed, _damages, _focuser);
                HandleAmmo();

                // Trigger enforcer sound
                SoundManager.PlayShotSound(SoundManager.ShotSoundType.ENFORCER, transform.position, .6f);
            },
            fctPeriodicName,
            0f,
            _bulletShotDeltaTime,
            () =>
            {
                StartCoroutine(StartShootDeltaTime());
                Destroy(GameObject.Find(fctPeriodicName));
            }
        );

        return true;
    }
}
