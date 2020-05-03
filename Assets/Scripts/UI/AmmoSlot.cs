using UnityEngine;
using UnityEngine.UI;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          AmmoSlot class
 * ------------------------------------------------
 */

public class AmmoSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject _ammos;
    private bool _isActive;

    private const float _AMMO_SLOT_EMPTY_ALPHA = .25f;
    private const float _AMMO_SLOT_FILLED_ALPHA = 1f;
    private const float _AMMO_SLOT_SPACING_FACTOR = 2f;

    private void Start()
    {
        _isActive = false;
    }

    /**
     * Add one ammo to the slot
     */
    public void AddAmmo()
    {
        if (!_isActive) MarkAsEnabled();

        GameObject ammoGO = Instantiate(
            AssetManager.Get_Prefab_Ammo(),
            transform.position,
            Quaternion.identity
        );

        ammoGO.transform.SetParent(_ammos.transform);
        ammoGO.transform.localScale = Vector3.one;
    }

    /**
     * Check the state of the ammo slot
     */
    public void CheckState(int slotCapacity)
    {
        int spacingCount = slotCapacity - 1;
        float ammoContainerHeight = _ammos.GetComponent<RectTransform>().rect.height;
        GridLayoutGroup ammoContainerLayout = _ammos.GetComponent<GridLayoutGroup>();

        if (_ammos.GetComponentsInChildren<Image>().Length == 0)
        {
            MarkAsDisabled();

            return;
        }

        Vector2 cellSize;
        Vector2 spacing;

        /*if (slotCapacity == 1)
        {
            cellSize = ammoContainerLayout.cellSize;
            cellSize.y = ammoContainerHeight;
            ammoContainerLayout.cellSize = cellSize;

            spacing = ammoContainerLayout.spacing;
            spacing.y = 0f;
            ammoContainerLayout.spacing = spacing;

            return;
        }*/

        //int i = 0; while (slotCapacity * i < ammoContainerHeight - spacingCount) ++i;
        float cellSizeY = (ammoContainerHeight - spacingCount * _AMMO_SLOT_SPACING_FACTOR) / slotCapacity; //slotCapacity * i >= ammoContainerHeight - spacingCount ? --i : slotCapacity * i;

        cellSize = ammoContainerLayout.cellSize;
        cellSize.y = cellSizeY;
        ammoContainerLayout.cellSize = cellSize;

        spacing = ammoContainerLayout.spacing;
        spacing.y = _AMMO_SLOT_SPACING_FACTOR; //(ammoContainerHeight - (slotCapacity * cellSizeY)) / spacingCount;
        ammoContainerLayout.spacing = spacing;
    }

    /**
     * Mark the slot as enabled
     */
    private void MarkAsEnabled()
    {
        _isActive = true;
        Image currentImage = GetComponent<Image>();
        Color color = currentImage.color;
        color.a = _AMMO_SLOT_FILLED_ALPHA;
        currentImage.color = color;
    }
    
    /**
     * Mark the slot as disabled
     */
    private void MarkAsDisabled()
    {
        _isActive = false;
        Image currentImage = GetComponent<Image>();
        Color color = currentImage.color;
        color.a = _AMMO_SLOT_EMPTY_ALPHA;
        currentImage.color = color;
    }
}
