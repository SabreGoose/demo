// using System.Collections;
// using System.Collections.Generic;
//
using System;
using UnityEngine;

[RequireComponent(typeof(ItemSpawner))]
public abstract class ItemUpgrade : MonoBehaviour
{
    private event Action _upgrade;
    protected ItemSpawner _itemSpawner;
    protected SpawnManager _spawnManager;
    [field: SerializeField] public int UpgradeID {get; protected set;}
    

    private void Start() 
    {
        SetUpgrade();
        _itemSpawner = GetComponent<ItemSpawner>();
        _spawnManager = GetComponentInParent<SpawnManager>();
        if (_upgrade != null)
            _upgrade();
        
    }


    private void SetUpgrade()
    {
        // int upgradeLevel = GameManager.Instance.GetUpgradeLevel(UpgradeID);
        // if (upgradeLevel > 0) _upgrade += LevelOne;
        // if (upgradeLevel > 1) _upgrade += LevelTwo;
        // if (upgradeLevel > 2) _upgrade += LevelThree;
        // if (upgradeLevel > 3) _upgrade += LevelFour;
    }


    // private void InvokeUpgrade()
    // {
    //     _upgrade();
    // }
    protected abstract void LevelOne();
    protected abstract void LevelTwo();
    protected abstract void LevelThree();
    protected abstract void LevelFour();
    
}
