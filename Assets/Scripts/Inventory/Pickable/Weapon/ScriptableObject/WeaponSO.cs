
/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          WeaponSO class
 * ------------------------------------------------
 */

public abstract class WeaponSO : ScriptableItem
{
    /**
     * Use the item
     */
    public override bool Use(Pickable pickable)
    {
        Focuser user = pickable.GetFocuser();

        if (user == null) return false;
        if (user is ICanEquip<Weapon> == false) return false;

        ((ICanEquip<Weapon>) user).Equip((Weapon) pickable);
        SoundManager.PlaySelectWeaponSound(user.transform.position);

        return true;        
    }
}
