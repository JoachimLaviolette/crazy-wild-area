using UnityEngine;
using System;
using JoaDev;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          HuntingRifle class
 * ------------------------------------------------
 */

public class HuntingRifle : Rifle
{
    protected override void Start()
    {
        base.Start();
        Setup();
    }

    /**
     * Setup the hunting rifle
     */
    protected override void Setup()
    {
        transform.localScale = transform.localScale * 10f;
        _bulletSpeed = 120f;
        _shootFrequency = 1;
        _bulletShotDeltaTime = 0f;
        _shootDeltaTime = 2f;
        _damages = 90f;
        _slotCount = 4;
        _slotCapacity = 1;
        _reloadTime = 1.8f;
        base.Setup();
    }

    /**
     * Shoot the target position
     */
    public override bool Shoot(Func<Vector3> GetTargetPosition)
    {
        if (!base.Shoot(GetTargetPosition)) return false;

        string fctPeriodicName = "HuntingRifleShot";

        FunctionPeriodic.Create(
            () =>
            {
                _isShooting = true;
                Bullet huntingRifleBullet = Instantiate(
                    AssetManager.Get_Prefab_HuntingRifleBullet(),
                    _shootingZone.position,
                    transform.rotation
                );

                huntingRifleBullet.Setup(GetTargetPosition(), _bulletSpeed, _damages, _focuser);
                HandleAmmo();

                // Trigger hunting sound
                SoundManager.PlayShotSound(SoundManager.ShotSoundType.HUNTING, transform.position, .6f);
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
