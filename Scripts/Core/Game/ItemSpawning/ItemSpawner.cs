using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    
    //LevelManager levelManager;
    private SpawnManager spawnManager;
    public SpawnControllerSO spawnController;
    private List<GameObject> prefabList;
    [HideInInspector] public Range TimeToStartRange;
    [HideInInspector] public Range StartSpeedRange;
    private bool isSpeedRising;
    private bool isNegative;
    private float boltSlowDownSpeed;
    //[HideInInspector] 
    [HideInInspector] public float scale;
    [HideInInspector] public int scoreBonus;
    [HideInInspector] public int timeBonus;
    [HideInInspector] public int doubleScoreTime;
    [HideInInspector] public int amount;
    private float timeBetweenSpawn;
    private float timeToStart;
    private ExtraCollision extraCollision;

    private void Awake() 
    {
        spawnManager = GetComponentInParent<SpawnManager>();
    }

    private void Start()
    {
        boltSlowDownSpeed = spawnManager.GetBoltSlowDownSpeed();
    }

    public void Initialize()
    {
        prefabList = spawnController.SpawnPrefabList;
        TimeToStartRange = Range.CreateCopy(spawnController.TimeToStartRange);
        StartSpeedRange = Range.CreateCopy(spawnController.StartSpeed);
        isSpeedRising = spawnController.IsSpeedRising;
        isNegative = spawnController.IsNegative;
        timeBonus = spawnController.TimeBonus;
        scoreBonus = spawnController.ScoreBonus;
        scale = spawnController.Scale;
        doubleScoreTime = spawnController.DoubleScoreTime;
        timeToStart = TimeToStartRange.GetRandom();
        timeBetweenSpawn = (60f - timeToStart) / amount;
        extraCollision = spawnController.ExtraCollision;
        
    }

    public GameObject GetRandomPrefab()
    {
        int index = Random.Range(0, prefabList.Count);
        return prefabList[index];
    }

    public void AddPrefabToSpawner(GameObject prefab)
    {
        if (prefabList is null) Debug.Log("Trouble");
        prefabList.Add(prefab);
    }

    public void RemovePrefabFromSpawner(GameObject prefab)
    {
        prefabList.Remove(prefab);
    }
    
    public void StartSpawning()
    {
        if (prefabList.Count < 1) return;
        boltSlowDownSpeed = spawnManager.GetBoltSlowDownSpeed();
        StartCoroutine(SpawnCoroutine());
    }


    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(timeToStart);
        while (true)
        {
            float secondsBeforeNextSpawn = timeBetweenSpawn + spawnManager.TimeBetweenSpawnRange.GetRandom();
            GameObject prefab = GetRandomPrefab();
            float spawnX = spawnManager.GetSpawnPoint();
            Item instance = Instantiate(
                prefab, 
                new Vector3(spawnX, transform.position.y, 0), 
                prefab.transform.rotation)
                .GetComponent<Item>();
            instance.transform.localScale *= scale;
            float speed = StartSpeedRange.GetRandom();
            if (isSpeedRising) 
                speed += spawnManager.speedIncrease;
            instance.SetBaseVelocity(speed);
            if (spawnManager.IsBoltActive())
                instance.MultiplyVelocity(boltSlowDownSpeed);
            if (isNegative)
                instance.MakeItemNegative();
            instance.SetBonuses(scoreBonus, timeBonus, doubleScoreTime);
            instance.AddToSpawnList(spawnManager.SpawnedItems);
            instance.destroyBoundY = spawnManager.ItemDestroyBoundY;
            instance.extraCollision = extraCollision;
            yield return new WaitForSeconds(secondsBeforeNextSpawn);
        }
    }

}
