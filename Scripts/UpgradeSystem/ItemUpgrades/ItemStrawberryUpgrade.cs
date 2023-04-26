using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStrawberryUpgrade : ItemUpgrade
{
    [SerializeField] private float[] _bonusScale;
    [SerializeField] private float[] _speedDeacrease;
    [SerializeField] private float _bonusTime; 
    
    protected override void LevelOne()
    {
        _itemSpawner.StartSpeedRange.IncreaseRange(-_speedDeacrease[0]);
        _itemSpawner.scale += _bonusScale[0];
    }

    protected override void LevelThree()
    {
        _itemSpawner.StartSpeedRange.IncreaseRange(-_speedDeacrease[2]);
        _itemSpawner.scale += _bonusScale[2];
    }

    protected override void LevelTwo()
    {
        _itemSpawner.StartSpeedRange.IncreaseRange(-_speedDeacrease[1]);
        _itemSpawner.scale += _bonusScale[1];
    }

    protected override void LevelFour()
    {
        //_itemSpawner.TimeBetweenSpawnRange.IncreaseRange(-_bonusTime);
    }


}
