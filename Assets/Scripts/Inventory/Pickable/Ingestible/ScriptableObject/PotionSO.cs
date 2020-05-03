using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          PotionSO class
 * ------------------------------------------------
 */

[CreateAssetMenu(fileName = "New potion", menuName = "Inventory/Pickable/Ingestible/PotionSO")]
public class PotionSO : IngestibleSO
{
    /**
     * Initialize the ingestible properties
     */
    protected override void Initialize()
    {
        _modifier = 15f;
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
