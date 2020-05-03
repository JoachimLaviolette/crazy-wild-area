using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          SoundManager class
 * ------------------------------------------------
 */

public static class SoundManager
{
    public enum ShotSoundType
    {
        BURST,
        HUNTING,
        ENFORCER,
        NO_AMMO,
    };
    
    public enum EnvironmentSoundType
    {
        BIRD,
        WIND,
    };

    private const float _MIN_VOLUME = 0f;
    private const float _MAX_VOLUME = 1f;
    private const float _MAX_DISTANCE = 500f;
    private const float _DOPPLER_LEVEL = 0f;
    private const float _SPATIAL_BLEND = 1f;

    private const string _SOUND_GO_PREFIX = "soundGO";
    private const string _SOUND_GO_3D_PREFIX = "soundGO_3D";

    /**
     * Play the given clip
     */
    private static void PlaySound(AudioClip clip, float volume = _MAX_VOLUME, bool loop = false, string extensionName = "")
    {
        GameObject soundGO = new GameObject("soundGO" + FormatExtensionName(extensionName));

        AudioSource source = soundGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.Play();

        if (!loop) Object.Destroy(soundGO, source.clip.length);
    }

    /**
     * Play the given clip in 3D
     */
    private static void PlaySound(AudioClip clip, Vector3 worldPosition, float volume = _MAX_VOLUME, bool loop = false, string extensionName = "")
    {
        GameObject soundGO = new GameObject("soundGO_3D" + FormatExtensionName(extensionName));
        soundGO.transform.position = worldPosition;

        AudioSource source = soundGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.maxDistance = _MAX_DISTANCE;
        source.dopplerLevel = _DOPPLER_LEVEL;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.spatialBlend = _SPATIAL_BLEND;
        source.loop = loop;
        source.Play();

        if (!loop) Object.Destroy(soundGO, source.clip.length);
    }

    /**
     * Stop the clip associated to the 3D sound game object with the given extension name
     */
    private static void StopSound(string extensionName = "", bool isDynamic = false)
    {
        string prefix = isDynamic ? _SOUND_GO_3D_PREFIX : _SOUND_GO_PREFIX;
        GameObject soundGO = GameObject.Find(prefix + FormatExtensionName(extensionName));
        Object.Destroy(soundGO);
    }

    /**
     * Format extension name
     */
    private static string FormatExtensionName(string extensionName)
    {
        if (!string.IsNullOrEmpty(extensionName)) extensionName = "_" + extensionName;

        return extensionName;
    }

    /**
     * Play shot sound related to the given shot sound type
     */
    public static void PlayShotSound(ShotSoundType type, Vector3 worldPosition, float volume = 1f)
    {
        switch (type)
        {
            case ShotSoundType.BURST:
                PlaySound(AssetManager.Get_Clip_BurstShotSound(), worldPosition, volume);
                
                return;
            case ShotSoundType.HUNTING:
                PlaySound(AssetManager.Get_Clip_HuntingShotSound(), worldPosition, volume);

                return;
            case ShotSoundType.ENFORCER:
                PlaySound(AssetManager.Get_Clip_EnforcerShotSound(), worldPosition, volume);

                return;
            case ShotSoundType.NO_AMMO:
                PlaySound(AssetManager.Get_Clip_NoAmmoShotSound(), worldPosition, volume);

                return;
        }
    }

    /**
     * Play throw sound
     */
    public static void PlayThrowSound(Vector3 worldPosition, float volume = 1f)
    {
        PlaySound(AssetManager.Get_Clip_ThrowSound(), worldPosition, volume);
    }

    /**
     * Play damage sound
     */
    public static void PlayDamageSound(bool hasArmor, Vector3 soundPosition, float volume = _MAX_VOLUME)
    {
        if (hasArmor) PlaySound(AssetManager.Get_Clip_DamageArmorSound(), soundPosition, volume);
        PlaySound(AssetManager.Get_Clip_DamageSound(), soundPosition, volume);
    }

    /**
     * Play the environment sound related to the given environment sound type
     */
    public static void PlayEnvironmentSound(EnvironmentSoundType type, float volume = _MAX_VOLUME)
    {
        switch (type)
        {
            case EnvironmentSoundType.BIRD:
                PlaySound(AssetManager.Get_Clip_BirdSound(), volume, true);

                return;
            case EnvironmentSoundType.WIND:
                PlaySound(AssetManager.Get_Clip_WindSound(), volume, true);

                return;
        }
    }

    /**
     * Play dead sound
     */
    public static void PlayDeadSound(Vector3 deadPosition, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_DeadSound(), deadPosition, volume);
    }

    /**
     * Play pickup sound
     */
    public static void PlayPickupSound(Vector3 pickupPosition, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_PickSound(), pickupPosition, volume);
    }

    /**
     * Play ingestible sound
     */
    public static void PlayIngestibleSound(Vector3 usePosition, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_IngestibleSound(), usePosition, volume);
    }

    /**
     * Play select weapon sound
     */
    public static void PlaySelectWeaponSound(Vector3 usePosition, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_SelectWeaponSound(), usePosition, volume);
    }

    /**
     * Play reload sound
     */
    public static void PlayReloadSound(Vector3 weaponPosition, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_ReloadSound(), weaponPosition, volume);
    }

    /**
     * Play low health sound
     */
    public static void PlayLowHealthSound(string entityName, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_LowHealthSound(), volume, true, entityName);
    }

    /**
     * Play low health sound in 3D
     */
    public static void PlayLowHealthSound(Vector3 entityPosition, string entityName, float volume = _MAX_VOLUME)
    {
        PlaySound(AssetManager.Get_Clip_LowHealthSound(), entityPosition, volume, true, entityName);
    }

    /**
     * Stop low health sound
     */
    public static void StopLowHealthSound(string entityName)
    {
        StopSound(entityName);
    }
}