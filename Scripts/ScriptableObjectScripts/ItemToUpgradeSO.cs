using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemUpgardeInfo_", menuName = "ScriptableObjects/ItemUpdradeInfo", order = 1)]
public class ItemToUpgradeSO : ScriptableObject
{
    private static int currentId;
    [field: SerializeField] public int ID {get; private set;}
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public Sprite MainImage {get; private set;}
    [SerializeField] private UpgradeInfo[] _upgradeInfos = new UpgradeInfo[4];


    public string GetUpgradeDescription(int index)
    {
        return _upgradeInfos[index].description;
    }

    public int GetUpdatePrice(int index)
    {
        return _upgradeInfos[index].price;
    }

    public int GetUpdateRequireableLevel(int index)
    {
        return _upgradeInfos[index].requireableLevel;
    }

    public Sprite GetUpdateSprite(int index)
    {
        return _upgradeInfos[index].sprite;
    }


    public ItemToUpgradeSO()
    {
        ID = currentId;
        currentId++;
    }


}

[System.Serializable]
public class UpgradeInfo
{
    [TextArea(2,4)]
    public string description;
    public Sprite sprite;
    public int price;
    public int requireableLevel;
}
