using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Ingestible class
 * ------------------------------------------------
 */

public abstract class Ingestible : Pickable
{
    /**
     * Add the picked ingestible to the interacting entity inventory
     */
    protected override void Pickup(Focuser focuser)
    {
        if (_focuserInventory.Add(this))
        {
            gameObject.SetActive(false);
            base.Pickup(focuser);
        }
    }
}
