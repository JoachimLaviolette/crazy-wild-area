using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          InventoryUI class
 * ------------------------------------------------
 */

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _weaponsGO, _ingestiblesGO, _ammunitionGO;
    [SerializeField]
    private Transform _weaponSlotParent, _ingestibleSlotParent, _ammoSlotParent;
    [SerializeField]
    private GameObject _currentAmmoSlotData, _globalAmmoData;
    private Inventory _focusedEntityInventory;
    private Weapon _focusedEntityWeapon;
    private InventorySlot[] _weaponSlots;
    private InventorySlot[] _ingestibleSlots;
    private AmmoSlot[] _ammoSlots;

    private const string _CURRENT_AMMO_SLOT_DATA = "{0} | {1}";
    private const string _GLOBAL_AMMO_DATA = "{0} | {1}";

    private void Start()
    {
        _weaponSlots = _weaponSlotParent.GetComponentsInChildren<InventorySlot>();
        _ingestibleSlots = _ingestibleSlotParent.GetComponentsInChildren<InventorySlot>();
        _ammoSlots = null;
    }

    private void Update()
    {
        HandleInput();
    }

    /**
     * Handle user input to update UI state
     */
    private void HandleInput()
    {
        if (EntityManager.GetFocusedEntity() == null)
        {
            _weaponsGO.SetActive(false);
            _ingestiblesGO.SetActive(false);
            _ammunitionGO.SetActive(false);
        }
        else
        {
            _weaponsGO.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)) _ingestiblesGO.SetActive(!_ingestiblesGO.activeSelf);
        }
    }

    /**
     * Delegate called to update inventory UI when the focused entity's inventory changed
     */
    private void UpdateUI(object sender, Inventory.OnInventoryChangedEventArgs args)
    {
        if (args.inventory != _focusedEntityInventory) _focusedEntityInventory = args.inventory;
        
        UpdateSlots<Weapon>(); 
        UpdateSlots<Ingestible>();
        CheckOnDestroy(args.lastRemoved);
    }

    /**
     * Delegate called to update inventory UI when the focused entity's current weapon changed
     */
    private void UpdateUI(object sender, Entity.OnCurrentWeaponChangedEventArgs args)
    {
        UpdateCurrentWeaponUIData(args.weapon);
    }

    /**
     * Delegate called to update inventory UI when the focused entity's current weapon changed
     */
    private void UpdateUI(object sender, Weapon.OnDataChangedEventArgs args)
    {
        UpdateCurrentWeaponUIData(args.weapon);
    }

    /**
     * Update the current weapon UI data
     */
    private void UpdateCurrentWeaponUIData(Weapon weapon)
    {
        if (weapon != _focusedEntityWeapon) _focusedEntityWeapon = weapon;

        if (!(_focusedEntityWeapon is ShootingWeapon))
        {
            _ammunitionGO.SetActive(false);

            return;
        }

        _ammunitionGO.SetActive(true);
        UpdateAmmoSlots();
        UpdateAmmoData();
    }

    /**
     * Update ammo slots
     */
    private void UpdateAmmoSlots()
    {
        if (_focusedEntityWeapon is ShootingWeapon sw)
        {
            if (_ammoSlots != null)
            {
                for (int x = 0; x < _ammoSlots.Length; ++x)
                {
                    if (_ammoSlots[x] != null)
                    {
                        _ammoSlots[x].transform.SetParent(null);
                        Destroy(_ammoSlots[x].gameObject);
                    }
                }
                _ammoSlots = null;
            }

            for (int x = 0; x < sw.GetSlotCount(); ++x)
            {
                GameObject ammoSlotGO = Instantiate(
                    AssetManager.Get_Prefab_AmmoSlot(),
                    transform.position,
                    Quaternion.identity
                );

                ammoSlotGO.transform.SetParent(_ammoSlotParent);
                ammoSlotGO.transform.localScale = Vector3.one;
            }

            _ammoSlots = _ammoSlotParent.GetComponentsInChildren<AmmoSlot>();
            int ammoCounter = 0;
            int globalAmmoCount = sw.GetCurrentGlobalAmmoCount();
            int slotCapacity = sw.GetSlotCapacity();

            foreach (AmmoSlot slot in _ammoSlots)
            {
                for (int x = 0; x < slotCapacity; ++x)
                {
                    if (ammoCounter < globalAmmoCount)
                    {
                        slot.AddAmmo();
                        ++ammoCounter;
                    }
                }

                slot.CheckState(sw.GetSlotCapacity());
            }
        }
    }
    
    /**
     * Update ammo data
     */
    private void UpdateAmmoData()
    {
        if (_focusedEntityWeapon is ShootingWeapon sw)
        {
            _currentAmmoSlotData.GetComponent<Text>().text = string.Format(_CURRENT_AMMO_SLOT_DATA, sw.GetCurrentSlotAmmoCount(), sw.GetCurrentSlot());
            _globalAmmoData.GetComponent<Text>().text = string.Format(_GLOBAL_AMMO_DATA, sw.GetCurrentGlobalAmmoCount(), sw.GetMaxAmmoCapacity());
        }
    }

    /**
     * Check if there was an item removed that we now have to destroy
     */
    private void CheckOnDestroy(Pickable lastItemRemoved = null)
    {
        if (lastItemRemoved != null) Destroy(lastItemRemoved);
    }

    /**
     * Update the T inventory slots
     */
    private void UpdateSlots<T>()
    {
        InventorySlot[] slots = typeof(T) == typeof(Weapon) ? _weaponSlots
            : typeof(T) == typeof(Ingestible) ? _ingestibleSlots
            : null;

        if (slots == null) return;
        for (int x = 0; x < slots.Length; x++)
        {
            if (x < _focusedEntityInventory.GetInventorySize<T>()) slots[x].AddItem(_focusedEntityInventory.GetItemAt<T>(x));
            else slots[x].ClearSlot<T>();
        }
    }

    /**
     * Add the inventory UI as subscriber to the given entity data
     */
    public void Subscribe(Entity entity)
    {
        Inventory inventory = entity.GetInventory();
        Weapon weapon = entity.GetCurrentWeapon();

        inventory._onInventoryChanged += UpdateUI;
        entity._onCurrentWeaponChanged += UpdateUI;

        _focusedEntityInventory = inventory;
        _focusedEntityWeapon = weapon;
    }
    
    /**
     * Add the inventory UI as subscriber to the given weapon
     */
    public void Subscribe(Weapon weapon)
    {
        weapon._onDataChanged += UpdateUI;
        _focusedEntityWeapon = weapon;
    }

    /**
     * Remove the inventory UI from subscribers of the given entity
     */
    public void Unsubscribe(Entity entity)
    {
        Inventory inventory = entity.GetInventory();
        Weapon weapon = entity.GetCurrentWeapon();

        inventory._onInventoryChanged -= UpdateUI;
        entity._onCurrentWeaponChanged -= UpdateUI;

        if (_focusedEntityInventory == inventory) _focusedEntityInventory = null;
        if (_focusedEntityWeapon == weapon) _focusedEntityWeapon = null;
    }

    /**
     * Remove the inventory UI from subscribers of the given weapon
     */
    public void Unsubscribe(Weapon weapon)
    {
        weapon._onDataChanged -= UpdateUI;

        if (_focusedEntityWeapon == weapon) _focusedEntityWeapon = null;
    }

    /**
     * Unmark the given weapon slot as current
     */
    public void UnmarkWeaponSlot(InventorySlot slot)
    {
        if (_weaponSlots.Contains(slot)) slot.UnmarkAsCurrent();
    }

    /**
     * Unmark all the weapon slots as current
     */
    public void UnmarkAllWeaponSlots()
    {
        foreach (InventorySlot weaponSlot in _weaponSlots) UnmarkWeaponSlot(weaponSlot);
    }
}
