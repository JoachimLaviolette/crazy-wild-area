using UnityEngine;
using JoaDev.Utils;
using System;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          HealthBar class
 * ------------------------------------------------
 */

public class HealthBar : MonoBehaviour
{
    private Transform _healthBarLevelAnchor;
    private Transform _armorBarLevelAnchor;
    private HealthBarLevel _healthBarLevel;
    private HealthSystem _healthSystem;

    private static string HEALTH_LEVEL_GOOD = "#7BCF55";
    private static string HEALTH_LEVEL_MEDIUM = "#EFCC38";
    private static string HEALTH_LEVEL_BAD = "#EE4737";

    private void Awake()
    {
        _healthBarLevelAnchor = GetComponentInChildren<HealthBarLevelAnchor>().transform;
        _armorBarLevelAnchor = GetComponentInChildren<ArmorBarLevelAnchor>().transform;
        _healthBarLevel = _healthBarLevelAnchor.GetComponentInChildren<HealthBarLevel>();
    }

    /**
     * Setup the health bar
     */
    public void Setup(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _healthSystem._onHealthChanged += UpdateHealthBarLevel;
        _healthSystem._onArmorChanged += UpdateArmorBarLevel;
        _healthSystem.NotifySubscribersOnArmorChanged();
    }

    /**
     * Update health bar level when the associated event has been triggered
     */
    public void UpdateHealthBarLevel(object sender, HealthSystem.OnHealthChangedEventArgs args)
    {
        SetHealthBarLevel(args.normalizedHealthAmount);
        SetHealthBarColor(args.normalizedHealthAmount);
    }

    /**
     * Update armor bar level when the associated event has been triggered
     */
    public void UpdateArmorBarLevel(object sender, HealthSystem.OnArmorChangedEventArgs args)
    {
        SetArmorBarLevel(args.normalizedArmorAmount);
    }

    /**
     * Set health bar level
     */
    private void SetHealthBarLevel(float normalizedHealthAmount)
    {
        _healthBarLevelAnchor.localScale = new Vector3(normalizedHealthAmount, _healthBarLevelAnchor.localScale.y, _healthBarLevelAnchor.localScale.z);
    }

    /**
     * Set armor bar level
     */
    private void SetArmorBarLevel(float normalizedArmorAmount)
    {
        _armorBarLevelAnchor.localScale = new Vector3(normalizedArmorAmount, _armorBarLevelAnchor.localScale.y, _armorBarLevelAnchor.localScale.z);
    }

    /**
     * Set health bar color according to the remaining health amount
     */
    private void SetHealthBarColor(float normalizedHealthAmount)
    {
        string colorHex = HEALTH_LEVEL_GOOD;

        if (normalizedHealthAmount < (2f / 3)) colorHex = HEALTH_LEVEL_MEDIUM;
        if (normalizedHealthAmount < (1f / 3)) colorHex = HEALTH_LEVEL_BAD;

        _healthBarLevel.GetComponent<SpriteRenderer>().color = Utils.GetColorFromString(colorHex);
    }
}
