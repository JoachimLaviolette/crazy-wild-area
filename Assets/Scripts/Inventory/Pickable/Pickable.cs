using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Pickable class
 * ------------------------------------------------
 */

public class Pickable : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected ScriptableItem _scriptableItem;
    protected Inventory _focuserInventory;
    protected int _inventoryIndex;
    protected Focuser _focuser;

    /**
     * Specify how to interact with a scriptable item
     */
    public void Interact(Focuser focuser)
    {
        Pickup(focuser);
    }

    /**
     * Setup the pickable
     */
    public void Setup(Inventory inventory, Focuser focuser)
    {
        _focuserInventory = inventory;
        _focuser = focuser;
    }

    /**
     * Add the picked item to the interacting object's inventory
     */
    protected virtual void Pickup(Focuser focuser)
    {
        SoundManager.PlayPickupSound(transform.position);
    }

    /**
     * Return the scriptable item
     */
    public ScriptableItem GetScriptableItem()
    {
        return _scriptableItem;
    }

    /**
     * Get the user who picked the item
     */
    public Focuser GetFocuser()
    {
        return _focuser;
    }
}
