using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;


/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Entity class
 * ------------------------------------------------
 */

public class Entity : Focuser, IDamageable, ICanEquip<Weapon>, ICanModify, ICanUseWeapon, ICanThrow
{
    public class OnScoringChangedEventArgs : EventArgs { public float killCount; }
    public event EventHandler<OnScoringChangedEventArgs> _onScoringChanged;
    public class OnCurrentWeaponChangedEventArgs : EventArgs { public Weapon weapon; }
    public event EventHandler<OnCurrentWeaponChangedEventArgs> _onCurrentWeaponChanged;

    private string _name;
    private float _walkSpeed;
    private float _runSpeed;
    private float _currentSpeed;
    private float _throwImpulseForce;
    private float _speedModifier;
    private float _speedModifierPeriod;

    private HealthSystem _healthSystem;
    private HealthBar _healthBar;
    private static bool _isLowHealthSoundPlaying;
    private Material _defaultMaterial;
    [SerializeField]
    private Material _damageMaterial;

    private float _speedSmoothVelocity;
    private float _speedSmoothTime;
    private float _rotationSmoothVelocity;
    private float _rotationSmoothTime;
    private CameraController _camera;
    private MinimapCameraController _minimapCamera;
    private ZoomCameraController _zoomCamera;
    private bool _isZooming;

    private Inventory _inventory;
    private Weapon _currentWeapon;
    private Transform _weaponHandlingZone;

    private int _kills;
    private int _rank;

    private const string _DEFAULT_NAME = "Entity";
    private const float _DEFAULT_HEALTH = 100f;
    private const float _DEFAULT_MAX_HEALTH = 100f;
    private const float _DEFAULT_ARMOR = 0f;
    private const float _DEFAULT_MAX_ARMOR = 100f;
    private const float _DEFAULT_WALK_SPEED = 5f;
    private const float _DEFAULT_RUN_SPEED = 12f;
    private const float _DEFAULT_THROW_IMPULSE_FORCE = 10f;
    private const float _DEFAULT_DAMAGES = 10f;
    private const int _DEFAULT_RANK = -1;

    private void Start()
    {
        _camera = AssetManager.GetMainCamera().GetComponent<CameraController>();
        _minimapCamera = AssetManager.GetMinimapCamera().GetComponent<MinimapCameraController>();
        _zoomCamera = GetComponentInChildren<ZoomCameraController>();
        _weaponHandlingZone = GetComponentInChildren<WeaponHandlingZone>().transform;
        _speedModifier = 1f;
        _speedModifierPeriod = 0f;
        _speedSmoothTime = .3f;
        _rotationSmoothTime = .3f;
        _isZooming = false; 
        _defaultMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void Update()
    {
        HandleClickDown();
        HandleMovements();
        HandleModifiers();
        HandleWeaponUse();
        HandleZoom();
    }

    protected override void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            if (interactable is Pickable)
            {
                ((Pickable) interactable).Setup(_inventory, this);
                interactable.Interact(this);
            }
            else if (damageable != null && EntityManager.GetFocusedEntity() != null)
                if (damageable != EntityManager.GetFocusedEntity().GetComponent<IDamageable>()) damageable.Damage(this, _DEFAULT_DAMAGES);
        }
    }

    /**
     * Setup the entity
     */
    public void Setup(string name = _DEFAULT_NAME, float health = _DEFAULT_HEALTH, float maxHealth = _DEFAULT_MAX_HEALTH, float armor = _DEFAULT_ARMOR, float maxArmor = _DEFAULT_MAX_ARMOR, float walkSpeed = _DEFAULT_WALK_SPEED, float runSpeed = _DEFAULT_RUN_SPEED, float throwImpulseForce = _DEFAULT_THROW_IMPULSE_FORCE, HealthBar healthBar = null)
    {
        _name = name;
        _walkSpeed = walkSpeed;
        _runSpeed = runSpeed;
        _throwImpulseForce = throwImpulseForce;

        _healthBar = healthBar;
        _healthSystem = new HealthSystem(health, maxHealth, armor, maxArmor);
        _healthSystem._onHealthChanged += HandleHealthSound;
        _healthBar?.Setup(_healthSystem);
        _healthBar.gameObject.SetActive(false);
        _isLowHealthSoundPlaying = false;

        _inventory = new Inventory(this);
        _currentWeapon = null;

        _kills = 0;
        _rank = _DEFAULT_RANK;
    }

    /**
     * Handle health sound
     */
    private void HandleHealthSound(object sender, HealthSystem.OnHealthChangedEventArgs args)
    {
        if (this != EntityManager.GetFocusedEntity()) return;

        CheckHealthSound(args.isLowHealth);
    }

    /**
     * Check the health sound
     */
    private void CheckHealthSound(bool isLowHealth)
    {
        if (isLowHealth && !_isLowHealthSoundPlaying)
        {
            SoundManager.PlayLowHealthSound(_name);
            _isLowHealthSoundPlaying = true;
        }
        else if (!isLowHealth && _isLowHealthSoundPlaying)
        {
            SoundManager.StopLowHealthSound(_name);
            _isLowHealthSoundPlaying = false;
        }
    }

    /**
     * Check if the current entity was clicked
     */
    private void HandleClickDown()
    {
        if (!Input.GetMouseButtonDown(1)) return; 
        if (EventSystem.current.IsPointerOverGameObject()) return;        
        if (!Physics.Raycast(_camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;        
        if (hit.transform.gameObject.GetComponent<Entity>() != this) return;
        if (EntityManager.GetFocusedEntity() == this) return;

        // Follow the clicked entity as the new camera's target
        _camera.SetGetTargetPosition(() => { return transform.position; });
        _minimapCamera.SetGetTargetPosition(() => { return transform.position; });

        Entity currentFocusedEntity = EntityManager.GetFocusedEntity();

        // No more listen to the current entity's inventory
        if (currentFocusedEntity != null)
        {
            UIManager.GetScoringUI().Unsubscribe(currentFocusedEntity);
            UIManager.GetInventoryUI().Unsubscribe(currentFocusedEntity);
        }
        
        // If for the current entity low health sound was playing, stop it
        if (_isLowHealthSoundPlaying)
        {
            SoundManager.StopLowHealthSound(currentFocusedEntity.GetName());
            _isLowHealthSoundPlaying = false;
        }

        // Mark the clicked entity as the new focused one
        EntityManager.SetFocusedEntity(this);

        // Hide the former focused entity data and display the new focused entity's
        if (currentFocusedEntity != null) currentFocusedEntity._healthBar.gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(true);

        // We now observe the clicked entity and its inventory
        UIManager.GetScoringUI().Subscribe(this);
        UIManager.GetInventoryUI().Subscribe(this);

        // Ask the inventory UI and the scoring UI to update based on the new focused entity's inventory data
        _inventory.NotifySubscribers();
        NotifySubscribers();

        // Check if we have to play low health sound for the new focused entity
        CheckHealthSound(GetNormalizedHealth() < (1f / 3));
    }

    /**
     * Handle the entity movements
     */
    private void HandleMovements()
    {
        if (EntityManager.GetFocusedEntity() == this)
        {
            Vector3 moveDirUp = Vector3.zero;
            Vector3 moveDirDown = Vector3.zero;
            Vector3 moveDirRight = Vector3.zero;
            Vector3 moveDirLeft = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow)) moveDirUp = Vector3.forward;
            if (Input.GetKey(KeyCode.DownArrow)) moveDirDown = Vector3.back;
            if (Input.GetKey(KeyCode.RightArrow)) moveDirRight = Vector3.right;
            if (Input.GetKey(KeyCode.LeftArrow)) moveDirLeft = Vector3.left;

            Vector3 moveDir = (moveDirUp + moveDirDown + moveDirRight + moveDirLeft).normalized;

            if (moveDir != Vector3.zero)
            {
                Camera camera = _camera.GetComponent<Camera>().enabled ? _camera.GetComponent<Camera>() : _zoomCamera.GetComponent<Camera>();
                float targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationSmoothVelocity, _rotationSmoothTime);
            }

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float targetSpeed = ((isRunning) ? _runSpeed : _walkSpeed) * _speedModifier * moveDir.magnitude;
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, _speedSmoothTime);
            transform.Translate(transform.forward * _currentSpeed * Time.deltaTime, Space.World);
        }
    }

    /**
     * Handle modifiers
     */
    private void HandleModifiers()
    {
        if (_speedModifierPeriod > 0f)
        {
            _speedModifierPeriod -= Time.deltaTime;
            _speedModifierPeriod = Mathf.Max(0, _speedModifierPeriod);
        }
        else _speedModifier = 1f;
    }

    /**
     * Handle the use of the current weapon
     */
    public void HandleWeaponUse()
    {
        if (_currentWeapon == null) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (EntityManager.GetFocusedEntity() != this) return;

        Camera camera = _camera.GetComponent<Camera>().enabled ? _camera.GetComponent<Camera>() : _zoomCamera.GetComponent<Camera>();

        if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;

        transform.LookAt(hit.point);
        Vector3 euleurAngles = transform.eulerAngles;
        euleurAngles.x = 0f;
        transform.eulerAngles = euleurAngles;
        _currentWeapon.transform.LookAt(hit.point);
        _currentWeapon.Use(() => { return hit.point; });
    }

    /**
     * Return the throw impulse force
     */
    public float GetImpulseForce()
    {
        return _throwImpulseForce;
    }

    /**
     * Handle zoom
     */
    private void HandleZoom()
    {
        if (EntityManager.GetFocusedEntity() != this) return;
        if (!Input.GetKey(KeyCode.LeftControl) && _isZooming)
        {
            Unzoom(); 

            return; 
        } 
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _isZooming = true;
            _zoomCamera.GetComponent<Camera>().enabled = true;
            _camera.GetComponent<Camera>().enabled = false;
        }
    }

    /**
     * Unzoom 
     */
    private void Unzoom()
    {
        if (EntityManager.GetFocusedEntity() != this) return;

        _isZooming = false;
        _zoomCamera.GetComponent<Camera>().enabled = false;
        _camera.GetComponent<Camera>().enabled = true;
    }

    /**
     * Return the name
     */
    public string GetName()
    {
        return _name;
    }

    /**
     * Return the current health
     */
    public float GetHealth()
    {
        return _healthSystem.GetHealth();
    }

    /**
     * Return the normalized health
     */
    public float GetNormalizedHealth()
    {
        return _healthSystem.GetNormalizedHealth();
    }

    /**
     * Return the current armor
     */
    public float GetArmor()
    {
        return _healthSystem.GetArmor();
    }

    /**
     * Return the normalized armor
     */
    public float GetNormalizedArmor()
    {
        return _healthSystem.GetNormalizedArmor();
    }

    /*
     * Return the inventory
     */
    public Inventory GetInventory()
    {
        return _inventory;
    }

    /**
     * Return the entity's current weapon
     */
    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    /**
     * Damage the entity of the given amount
     */
    public void Damage(Focuser source, float amount)
    {
        _healthSystem.Damage(amount);

        if (GetHealth() <= 0f)
        {
            if (source.GetComponent<Entity>()) source.GetComponent<Entity>().AddKill(1);     
            if (this == EntityManager.GetFocusedEntity() && _isLowHealthSoundPlaying) SoundManager.StopLowHealthSound(EntityManager.GetFocusedEntity().GetName());

            EntityManager.Remove(this);
            SoundManager.PlayDeadSound(transform.position, .5f);
            Destroy(gameObject);
        }

        // Trigger damage sound
        SoundManager.PlayDamageSound(GetArmor() > 0f, transform.position, .5f);
        StopCoroutine(PlayDamageAnimation());
        StartCoroutine(PlayDamageAnimation());
    }

    /**
     * Play damage animation
     */
    public IEnumerator PlayDamageAnimation()
    {
        for (int x = 0; x< 5; ++x)
        {
            GetComponent<MeshRenderer>().sharedMaterial = _damageMaterial;
            yield return new WaitForSeconds(.07f);
            GetComponent<MeshRenderer>().sharedMaterial = _defaultMaterial;
            yield return new WaitForSeconds(.07f);
        }
    }

    /**
     * Heal the entity of the given amount
     */
    public void Heal(float amount)
    {
        _healthSystem.Heal(amount);
    }

    /**
     * Return the weapong handling zone
     */
    public Transform GetWeaponHandlingZone()
    {
        return _weaponHandlingZone;
    }

    /**
     * Add the given kill count to the current kills
     */
    public void AddKill(int count)
    {
        _kills += Mathf.Max(0, count);
        NotifySubscribers();
    }

    /**
     * Set the given speed modifier for the given period of time
     */
    public void SetSpeedModifier(float speedModifier, float speedModifierPeriod)
    {
        _speedModifier = Mathf.Max(1f, speedModifier);
        _speedModifierPeriod = speedModifierPeriod;
    }

    /**
     * Set the given armor modifier
     */
    public void SetArmorModifier(float armorModifier)
    {
        _healthSystem.AddArmor(armorModifier);
    }

    /**
     * Set the given health modifier
     */
    public void SetHealthModifier(float healthModifier)
    {
        Heal(healthModifier);
    }

    /**
     * Equip the given weapon
     */
    public void Equip(Weapon w)
    {
        if (_currentWeapon != null) Unequip(_currentWeapon);

        _currentWeapon = w;
        _currentWeapon.SetCurrent(true);
        _currentWeapon.gameObject.SetActive(true);
        _currentWeapon.transform.position = _weaponHandlingZone.position;
        _currentWeapon.transform.rotation = transform.rotation;
        _currentWeapon.transform.SetParent(transform);

        UIManager.GetInventoryUI().Subscribe(_currentWeapon);
        _currentWeapon.NotifySubscribers();
    }

    /**
     * Unequip the given weapon
     */
    public void Unequip(Weapon w)
    {
        UIManager.GetInventoryUI().Unsubscribe(w);
        w.SetCurrent(false);

        if (w is ShootingWeapon sw)
        {
            sw.SetIsReloading(false);
            sw.gameObject.SetActive(false);
        }
    }

    /**
     * Ask the subscribers to update themselves (i.e the scoring UI)
     */
    public void NotifySubscribers()
    {
        _onScoringChanged?.Invoke(this, new OnScoringChangedEventArgs
        {
            killCount = _kills
        });

        _onCurrentWeaponChanged?.Invoke(this, new OnCurrentWeaponChangedEventArgs
        {
            weapon = _currentWeapon
        });
    }
}
