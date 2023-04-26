using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_SpawnController", menuName = "ScriptableObjects/NewSpawnController", order = 1)]
public class SpawnControllerSO : ScriptableObject
{
    [SerializeField] private List<GameObject> spawnPrefabList = new List<GameObject>();
    [SerializeField] private Range timeToStartRange;
    [SerializeField] private Range startSpeedRange;
    [SerializeField] private bool isSpeedRising;
    [SerializeField] private float scale;
    [SerializeField] private bool isNegative;
    [SerializeField] private int scoreBonus;
    [SerializeField] private int timeBonus;
    [SerializeField] private int doubleScoreTime;
    [SerializeField] private ExtraCollision extraCollision; 

    public List<GameObject> SpawnPrefabList 
    {
        get 
        { 
            List<GameObject> list = new List<GameObject>();
            list.AddRange(spawnPrefabList);
            return list; 
        }
    }
    public Range TimeToStartRange => timeToStartRange;
    public Range StartSpeed => startSpeedRange;
    public bool IsSpeedRising => isSpeedRising;
    public float Scale => scale;
    public bool IsNegative => isNegative;
    public int ScoreBonus => scoreBonus;
    public int TimeBonus => timeBonus;
    public int DoubleScoreTime => doubleScoreTime;
    public ExtraCollision ExtraCollision => extraCollision;

}
