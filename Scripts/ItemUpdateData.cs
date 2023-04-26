
[System.Serializable]
public class ItemUpdateData
{
    public int id; 
    public bool[] isUpgradeGained;

    public ItemUpdateData(ItemToUpgradeSO itemToUpgrade)
    {
        id = itemToUpgrade.ID;
        isUpgradeGained = new bool[4];
    }

}
