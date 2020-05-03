using UnityEngine;
using System;
using JoaDev;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          BurstRifle class
 * ------------------------------------------------
 */

public class BurstRifle : Rifle
{
    protected override void Start()
    {
        base.Start();
        Setup();
    }

    /**
     * Setup the burst rifle
     */
    protected override void Setup()
    {
        transform.localScale = transform.localScale * 5f;
        _bulletSpeed = 80f;
        _shootFrequency = 3;
        _bulletShotDeltaTime = .05f;
        _shootDeltaTime = .8f;
        _damages = 12.5f;
        _slotCount = 3;
        _slotCapacity = 9;
        _reloadTime = 1.5f;
        base.Setup();
    }

    /**
     * Shoot the target position
     */
    public override bool Shoot(Func<Vector3> GetTargetPosition)
    {
        if (!base.Shoot(GetTargetPosition)) return false;

        int bulletCount = 0;
        string fctPeriodicName = "BustRifleShot";

        FunctionPeriodic.Create(
            () =>
            {
                _isShooting = true;
                Bullet burstRifleBullet = Instantiate(
                    AssetManager.Get_Prefab_BurstRifleBullet(),
                    _shootingZone.position,
                    transform.rotation
                );

                burstRifleBullet.Setup(GetTargetPosition(), _bulletSpeed, _damages, _focuser);
                bulletCount++;
                HandleAmmo();

                // Trigger burst sound
                SoundManager.PlayShotSound(SoundManager.ShotSoundType.BURST, transform.position, .6f);
            },
            fctPeriodicName,
            0f,
            _bulletShotDeltaTime,
            () =>
            {
                if (bulletCount >=_shootFrequency)
                {
                    StartCoroutine(StartShootDeltaTime());
                    Destroy(GameObject.Find(fctPeriodicName)); 
                }
            }
        );

        return true;
    }
}
