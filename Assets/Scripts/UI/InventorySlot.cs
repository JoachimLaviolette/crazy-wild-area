using UnityEngine;
using UnityEngine.UI;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          InventorySlot class
 * ------------------------------------------------
 */

public class InventorySlot : MonoBehaviour
{
    private Pickable _pickable;
    private ScriptableItem _currentItem;
    [SerializeField] 
    private Image _icon;
    [SerializeField]
    private Button _itemButton;
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private Sprite _currentItemSprite;
    [SerializeField]
    private Sprite _regularItemSprite;
    
    private const float _WEAPON_SLOT_EMPTY_ALPHA = .25f;
    private const float _WEAPON_SLOT_FILLED_ALPHA = 1f;
    private const float _CURRENT_Y = 30f;
    
    /**
     * Add the given item to the slot
     */
    public void AddItem(Pickable pickable)
    {
        _pickable = pickable;
        _currentItem = _pickable.GetScriptableItem();
        _icon.sprite = _pickable.GetScriptableItem().GetRegularIcon();
        _icon.enabled = true;
        _itemButton.interactable = true;

        if (_removeButton != null && _pickable is Ingestible) _removeButton.interactable = true;
        if (pickable is Weapon)
        {
            Color color = _itemButton.image.color;
            color.a = _WEAPON_SLOT_FILLED_ALPHA;
            _itemButton.image.color = color;

            if (((Weapon) pickable).IsCurrent()) MarkAsCurrent();
            else UnmarkAsCurrent();
        }
    }

    /**
     * Clear the slot
     */
    public void ClearSlot<T>()
    {
        _pickable = null;
        _currentItem = null;
        _icon.sprite = null;
        _icon.enabled = false;
        _itemButton.interactable = false;

        if (_removeButton != null && typeof(T) == typeof(Ingestible)) _removeButton.interactable = false;
        if (typeof(T) == typeof(Weapon))
        {
            Color color = _itemButton.image.color;
            color.a = _WEAPON_SLOT_EMPTY_ALPHA;
            _itemButton.image.color = color;
            _itemButton.image.sprite = _regularItemSprite;
        }
    }

    /**
     * Triggered when we click the remove button of the slot
     */
    public void OnRemoveButton()
    {
        if (_pickable == null) return;
        if (_pickable is Ingestible)
        {
            Entity focusedEntity = EntityManager.GetFocusedEntity();
            if (focusedEntity != null) focusedEntity.GetInventory().Remove(_pickable);
        }
    }

    /**
     * Triggered when we click the item of slot
     */
    public void UseItem()
    {
        if (_currentItem == null) return;
        if (_currentItem.Use(_pickable))
        {
            if (_pickable is Ingestible) OnRemoveButton();
            else if (_pickable is Weapon)
            {
                UIManager.GetInventoryUI().UnmarkAllWeaponSlots();
                MarkAsCurrent();
            }
        }
    }

    /**
     * Mark the slot as current
     */
    public void MarkAsCurrent()
    {
        if (_currentItem == null) return;

        _itemButton.image.sprite = _currentItemSprite;
        _icon.sprite = _currentItem.GetCurrentIcon();
    }

    /**
     * Unmark the slot as current
     */
    public void UnmarkAsCurrent()
    {
        _itemButton.image.sprite = _regularItemSprite;
        if (_currentItem != null) _icon.sprite = _currentItem.GetRegularIcon();
    }
}

