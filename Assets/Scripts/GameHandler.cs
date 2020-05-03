using UnityEngine;
using JoaDev;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          GameHandler class
 * ------------------------------------------------
 */

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private CameraController _camera;

    private static float _MAX_RADIUS;

    private const int _ENTITY_MAX_SPAWNS = 80;
    private const int _BIRD_MAX_SPAWNS = 50;
    private const int _CLOUD_MAX_SPAWNS = 10;
    private const int _POTION_MAX_SPAWNS = 45;
    private const int _SUPER_POTION_MAX_SPAWNS = 25;
    private const int _SPEED_POTION_MAX_SPAWNS = 35;
    private const int _ARMOR_POTION_MAX_SPAWNS = 45;
    private const int _BURST_RIFLE_MAX_SPAWNS = 20;
    private const int _HUNTING_RIFLE_MAX_SPAWNS = 30;
    private const int _ENFORCER_MAX_SPAWNS = 20;
    private const int _GRENADE_MAX_SPAWNS = 40;

    private const float _SPACE_FACTOR = 10f;
    private const float _SPAWN_BIRD_X = -180f;
    private const float _SPAWN_BIRD_Y = 80f;
    private const float _SPAWN_CLOUD_X = -180f;
    private const float _SPAWN_CLOUD_Y = 100f;
    private const float _MIN_WALK_SPEED = 5f;
    private const float _MAX_WALK_SPEED = 10f;
    private const float _MIN_RUN_SPEED = 15f;
    private const float _MAX_RUN_SPEED = 20f;
    private const float _THROW_IMPULSE_FORCE = 60f;

    private void Start()
    {
        ComputeDynamicParameters();
        InstantiateGame();
    }

    private void ComputeDynamicParameters()
    {
        _MAX_RADIUS = AssetManager.GetTerrain().localScale.x / 2 - AssetManager.Get_Prefab_Wall().localScale.z / 2 - _SPACE_FACTOR;
    }

    /**
     * Instantiate the game
     */
    private void InstantiateGame()
    {
        _camera.SetGetTargetPosition(null);
        SpawnEntities();
        SpawnEnvironmentComponents();
        SpawnPickables();
        PlayBackgroundSounds();
    }

    /**
     * Spawn entities in the arena
     */
    private void SpawnEntities()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (EntityManager.Count() < _ENTITY_MAX_SPAWNS)
                {
                    Vector3 entityWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS), 
                        AssetManager.Get_Prefab_Entity().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y, 
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    Entity entity = Instantiate(
                        AssetManager.Get_Prefab_Entity(),
                        entityWorldPosition,
                        Quaternion.identity
                    );

                    HealthBar healthBar = Instantiate(
                        AssetManager.Get_Prefab_HealthBar(),
                        entity.transform.position + Vector3.up * .2f,
                        entity.transform.rotation
                    );

                    healthBar.transform.SetParent(entity.transform);

                    entity.Setup(string.Format("Entity_{0}", EntityManager.Count().ToString()), 100f, 100f, 0f, 100f, Random.Range(_MIN_WALK_SPEED, _MAX_WALK_SPEED), Random.Range(_MIN_RUN_SPEED, _MAX_RUN_SPEED), _THROW_IMPULSE_FORCE, healthBar);

                    EntityManager.Add(entity);
                }
            }, 
            "SpawnEntities", 
            0f, 
            0f,
            () =>
            {
                if (EntityManager.Count() >= _ENTITY_MAX_SPAWNS) Destroy(GameObject.Find("SpawnEntities"));
            }
        );
    }

    /**
     * Spawn the environment
     */
    private void SpawnEnvironmentComponents()
    {
        SpawnBirds();
        SpawnClouds();
    }

    /**
     * Spawn some birds in the air
     */
    private void SpawnBirds()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (EnvironmentManager.ComponentCount<Bird>() < _BIRD_MAX_SPAWNS)
                {
                    Vector3 birdWorldPosition = new Vector3(
                        _SPAWN_BIRD_X, 
                        0f,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    Bird bird = Instantiate(
                        AssetManager.Get_Prefab_Bird(),
                        birdWorldPosition,
                        Quaternion.identity
                    );

                    
                    EnvironmentManager.AddComponent(bird);
                }
            },
            "SpawnBirds",
            0f,
            0f
        );
    }

    /**
     * Spawn some clouds in the air
     */
    private void SpawnClouds()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (EnvironmentManager.ComponentCount<Cloud>() < _CLOUD_MAX_SPAWNS)
                {
                    Vector3 cloudWorldPosition = new Vector3(
                        _SPAWN_CLOUD_X, 
                        0f,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    Cloud cloud = Instantiate(
                        AssetManager.Get_Prefab_Cloud(),
                        cloudWorldPosition,
                        Quaternion.identity
                    );

                    EnvironmentManager.AddComponent(cloud);
                }
            },
            "SpawnClouds",
            0f,
            0f
        );
    }

    /**
     * Spawn pickable items in the arena
     */
    private void SpawnPickables()
    {
        SpawnIngestibles();
        SpawnWeapons();
    }

    /**
     * Spawn ingestibles in the arena
     */
    private void SpawnIngestibles()
    {
        SpawnPotions();
        SpawnSuperPotions();
        SpawnSpeedPotions();
        SpawnArmorPotions();
    }

    /**
     * Spawn potions in the area
     */
    private void SpawnPotions()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (IngestibleManager.IngestibleCount<Potion>() < _POTION_MAX_SPAWNS)
                {
                    Vector3 potionWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_Potion().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    Potion potion = Instantiate(
                        AssetManager.Get_Prefab_Potion(),
                        potionWorldPosition,
                        Quaternion.identity
                    );

                    IngestibleManager.AddIngestible(potion);
                }
            },
            "SpawnPotions",
            0f,
            0f,
            () =>
            {
                if (IngestibleManager.IngestibleCount<Potion>() >= _POTION_MAX_SPAWNS) Destroy(GameObject.Find("SpawnPotions"));
            }
        );
    }

    /**
    * Spawn SuperPotions in the area
    */
    private void SpawnSuperPotions()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (IngestibleManager.IngestibleCount<SuperPotion>() < _SUPER_POTION_MAX_SPAWNS)
                {
                    Vector3 superPotionWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_SuperPotion().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    SuperPotion superPotion = Instantiate(
                        AssetManager.Get_Prefab_SuperPotion(),
                        superPotionWorldPosition,
                        Quaternion.identity
                    );

                    IngestibleManager.AddIngestible(superPotion);
                }
            },
            "SpawnSuperPotions",
            0f,
            0f,
            () =>
            {
                if (IngestibleManager.IngestibleCount<SuperPotion>() >= _SUPER_POTION_MAX_SPAWNS) Destroy(GameObject.Find("SpawnSuperPotions"));
            }
        );
    }

    /**
    * Spawn SpeedPotions in the area
    */
    private void SpawnSpeedPotions()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (IngestibleManager.IngestibleCount<SpeedPotion>() < _SPEED_POTION_MAX_SPAWNS)
                {
                    Vector3 speedPotionWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_SpeedPotion().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    SpeedPotion speedPotion = Instantiate(
                        AssetManager.Get_Prefab_SpeedPotion(),
                        speedPotionWorldPosition,
                        Quaternion.identity
                    );

                    IngestibleManager.AddIngestible(speedPotion);
                }
            },
            "SpawnSpeedPotions",
            0f,
            0f,
            () =>
            {
                if (IngestibleManager.IngestibleCount<SpeedPotion>() >= _SPEED_POTION_MAX_SPAWNS) Destroy(GameObject.Find("SpawnSpeedPotions"));
            }
        );
    }

    /**
     * Spawn armor potions in the area
     */
    private void SpawnArmorPotions()
    {
        FunctionPeriodic.Create(
            () =>
            {
                if (IngestibleManager.IngestibleCount<ArmorPotion>() < _ARMOR_POTION_MAX_SPAWNS)
                {
                    Vector3 armorPotionWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_ArmorPotion().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    ArmorPotion armorPotion = Instantiate(
                        AssetManager.Get_Prefab_ArmorPotion(),
                        armorPotionWorldPosition,
                        Quaternion.identity
                    );

                    IngestibleManager.AddIngestible(armorPotion);
                }
            },
            "SpawnArmorPotions",
            0f,
            0f,
            () =>
            {
                if (IngestibleManager.IngestibleCount<ArmorPotion>() >= _ARMOR_POTION_MAX_SPAWNS) Destroy(GameObject.Find("SpawnArmorPotions"));
            }
        );
    }

    /**
     * Spawn weapons in the area
     */
    private void SpawnWeapons()
    {
        SpawnBurstRifles();
        SpawnHuntingRifles();
        SpawnEnforcers();
        SpawnGrenades();
    }

    /**
     * Spawn burst rifles in the area
     */
    private void SpawnBurstRifles()
    {
        string funcName = "SpawnBurstRifles";

        FunctionPeriodic.Create(
            () =>
            {
                if (WeaponManager.WeaponCount<BurstRifle>() < _BURST_RIFLE_MAX_SPAWNS)
                {
                    Vector3 burstRifleWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_BurstRifle().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    BurstRifle burstRifle = Instantiate(
                        AssetManager.Get_Prefab_BurstRifle(),
                        burstRifleWorldPosition,
                        Quaternion.identity
                    );

                    WeaponManager.AddWeapon(burstRifle);
                }
            },
            funcName,
            0f,
            0f,
            () =>
            {
                if (WeaponManager.WeaponCount<BurstRifle>() >= _BURST_RIFLE_MAX_SPAWNS) Destroy(GameObject.Find(funcName));
            }
        );
    }

    /**
     * Spawn hunting rifles in the area
     */
    private void SpawnHuntingRifles()
    {
        string funcName = "SpawnHuntingRifles";

        FunctionPeriodic.Create(
            () =>
            {
                if (WeaponManager.WeaponCount<HuntingRifle>() < _HUNTING_RIFLE_MAX_SPAWNS)
                {
                    Vector3 huntingRifleWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_HuntingRifle().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    HuntingRifle huntingRifle = Instantiate(
                        AssetManager.Get_Prefab_HuntingRifle(),
                        huntingRifleWorldPosition,
                        Quaternion.identity
                    );

                    WeaponManager.AddWeapon(huntingRifle);
                }
            },
            funcName,
            0f,
            0f,
            () =>
            {
                if (WeaponManager.WeaponCount<HuntingRifle>() >= _HUNTING_RIFLE_MAX_SPAWNS) Destroy(GameObject.Find(funcName));
            }
        );
    }

    /**
     * Spawn enforcers in the area
     */
    private void SpawnEnforcers()
    {
        string funcName = "SpawnEnforcers";

        FunctionPeriodic.Create(
            () =>
            {
                if (WeaponManager.WeaponCount<Enforcer>() < _ENFORCER_MAX_SPAWNS)
                {
                    Vector3 enforcerWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_Enforcer().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    Enforcer enforcer = Instantiate(
                        AssetManager.Get_Prefab_Enforcer(),
                        enforcerWorldPosition,
                        Quaternion.identity
                    );

                    WeaponManager.AddWeapon(enforcer);
                }
            },
            funcName,
            0f,
            0f,
            () =>
            {
                if (WeaponManager.WeaponCount<Enforcer>() >= _ENFORCER_MAX_SPAWNS) Destroy(GameObject.Find(funcName));
            }
        );
    }

    /**
     * Spawn greandes in the area
     */
    private void SpawnGrenades()
    {
        string funcName = "SpawnGrenades";

        FunctionPeriodic.Create(
            () =>
            {
                if (WeaponManager.WeaponCount<Grenade>() < _GRENADE_MAX_SPAWNS)
                {
                    Vector3 grenadeWorldPosition = new Vector3(
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS),
                        AssetManager.Get_Prefab_Grenade().transform.localScale.y / 2 + AssetManager.GetTerrain().localScale.y,
                        Random.Range(-_MAX_RADIUS, _MAX_RADIUS)
                    );

                    Grenade grenade = Instantiate(
                        AssetManager.Get_Prefab_Grenade(),
                        grenadeWorldPosition,
                        Quaternion.identity
                    );

                    WeaponManager.AddWeapon(grenade);
                }
            },
            funcName,
            0f,
            0f,
            () =>
            {
                if (WeaponManager.WeaponCount<Grenade>() >= _GRENADE_MAX_SPAWNS) Destroy(GameObject.Find(funcName));
            }
        );
    }

    /**
     * Play background sounds
     */
    private void PlayBackgroundSounds()
    {
        SoundManager.PlayEnvironmentSound(SoundManager.EnvironmentSoundType.BIRD);
        SoundManager.PlayEnvironmentSound(SoundManager.EnvironmentSoundType.WIND);
    }
}
