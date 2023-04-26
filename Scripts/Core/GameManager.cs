using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Bolt
//Bomb
//Star
//Potion
//Strawberry
//Shield
//Mango




public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public bool Test;
    [field: SerializeField] public ItemToUpgradeSO[] ItemsToUpgrade {get; private set;}
    public List<ItemUpdateData> ItemUpdateDataList = new List<ItemUpdateData>();
    [SerializeField] private Color _darkSkyColor;
    [HideInInspector] public bool shopFromRestart;
    private Vector3 _screenBounds;
    public Vector3 ScreenBounds
    {
        get
        {
            return _screenBounds;
        }
    }
    //[field: SerializeField] public int CurrentScore {get; set;}
    [field: SerializeField] public int BestScore {get; set;}
    [field: SerializeField] public int Gold {get; set;}
    //[SerializeField] private int[] upgradeLevel = {0, 0, 0, 0, 0};
    [field: SerializeField] public int ShieldAmount {private set; get;}
    [field: SerializeField] public int ShieldAmountMax {private set; get;}
    [field: SerializeField] public int BoltAmount {private set; get;}
    [field: SerializeField] public int BoltAmountMax {private set; get;}

    [field: SerializeField] public bool PlayingFirstTime {get; private set;}
    [field: SerializeField] public int Level {get; private set;}
    public int GoldGainForPlay 
    {
        get
        {
            return _goldGainsForPlay[Level];
        }
    }
    private int _currentPlayGroundIndex;
    public float Experience
    {
        get
        {
            return _experience;
        }
        set
        {
            if (value <= 0) return;
            _experience = value;
            while (_experience >= ExperienceForLevel(Level + 1))
                Level++;
        }
        
    }
    [SerializeField] private float _experience;
    [SerializeField] private int[] _goldGainsForPlay;

    private Dictionary<int, ItemUpdateData> ItemUpdateDatas = new Dictionary<int, ItemUpdateData>();



    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RestoreData();
    }

    private void Start()
    {
        foreach (ItemToUpgradeSO itemToUpgrade in ItemsToUpgrade)
        {
            ItemUpdateData itemUpdateData = new ItemUpdateData(itemToUpgrade);
            ItemUpdateDatas.Add(itemToUpgrade.ID, itemUpdateData);
            ItemUpdateDataList.Add(itemUpdateData);
        }
    }

    public bool IsItemUpdateGained(int itemID, int updateIndex)
    {
        ItemUpdateData itemUpdateData = ItemUpdateDatas.GetValueOrDefault(itemID);
        return itemUpdateData.isUpgradeGained[updateIndex];
    }

    public void GainItemUpdate(int itemID, int updateIndex)
    {
        ItemUpdateData itemUpdateData = ItemUpdateDatas.GetValueOrDefault(itemID);
        itemUpdateData.isUpgradeGained[updateIndex] = true;
    }
    
    public static void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public static void LoadInfo()
    {
        SceneManager.LoadScene("InfoMenu");
    }

    public static void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }

    // public void SetScore (int value)
    // {
    //     if (value == 0) return;
    //     CurrentScore = Mathf.Clamp(CurrentScore + value, 0, int.MaxValue);
    //     UIManager.Instance.UpdateScore(CurrentScore);
    //     if (value < 0)
    //     {
    //         UIManager.Instance.MarkScore(Color.red);
    //     }
    // }

    public void StopGame()
    {
        Time.timeScale = 0;
        //UIManager.Instance.FadeIn();
        StartCoroutine(GameOverCoroutine());
        
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        GameOver();
    }

    // public bool CheckBestScore()
    // {
    //     if (CurrentScore > BestScore)
    //     {
    //         BestScore = CurrentScore;
    //         return true;
    //     }
    //     return false;
    // }

    public void SetScreenBounds()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(
        new Vector3(Screen.width, Screen.height, 0));
    }

    // public void SetUpgradeLevel(int index, int value)
    // {
    //     Mathf.Clamp(upgradeLevel[index] + value, 0, 4);
    // }
    public Color DarkSkyColor()
    {
        return _darkSkyColor;
    }

    public void UpgradeGold()
    {
        Gold += GoldGainForPlay;
    }

    public void SaveData()
    {
        if (Test) return;
        SaveGameManager.currentSaveData.gold = Gold;
        SaveGameManager.currentSaveData.bestScore = BestScore;
        //SaveGameManager.currentSaveData.upgradeLevel = upgradeLevel;
        SaveGameManager.Save();

    }

    public void RestoreData()
    {
        if (Test) return;
        SaveGameManager.Load();
        Gold = SaveGameManager.currentSaveData.gold;
        BestScore = SaveGameManager.currentSaveData.bestScore;
        //upgradeLevel = SaveGameManager.currentSaveData.upgradeLevel;

    }

    public void RemoveData()
    {
        SaveGameManager.Flush();
        Gold = 0;
        //upgradeLevel = new int[] {0,0,0,0,0};
        BestScore = 0;
    }

    public void SetBoltAmount(int value)
    {
        BoltAmount = Mathf.Clamp(value, 0, BoltAmountMax);
        // if (UIManager.Instance != null)
        //     UIManager.Instance.UpgradeBoltAmount();

    }

    public void SetBoltAmountMax(int value)
    {
        BoltAmountMax = Mathf.Max(value, 0);
    }

    public void SetShieldAmount(int value)
    {
        ShieldAmount = Mathf.Clamp(value, 0, ShieldAmountMax);
        // if (UIManager.Instance != null)
        //     UIManager.Instance.UpgradeShieldAmount();
    }

    public void SetShieldAmountMax(int value)
    {
        ShieldAmountMax = Mathf.Max(value, 0);
    }

    public float ExperienceForLevel(int level)
    {
        if (level < 1) return 0;
        return 200 + 100 * (level) + 50 * (level) * level;
    }


}
