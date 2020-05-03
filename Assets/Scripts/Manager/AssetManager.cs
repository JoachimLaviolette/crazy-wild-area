using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          AssetManager class
 * ------------------------------------------------
 */

public class AssetManager : MonoBehaviour
{
    private static AssetManager _instance;
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Camera _minimapCamera;
    [SerializeField]
    private Camera _UICamera;
    [SerializeField]
    private Transform _terrain;

    // ------------------------------------- Prefabs
    [SerializeField]
    private Transform _prefab_Wall;
    [SerializeField]
    private Entity _prefab_Entity;
    [SerializeField]
    private Bird _prefab_Bird;
    [SerializeField]
    private Cloud _prefab_Cloud;
    [SerializeField]
    private Potion _prefab_Potion;
    [SerializeField]
    private SuperPotion _prefab_SuperPotion;
    [SerializeField]
    private SpeedPotion _prefab_SpeedPotion;
    [SerializeField]
    private ArmorPotion _prefab_ArmorPotion;
    [SerializeField]
    private HealthBar _prefab_HealthBar;
    [SerializeField]
    private GameObject _prefab_Ammo;
    [SerializeField]
    private GameObject _prefab_AmmoSlot;
    [SerializeField]
    private BurstRifle _prefab_BurstRifle;
    [SerializeField]
    private Bullet _prefab_BurstRifleBullet;
    [SerializeField]
    private HuntingRifle _prefab_HuntingRifle;
    [SerializeField]
    private Bullet _prefab_HuntingRifleBullet;
    [SerializeField]
    private Enforcer _prefab_Enforcer;
    [SerializeField]
    private Bullet _prefab_EnforcerBullet;
    [SerializeField]
    private Grenade _prefab_Grenade;

    // ------------------------------------- Audio clips
    // Entity
    [SerializeField]
    private AudioClip _clip_DeadSound;
    [SerializeField]
    private AudioClip _clip_LowHealthSound;
    [SerializeField]
    private AudioClip _clip_DamageSound;
    [SerializeField]
    private AudioClip _clip_DamageArmorSound;
    // Environment
    [SerializeField]
    private AudioClip _clip_WindSound;
    [SerializeField]
    private AudioClip _clip_BirdSound;
    [SerializeField]
    private AudioClip _clip_RainSound;
    [SerializeField]
    private AudioClip _clip_ThunderSound;
    // Pickable
    [SerializeField]
    private AudioClip _clip_PickSound;
    // Ingestibles
    [SerializeField]
    private AudioClip _clip_IngestibleSound;
    // Shooting weapons
    [SerializeField]
    private AudioClip _clip_SelectWeaponSound;
    [SerializeField]
    private AudioClip _clip_ReloadSound;
    [SerializeField]
    private AudioClip _clip_BurstShotSound;
    [SerializeField]
    private AudioClip _clip_HuntingShotSound;
    [SerializeField]
    private AudioClip _clip_EnforcerShotSound;
    [SerializeField]
    private AudioClip _clip_NoAmmoShotSound;
    [SerializeField]
    private AudioClip _clip_ThrowSound;
    [SerializeField]
    private AudioClip _clip_UnpinSound;
    [SerializeField]
    private AudioClip _clip_ExplosionSound;

    private void Awake()
    {
        _instance = this;
    }

    private AssetManager() { }

    /**
     * Get the main camera
     */
    public static Camera GetMainCamera()
    {
        return _instance._mainCamera;
    }

    /**
     * Get the minimap camera
     */
    public static Camera GetMinimapCamera()
    {
        return _instance._minimapCamera;
    }

    /**
     * Get the UI camera
     */
    public static Camera GetUICamera()
    {
        return _instance._UICamera;
    }

    /**
     * Get the terrain
     */
    public static Transform GetTerrain()
    {
        return _instance._terrain;
    }

    // --------------------------------------------------------- Prefabs
    /**
     * Get wall prefab
     */
    public static Transform Get_Prefab_Wall()
    {
        return _instance._prefab_Wall;
    }


    /**
     * Get entity prefab
     */
    public static Entity Get_Prefab_Entity()
    {
        return _instance._prefab_Entity;
    }

    /**
     * Get bird prefab
     */
    public static Bird Get_Prefab_Bird()
    {
        return _instance._prefab_Bird;
    }

    /**
     * Get cloud prefab
     */
    public static Cloud Get_Prefab_Cloud()
    {
        return _instance._prefab_Cloud;
    }

    /**
     * Get potion prefab
     */
    public static Potion Get_Prefab_Potion()
    {
        return _instance._prefab_Potion;
    }

    /**
     * Get super potion prefab
     */
    public static SuperPotion Get_Prefab_SuperPotion()
    {
        return _instance._prefab_SuperPotion;
    }

    /**
     * Get speed potion prefab
     */
    public static SpeedPotion Get_Prefab_SpeedPotion()
    {
        return _instance._prefab_SpeedPotion;
    }

    /**
     * Get armor potion prefab
     */
    public static ArmorPotion Get_Prefab_ArmorPotion()
    {
        return _instance._prefab_ArmorPotion;
    }

    /**
     * Get health bar prefab
     */
    public static HealthBar Get_Prefab_HealthBar()
    {
        return _instance._prefab_HealthBar;
    }

    /**
     * Get ammo prefab
     */
    public static GameObject Get_Prefab_Ammo()
    {
        return _instance._prefab_Ammo;
    }

    /**
     * Get ammo slot prefab
     */
    public static GameObject Get_Prefab_AmmoSlot()
    {
        return _instance._prefab_AmmoSlot;
    }

    /**
     * Get the appropriate prefab regarding the given weapon's type
     */
    public static Weapon Get_Prefab_Weapon(Weapon w)
    {
        if (w is BurstRifle) return Get_Prefab_BurstRifle();

        return null;
    }

    /**
     * Get burst rifle prefab
     */
    public static BurstRifle Get_Prefab_BurstRifle()
    {
        return _instance._prefab_BurstRifle;
    }

    /**
     * Get burst rifle bullet prefab
     */
    public static Bullet Get_Prefab_BurstRifleBullet()
    {
        return _instance._prefab_BurstRifleBullet;
    }

    /**
     * Get hunting rifle prefab
     */
    public static HuntingRifle Get_Prefab_HuntingRifle()
    {
        return _instance._prefab_HuntingRifle;
    }

    /**
     * Get hunting rifle bullet prefab
     */
    public static Bullet Get_Prefab_HuntingRifleBullet()
    {
        return _instance._prefab_HuntingRifleBullet;
    }

    /**
     * Get enforcer prefab
     */
    public static Enforcer Get_Prefab_Enforcer()
    {
        return _instance._prefab_Enforcer;
    }

    /**
     * Get enforcer bullet prefab
     */
    public static Bullet Get_Prefab_EnforcerBullet()
    {
        return _instance._prefab_EnforcerBullet;
    }

    /**
     * Get grenade prefab
     */
    public static Grenade Get_Prefab_Grenade()
    {
        return _instance._prefab_Grenade;
    }

    // --------------------------------------------------------- Audio clips
    // ---------------------------- Entity
    /**
     * Get dead sound
     */
    public static AudioClip Get_Clip_DeadSound()
    {
        return _instance._clip_DeadSound;
    }

    /**
     * Get low health sound
     */
    public static AudioClip Get_Clip_LowHealthSound()
    {
        return _instance._clip_LowHealthSound;
    }

    // ---------------------------- Environment
    /**
     * Get bird sound
     */
    public static AudioClip Get_Clip_BirdSound()
    {
        return _instance._clip_BirdSound;
    }

    /**
     * Get wind sound
     */
    public static AudioClip Get_Clip_WindSound()
    {
        return _instance._clip_WindSound;
    }

    /**
     * Get rain sound
     */
    public static AudioClip Get_Clip_RainSound()
    {
        return _instance._clip_RainSound;
    }

    /**
     * Get thunder sound
     */
    public static AudioClip Get_Clip_TunderSound()
    {
        return _instance._clip_ThunderSound;
    }

    // ---------------------------- Pickable
    /**
     * Get pick sound
     */
    public static AudioClip Get_Clip_PickSound()
    {
        return _instance._clip_PickSound;
    }

    // ---------------------------- Ingestibles
    /**
     * Get ingestible sound
     */
    public static AudioClip Get_Clip_IngestibleSound()
    {
        return _instance._clip_IngestibleSound;
    }

    // ---------------------------- Shooting weapons
    /**
     * Get select weapon sound
     */
    public static AudioClip Get_Clip_SelectWeaponSound()
    {
        return _instance._clip_SelectWeaponSound;
    }

    /**
     * Get reload sound
     */
    public static AudioClip Get_Clip_ReloadSound()
    {
        return _instance._clip_ReloadSound;
    }

    /**
     * Get burst shot sound
     */
    public static AudioClip Get_Clip_BurstShotSound()
    {
        return _instance._clip_BurstShotSound;
    }

    /**
     * Get hunting shot sound
     */
    public static AudioClip Get_Clip_HuntingShotSound()
    {
        return _instance._clip_HuntingShotSound;
    }

    /**
     * Get enforcer shot sound
     */
    public static AudioClip Get_Clip_EnforcerShotSound()
    {
        return _instance._clip_EnforcerShotSound;
    }

    /**
     * Get no ammo shot sound
     */
    public static AudioClip Get_Clip_NoAmmoShotSound()
    {
        return _instance._clip_NoAmmoShotSound;
    }

    /**
     * Get throw sound
     */
    public static AudioClip Get_Clip_ThrowSound()
    {
        return _instance._clip_ThrowSound;
    }

    /**
     * Get unpin sound
     */
    public static AudioClip Get_Clip_UnpinSound()
    {
        return _instance._clip_UnpinSound;
    }

    /**
     * Get explosion sound
     */
    public static AudioClip Get_Clip_ExplosionSound()
    {
        return _instance._clip_ExplosionSound;
    }

    /**
     * Get damage sound
     */
    public static AudioClip Get_Clip_DamageSound()
    {
        return _instance._clip_DamageSound;
    }

    /**
     * Get damage sound when an armor is worn
     */
    public static AudioClip Get_Clip_DamageArmorSound()
    {
        return _instance._clip_DamageArmorSound;
    }
}
