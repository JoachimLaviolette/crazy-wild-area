using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          ArmorPotionSO class
 * ------------------------------------------------
 */

[CreateAssetMenu(fileName = "New armor potion", menuName = "Inventory/Pickable/Ingestible/ArmorPotionSO")]
public class ArmorPotionSO : IngestibleSO
{
    /**
     * Initialize the ingestible properties
     */
    protected override void Initialize()
    {
        _modifier = 30f;
    }

    /**
     * Consume the ingestible
     */
    protected override void Consume(Pickable pickable)
    {
        Focuser user = pickable.GetFocuser();

        if (user == null) return;
        if (user is ICanModify == false) return;

        ((ICanModify) user).SetArmorModifier(_modifier);
    }
}
