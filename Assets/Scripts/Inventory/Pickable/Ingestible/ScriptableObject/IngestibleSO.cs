/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          IngestibleSO class
 * ------------------------------------------------
 */

public abstract class IngestibleSO : ScriptableItem
{
    protected float _modifier;
    protected float _modifierPeriod;

    /**
     * Initialize the ingestible properties
     */
    protected abstract void Initialize();

    /**
     * Consume the ingestible
     */
    protected abstract void Consume(Pickable pickable);

    /**
     * Use the item
     */
    public override bool Use(Pickable pickable)
    {
        Focuser user = pickable.GetFocuser();

        if (user == null) return false;

        Initialize();
        Consume(pickable);
        SoundManager.PlayIngestibleSound(user.transform.position, .6f);

        return true;
    }
}
