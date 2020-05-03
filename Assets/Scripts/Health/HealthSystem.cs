using UnityEngine;
using System;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          HealthSystem class
 * ------------------------------------------------
 */

public class HealthSystem
{
    public class OnHealthChangedEventArgs : EventArgs { public float normalizedHealthAmount; public bool isLowHealth; }
    public event EventHandler<OnHealthChangedEventArgs> _onHealthChanged;
    public class OnArmorChangedEventArgs : EventArgs { public float normalizedArmorAmount; }
    public event EventHandler<OnArmorChangedEventArgs> _onArmorChanged;

    private float _health;
    private float _armor;
    private float _maxHealth;
    private float _maxArmor;

    public HealthSystem(float health, float maxHealth, float armor, float maxArmor)
    {
        _health = health;
        _maxHealth = maxHealth;
        _armor = armor;
        _maxArmor = maxArmor;
    }

    /**
     * Return the current health
     */
    public float GetHealth()
    {
        return _health;
    }

    /**
     * Return the normalized health
     */
    public float GetNormalizedHealth()
    {
        return _health / _maxHealth;
    }

    /**
     * Return the current armor
     */
    public float GetArmor()
    {
        return _armor;
    }

    /**
     * Add the given amount of armor
     */
    public void AddArmor(float amount)
    {
        _armor += amount;
        if (amount < 0f) _armor = Mathf.Max(0f, _armor);
        else _armor = Mathf.Min(_maxArmor, _armor);
        NotifySubscribersOnArmorChanged();
    }

    /**
     * Return the normalized armor
     */
    public float GetNormalizedArmor()
    {
        return _armor / _maxArmor;
    }

    /**
     * Subtract the given amount to the health
     */
    public void Damage(float amount)
    {
        float remainingDamageAmount = amount;

        if (_armor > 0f) remainingDamageAmount = DamageArmor(amount);
        if (remainingDamageAmount == 0f) return;

        _health -= Mathf.Abs(remainingDamageAmount);
        _health = Mathf.Max(0f, _health);

        // Ask the subscribers to update themselves (i.e the health bar)
        _onHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { 
            normalizedHealthAmount = GetNormalizedHealth(),
            isLowHealth = GetNormalizedHealth() < (1f / 3)
        });
    }

    /**
     * Subtract the given amount to the armor
     */
    public float DamageArmor(float amount)
    {
        _armor -= Mathf.Abs(amount);
        float remainingAmount = Mathf.Abs(Mathf.Min(_armor, 0f));
        _armor = Mathf.Max(0f, _armor);

        // Ask the subscribers to update themselves (i.e the armor bar)
        NotifySubscribersOnArmorChanged();

        return remainingAmount;
    }

    /**
     * Add the given amount to the health
     */
    public void Heal(float amount)
    {
        _health += Math.Abs(amount);
        _health = Mathf.Min(_maxHealth, _health);

        _onHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { 
            normalizedHealthAmount = GetNormalizedHealth(),
            isLowHealth = GetNormalizedHealth() < (1f / 3)
        });
    }

    /**
     * Notifiy subscribers to update themselves
     */
    public void NotifySubscribersOnArmorChanged()
    {
        _onArmorChanged?.Invoke(this, new OnArmorChangedEventArgs { normalizedArmorAmount = GetNormalizedArmor() });
    }
}