using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "ScriptableObjects/NewLevelInfo", order = 1)]
public class LevelInfoSO : ScriptableObject
{
    //[SerializeField] public ItemSpawnerGenerator[] ItemSpawnersGenerator;
    [SerializeField] public RangeInt bombs;
    [SerializeField] public RangeInt soda;
    [SerializeField] public RangeInt stars;
    [SerializeField] public RangeInt strawberries;
    [SerializeField] public RangeInt mangoes;
    [SerializeField] public RangeInt allergy;
    [SerializeField] public Sprite background;
    [SerializeField] public SpawnControllerSO specialItem;
    [SerializeField] public RangeInt specialItemAmount;

}

[System.Serializable]
public class ItemSpawnerGenerator
{
    [SerializeField] SpawnControllerSO spawnController;
    [SerializeField] int itemsAmount;
}
