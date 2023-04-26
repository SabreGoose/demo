using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float speedIncreasePerSecond;
    public List<Item> SpawnedItems { get; private set; }
    public float speedIncrease {private set; get;}
    private LevelManager levelManager;

    [SerializeField] ItemSpawner fruitSpawner;
    [SerializeField] ItemSpawner bombSpawner;
    [SerializeField] ItemSpawner sodaSpawner;
    [SerializeField] ItemSpawner starSpawner;
    [SerializeField] ItemSpawner strawberrySpawner;
    [SerializeField] ItemSpawner mangoSpawner;
    [SerializeField] ItemSpawner allergySpawner;
    [SerializeField] ItemSpawner specialItemSpawner;
    [SerializeField] private RangeInt fruitAmount;
    [field: SerializeField] public Range TimeBetweenSpawnRange { get; private set; }
    private Range lastSpawnPoint = new Range();
    private Range spawnBoundsX = new Range();
    public float ItemDestroyBoundY { get; private set; }

    private void Awake() 
    {
        speedIncrease = 0;
        SpawnedItems = new List<Item>();
        levelManager = FindObjectOfType<LevelManager>();
        spawnBoundsX.max = levelManager.ScreenBounds.x - 10f * ResolutionScaler.GetScaler();
        spawnBoundsX.min = spawnBoundsX.max * -1f;
        ItemDestroyBoundY = levelManager.ScreenBounds.y * -1 -4 * ResolutionScaler.GetScaler();
        InitializeSpawners();
        //Instance = this;
    }

    private void Start()
    {
        levelManager.GameStartedEvent.AddListener(StartSpawn);
        levelManager.BoltActivatedEvent.AddListener(BoltActivatedScaleSpeed);
        levelManager.BoltIsOverEvent.AddListener(BoltIsOverResetSpeed);
        
    }

    private void InitializeSpawners()
    {
        LevelInfoSO levelInfo = levelManager.LevelInfo;
        if (levelInfo is null) 
            return;
        if (levelInfo.specialItem != null)
        {
            specialItemSpawner.spawnController = levelInfo.specialItem;
            InitializeSpawner(levelInfo.specialItemAmount, specialItemSpawner);
        }
        InitializeSpawner(levelInfo.bombs, bombSpawner);
        InitializeSpawner(levelInfo.mangoes, mangoSpawner);
        InitializeSpawner(levelInfo.soda, sodaSpawner);
        InitializeSpawner(levelInfo.strawberries, strawberrySpawner);
        InitializeSpawner(levelInfo.stars, starSpawner);
        InitializeSpawner(fruitAmount, fruitSpawner);
        InitializeSpawner(levelInfo.allergy, allergySpawner);
    }

    private void InitializeSpawner(RangeInt amountRange, ItemSpawner itemSpawner)
    {
        if (amountRange.max <= 0)
            return;
        itemSpawner.amount = amountRange.GetRandom();
        itemSpawner.Initialize();
        levelManager.GameStartedEvent.AddListener(itemSpawner.StartSpawning);
        Debug.Log(itemSpawner.name + " initialized");
    }

    private IEnumerator FallingSpeedIncrease()
    {
        while(true)
        {
            speedIncrease += speedIncreasePerSecond * Time.deltaTime;
            yield return null;
        }
    }

    public void StartSpawn()
    {
        StartCoroutine(FallingSpeedIncrease());
        // ItemSpawner[] spawners = GetComponentsInChildren<ItemSpawner>();
        // foreach (ItemSpawner spawner in spawners)
        // {
        //     spawner.StartSpawning();
        // }
    }

    private void ScaleItemsVelocity(float value)
    {
        foreach (Item item in SpawnedItems)
        {
            item.MultiplyVelocity(value);
        }
    }

    private void BoltActivatedScaleSpeed()
    {
        ScaleItemsVelocity(levelManager.BoltSlowDownSpeed);
    }

    private void BoltIsOverResetSpeed()
    {
        foreach (Item item in SpawnedItems)
        {
            item.ResetVelocityToBase();
        }
    }


    public bool IsBoltActive()
    {
        return levelManager.IsBoltActive;
    }

    public float GetBoltSlowDownSpeed()
    {
        return levelManager.BoltSlowDownSpeed;
    }

    public float GetSpawnPoint()
    {
        float spawnPoint = 0f;
        do
        {
            spawnPoint = spawnBoundsX.GetRandom();
        }
        while(lastSpawnPoint.isFloatInRange(spawnPoint));
        lastSpawnPoint.min = spawnPoint - 10 * ResolutionScaler.GetScaler();
        lastSpawnPoint.max = spawnPoint + 10 * ResolutionScaler.GetScaler();
        return spawnPoint;
    }

}
