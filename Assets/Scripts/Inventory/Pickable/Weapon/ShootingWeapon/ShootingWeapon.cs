using UnityEngine;
using System;
using System.Collections;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          ShootingWeapon class
 * ------------------------------------------------
 */

public abstract class ShootingWeapon : Weapon
{
    [SerializeField]
    protected Transform _shootingZone;
    protected int _shootFrequency; // How many bullets the weapon can shoot in a sec
    protected float _bulletSpeed;
    protected float _bulletShotDeltaTime; // How much time in a burst between each bullet shot
    protected float _shootDeltaTime; // How much time between each shot action

    protected int _maxAmmoCapacity; // The max capacity of the weapon
    protected int _globalAmmoCount; // How many ammos in total
    protected int _slotCapacity; // How many ammos per slot
    protected int _slotCount; // How many slots
    protected int _currentSlot; // The currently active slot
    protected bool _isShooting;
    protected bool _isReloading;
    protected float _reloadTime;
    
    /**
     * Setup the shooting weapon
     */
    protected virtual void Setup()
    {
        _currentSlot = _slotCount;
        _maxAmmoCapacity = _slotCapacity * _slotCount;
        _globalAmmoCount = _maxAmmoCapacity;
        _isShooting = false;
        _isReloading = false;
    }

    /**
     * Use the weapon
     */
    public override void Use(Func<Vector3> GetTargetPosition)
    {
        if (_isCurrent) Shoot(GetTargetPosition);
    }

    /**
     * Shoot the target position
     */
    public virtual bool Shoot(Func<Vector3> GetTargetPosition)
    {
        if (_focuser is ICanUseWeapon == false) return false;
        if (_isShooting) return false;
        if (_isReloading) return false;
        if (_globalAmmoCount == 0)
        {
            SoundManager.PlayShotSound(SoundManager.ShotSoundType.NO_AMMO, transform.position, .35f);

            return false;
        }

        return true;
    }

    /**
     * Handle ammunition
     */
    protected void HandleAmmo()
    {
        _globalAmmoCount--;
        if (_currentSlot > 0 && _globalAmmoCount % _slotCapacity == 0) StartCoroutine(Reload());        
        NotifySubscribers();
    }

    /**
     * Reload the weapon
     */
    public IEnumerator Reload()
    {
        _isReloading = true;
        SoundManager.PlayReloadSound(transform.position);
        yield return new WaitForSeconds(AssetManager.Get_Clip_ReloadSound().length * _reloadTime);
        _currentSlot--;
        _isReloading = false;
    }

    /**
     * Wait a certain amount of time before being able to shoot again
     */
    protected IEnumerator StartShootDeltaTime()
    {
        yield return new WaitForSeconds(_shootDeltaTime);
        _isShooting = false;
    }

    /**
     * Set the weapon's reloading state
     */
    public void SetIsReloading(bool isReloading)
    {
        _isReloading = isReloading;
    }

    /**
     * Return the count of ammo in the current slot
     */
    public int GetCurrentSlotAmmoCount()
    {
        if (_globalAmmoCount == 0) return _globalAmmoCount;
        return _globalAmmoCount % _slotCapacity == 0 ? _slotCapacity : _globalAmmoCount % _slotCapacity;
    }

    /**
     * Return the current slot
     */
    public int GetCurrentSlot()
    {
        if (_currentSlot == 0) return _currentSlot + 1;
        if (_globalAmmoCount == 0) return _currentSlot;
        if (_globalAmmoCount == _maxAmmoCapacity) return _currentSlot;
        if (_globalAmmoCount % _slotCapacity == 0 
            && _currentSlot > 1 
            && _currentSlot < _slotCapacity) return _currentSlot - 1;

        return _currentSlot;
    }

    /**
     * Return the slot count
     */
    public int GetSlotCount()
    {
        return _slotCount;
    }

    /**
     * Return the current global ammo count
     */
    public int GetCurrentGlobalAmmoCount()
    {
        return _globalAmmoCount;
    }

    /**
     * Return slot capacity
     */
    public int GetSlotCapacity()
    {
        return _slotCapacity;
    }

    /**
     * Return max ammo capacity
     */
    public int GetMaxAmmoCapacity()
    {
        return _maxAmmoCapacity;
    }
}