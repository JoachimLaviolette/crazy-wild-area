using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          SuperPotionSO class
 * ------------------------------------------------
 */

[CreateAssetMenu(fileName = "New super potion", menuName = "Inventory/Pickable/Ingestible/SuperPotionSO")]
public class SuperPotionSO : IngestibleSO
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

        ((ICanModify) user).SetHealthModifier(_modifier);
    }
}
