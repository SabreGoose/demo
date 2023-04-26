using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SpecialItemCollisions;

public class LevelManager : MonoBehaviour
{
    //Events
    public UnityEvent GameStartedEvent => gameStartedEvent;
    public UnityEvent ScoreChangedEvent => scoreChangedEvent;
    public UnityEvent GameTimerEvent => gameTimerEvent;
    public UnityEvent BoltTimerEvent => boltTimerEvent;
    public UnityEvent BoltActivatedEvent => boltActivatedEvent;
    public UnityEvent BoltIsOverEvent => boltIsOverEvent;
    public UnityEvent DoubleScoreEvent => doubleScoreEvent;
    public UnityEvent DoubleScoreTimerEvent => doubleScoreTimerEvent;
    public UnityEvent DoubleScoreIsOverEvent => doubleScoreIsOverEvent;
    public UnityEvent<Item> ItemCollectedEvent => itemCollectedEvent;
    public UnityEvent BoltEnabledEvent => boltEnabledEvent;

    private UnityEvent gameStartedEvent = new UnityEvent();
    private UnityEvent scoreChangedEvent = new UnityEvent();
    private UnityEvent gameTimerEvent = new UnityEvent();
    private UnityEvent boltTimerEvent = new UnityEvent();
    private UnityEvent boltActivatedEvent = new UnityEvent();
    private UnityEvent boltIsOverEvent = new UnityEvent();
    private UnityEvent doubleScoreEvent = new UnityEvent();
    private UnityEvent doubleScoreTimerEvent = new UnityEvent();
    private UnityEvent doubleScoreIsOverEvent = new UnityEvent();
    private UnityEvent<Item> itemCollectedEvent = new UnityEvent<Item>();
    private UnityEvent boltEnabledEvent = new UnityEvent();

    //Timers
    private float doubleScoreTimer;
    private int doubleScoreTimerInt;
    public int DoubleScoreTimer
    {
        get
        {
            return doubleScoreTimerInt;
        }
        private set
        {
            doubleScoreTimer = value;
            doubleScoreTimerInt = value;
        }
    }
    private float gameTimer;
    private int gameTimerInt;
    public int GameTimer
    {
        get
        {
            return gameTimerInt;
        }
        private set
        {
            gameTimer = value;
            gameTimerInt = value;
        }
    }
    private float boltTimer;
    private int boltTimerInt;
    public int BoltTimer
    {
        get
        {
            return boltTimerInt;
        }
        private set
        {
            boltTimer = value;
            boltTimerInt = value;
        }
    }
    

    private float timeScale;
    public int CurrentScore { get; private set; }
    public bool IsGameStarted { get; private set; }
    private GameManager gameManager;
    [SerializeField] private ItemSpawner _allergySpawner;
    [SerializeField] private ItemSpawner _fruitSpawner;
    [SerializeField] private float _startDelay;
    [SerializeField] private int boltTimerMax;
    [field: SerializeField] public float BoltCoolDown { get; private set;}
    [field: SerializeField] public float BoltSlowDownSpeed { get; private set; }
    [field: SerializeField] public float BoltCoolDownTimer { get; private set; }
    [field: SerializeField] public bool IsBoltActive { get; private set; }
    [field: SerializeField] public bool isBoltEnabled { get; private set; }

    [SerializeField] private int boltAmount;
    
    private bool isGamePaused;
    public Vector3 ScreenBounds { get; private set; }

    [field: SerializeField] public int GameTimeMax { get; private set; }
    
    public int BoltAmount { get; private set; }
    public GameObject AlelergyPrefab { get; private set;}
    public bool DoubleScore { get; private set; }

    [field: SerializeField] public LevelInfoSO LevelInfo { get; private set; }

    private SpecialCollisions specialCollisions;
    [field: SerializeField] public float FreezeTimerMax { get; private set; }

    private void Awake() 
    {
        ScreenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)); 
        Timer timer = FindObjectOfType<Timer>();
        ResolutionScaler.SetScaler();
        Time.timeScale = 1;
        GameTimer = GameTimeMax;
        specialCollisions = GetComponent<SpecialCollisions>();
    }
    
    
    private void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
        //GameManager.Instance.SetScreenBounds();
        BoltAmount = gameManager.BoltAmount;
        SetAllergyFruit();
        StartCoroutine(StartGame());
    }

    private void Update() 
    {
  
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(_startDelay);
        //IsGameStarted = true;
        StartCoroutine(TimersCoroutine());
        //Debug.Log(_startDelay);
        //_gameTimer.StartTimer();
        //_spawnManager.StartSpawn();
        gameStartedEvent.Invoke();
    }

    public void PauseGame(bool state)
    {
        //uiManager.PauseMenu(state);
        if (state) 
        {
            timeScale = Time.timeScale;
            Time.timeScale = 0;
            return;
        }
        Time.timeScale = timeScale;

    }

    public void RestartGame()
    {
        //uiManager.FadeIn();
        //uiManager.PauseMenu(false);
        StartCoroutine(RestartGameCoroutine());
    }

    private IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        GameManager.LoadGame();
    }

    public void MainMenu()
    {
        //GameManager.Instance.CurrentScore = 0;
        GameManager.LoadMainMenu();
    }

    public void ActivateBolt()
    {
        // int boltAmount = GameManager.Instance.BoltAmount;
        // if (boltAmount <= 0 ) return;
        IsBoltActive = true;
        isBoltEnabled = false;
        BoltTimer = boltTimerMax;
        BoltCoolDownTimer = BoltCoolDown;
        boltTimerEvent.Invoke();
        boltActivatedEvent.Invoke();
        boltAmount -= 1;

        //HeartTimerManager.Instance.timeScale = boltSlowDownSpeed;
        //_gameTimer.TimeScale = _boltSlowDownSpeed;
        //_spawnManager.ScaleSpeed(_boltSlowDownSpeed);
        //GameManager.Instance.SetBoltAmount(boltAmount - 1);
    }

    // public void ActivateBolt()
    // {
    //     Time.timeScale = _boltSlowDownSpeed;
    //     _boltTimer = _boltTimerMax;
    //     BoltTimer = (int) _boltTimerMax;
    //     _boltCoolDownTimer = _boltCoolDown;
    //     _isBoltActive = true;
    // }

    private void BoltIsOver()
    {
        IsBoltActive = false;
        Time.timeScale = 1;
        boltTimerInt = 0;
        boltTimerEvent.Invoke();
        boltIsOverEvent.Invoke();
        //_gameTimer.TimeScale = 1f;
        //_spawnManager.ScaleSpeed(1 / _boltSlowDownSpeed);
    }

    // public float GetBoltSlowDownSpeed()
    // {
    //     return _boltSlowDownSpeed;
    // }

    private void SetAllergyFruit()
    {
        //GameObject allergyFruit = _fruitSpawner.GetRandomPrefab();
        AlelergyPrefab = _fruitSpawner.GetRandomPrefab();
        _allergySpawner.AddPrefabToSpawner(AlelergyPrefab);
        _fruitSpawner.RemovePrefabFromSpawner(AlelergyPrefab);
    }

    public void CollectItem(Item item)
    {
        itemCollectedEvent.Invoke(item);
        int doubleScoreMultiplier = 1;
        int negativeMultiplier = 1;
        if (item.DoubleScoreTime > 0)
            ActivateDoubleScore(item.DoubleScoreTime);
        if (DoubleScore)
            doubleScoreMultiplier = 2;
        if (item.IsNegative)
        {
            negativeMultiplier = -1;
            doubleScoreMultiplier = 1;
        }
        if (item.extraCollision != ExtraCollision.Empty)
            specialCollisions.RunSpecialCollision(item.extraCollision);
        ChangeScore(item.ScoreBonus * negativeMultiplier * doubleScoreMultiplier);
        ChangeGameTime(item.TimeBonus * negativeMultiplier);
        if (item.collisionEffectPrefab != null)
            EffectManager.CreateEffect(item.collisionEffectPrefab,
                item.transform.position, item.collisionEffectColor);
    }

    private void ActivateDoubleScore(int doubleScoreTime)
    {
        if (DoubleScore)
        {
            DoubleScoreTimer += doubleScoreTime;
            //doubleScoreTimerInt = (int) doubleScoreTimer;
            DoubleScoreTimerEvent.Invoke();
            return;
        }
        DoubleScore = true;
        doubleScoreTimer = doubleScoreTime;
        doubleScoreTimerInt = (int) doubleScoreTimer;
        doubleScoreEvent.Invoke();
    }

    public void ChangeScore (int value)
    {
        if (value == 0) return;
        CurrentScore = Mathf.Max(CurrentScore + value, 0);
        scoreChangedEvent.Invoke();
    }

    private void ChangeGameTime(int value)
    {
        if (value == 0) return;
        gameTimer = Mathf.Clamp(gameTimer + value, 0, GameTimeMax);
        gameTimerEvent.Invoke();
    }

    private IEnumerator TimersCoroutine()
    {
        while (true)
        {
            if (!isGamePaused)
            {
                UpdateTimer(ref gameTimer, ref gameTimerInt, gameTimerEvent, false);
                DoubleScoreTimerUpdate();
                BoltTimerUpdate();
 
            }
            yield return null;
        }
        
    }

    private void UpdateTimer(ref float timer, ref int timerIntPublic, UnityEvent secondPassedEvent, bool ignoreBolt)
    {
        float timeDecrease = Time.deltaTime;
        if (!ignoreBolt && IsBoltActive)
            timeDecrease *= BoltSlowDownSpeed;
        timer -= Time.deltaTime;
        int timerInt = (int) timer + 1;
        if (timerInt < timerIntPublic)
        {
            timerIntPublic = timerInt;
            if (secondPassedEvent != null)
                secondPassedEvent.Invoke();
        }
    }

    private void DoubleScoreTimerUpdate()
    {
        if (doubleScoreTimer > 0)
        {
            UpdateTimer(ref doubleScoreTimer, ref doubleScoreTimerInt, doubleScoreTimerEvent, true);
        }
        else if (DoubleScore)
        {
            DoubleScore = false;
            DoubleScoreIsOverEvent.Invoke();
        }
    }

    private void BoltTimerUpdate()
    {
        if (boltTimer > 0)
        {
            UpdateTimer(ref boltTimer, ref boltTimerInt, boltTimerEvent, true);
        }
        else if (IsBoltActive) 
            BoltIsOver();
        if (BoltCoolDownTimer > 0)
            if (boltAmount > 0)
                BoltCoolDownTimer -= Time.deltaTime;
        else if (!isBoltEnabled)
        {
            isBoltEnabled = true;
            boltEnabledEvent.Invoke();
        } 
    }

    public void RegisterSpecialCollisionEvent(ExtraCollision extraCollision, UnityAction call)
    {
        UnityEvent collisionEvent = specialCollisions.CollisionsDictionary.GetValueOrDefault(extraCollision);
        collisionEvent.AddListener(call);
    }
}
