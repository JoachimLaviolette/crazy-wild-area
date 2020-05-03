using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          ScriptableItem class
 * ------------------------------------------------
 */

public abstract class ScriptableItem : ScriptableObject
{
    [SerializeField]
    protected string _name = "New scriptable item";
    [SerializeField]
    protected Sprite _regularIcon = null;
    [SerializeField]
    protected Sprite _currentIcon = null;
    [SerializeField]
    protected bool _isDefault = false;
    protected int _slotIndex;

    /**
     * Use the item
     */
    public abstract bool Use(Pickable pickable);

    /**
     * Return the name
     */
    public string GetName() 
    {
        return _name; 
    }

    /**
     * Return the regular icon
     */
    public Sprite GetRegularIcon()
    {
        return _regularIcon;
    }

    /**
     * Return the current icon
     */
    public Sprite GetCurrentIcon()
    {
        return _currentIcon;
    }

    /**
     * Return if it is a default item
     */
    public bool IsDefault()
    {
        return _isDefault;
    }
}
