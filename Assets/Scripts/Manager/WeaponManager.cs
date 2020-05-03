using UnityEngine;
using System.Collections.Generic;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          WeaponManager class
 * ------------------------------------------------
 */

public class WeaponManager : MonoBehaviour
{
    [HideInInspector]
    private static WeaponManager _instance;

    private List<Weapon> _weapons;

    private void Awake()
    {
        _instance = this;
        _instance._weapons = new List<Weapon>();
    }

    private WeaponManager() { }

    /**
     * Return the count of recorded T weapons
     */
    public static int WeaponCount<T>()
    {
        int sum = 0;

        foreach (Weapon w in _instance._weapons)
        {
            if (w is T)
            {
                sum++;
            }
        }

        return sum;
    }

    /**
     * Add the given weapon to the record list
     */
    public static void AddWeapon(Weapon w)
    {
        _instance._weapons.Add(w);
    }

    /**
     * Remove the given weapon from the record list
     */
    public static void RemoveWeapon(Weapon w)
    {
        _instance._weapons.Remove(w);
    }

    /**
     * Return the weapon at the given index
     */
    public static Weapon GetWeapon(int i)
    {
        return _instance._weapons[i];
    }
}
