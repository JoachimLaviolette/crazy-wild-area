using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          SpeedPotionSO class
 * ------------------------------------------------
 */

[CreateAssetMenu(fileName = "New speed potion", menuName = "Inventory/Pickable/Ingestible/SpeedPotionSO")]
public class SpeedPotionSO : IngestibleSO
{
    /**
     * Initialize the ingestible properties
     */
    protected override void Initialize()
    {
        _modifier = 1.5f; 
        _modifierPeriod = 7f;
    }

    /**
     * Consume the ingestible
     */
    protected override void Consume(Pickable pickable)
    {
        Focuser user = pickable.GetFocuser();

        if (user == null) return;
        if (user is ICanModify == false) return;

        ((ICanModify) user).SetSpeedModifier(_modifier, _modifierPeriod);
    }
}
