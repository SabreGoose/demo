using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _fadeImage, _gameTimerImage, _boltCooldownImage, allergyFruitImage,
        backgroundDarkPanel, backgroundImage;
    [SerializeField] private TextMeshProUGUI 
        _scoreText, 
        _gameTimerText, 
        _boltTimerText,
        _boltAmountText,
        _shieldAmountText, 
        _scoreFlyingTextPrefab,
        doubleScoreTimerText;
    [SerializeField] private Canvas _pauseMenuCanvas;
    //public static UIManager Instance;
    private Camera _mainCamera;
    private LevelManager levelManager;
    [SerializeField] private GameObject doubleScoreTimer;
    [SerializeField] private Color boltActivatedColor;
    [SerializeField] private NewButton boltButton;

    private void Awake() 
    {
        //Instance = this;
        _mainCamera = Camera.main;
        levelManager = FindObjectOfType<LevelManager>();
        backgroundImage.sprite = levelManager.LevelInfo.background;    
    }
    
    void Start()
    {
        levelManager.GameStartedEvent.AddListener(Init);
        levelManager.GameTimerEvent.AddListener(UpdateGameTimer);
        levelManager.ScoreChangedEvent.AddListener(UpdateScore);
        levelManager.ItemCollectedEvent.AddListener(ItemCollected);
        levelManager.BoltTimerEvent.AddListener(UpdateBoltTimer);
        levelManager.BoltActivatedEvent.AddListener(BoltActivatedDarkBackground);
        levelManager.BoltIsOverEvent.AddListener(BoltIsOverDarkBackground);
        levelManager.DoubleScoreEvent.AddListener(() => { 
            doubleScoreTimer.SetActive(true); 
            UpdateDoubleScoreTimer(); });
        levelManager.DoubleScoreIsOverEvent.AddListener(() => { doubleScoreTimer.SetActive(false); });
        levelManager.DoubleScoreTimerEvent.AddListener(UpdateDoubleScoreTimer);
        levelManager.BoltEnabledEvent.AddListener(() => { boltButton.interactable = true; });
        _scoreText.SetText("0");
        Sprite allergyPrefabSprite = levelManager.AlelergyPrefab.GetComponent<SpriteRenderer>().sprite;
        allergyFruitImage.sprite = allergyPrefabSprite;
        FadeOut();
    }

    private void Update()
    {
        UpdateBoltCooldown();
    }
    
    
    public void FadeIn()
    {
        _fadeImage.DOFade(1f,0.8f).SetUpdate(true);
    }

    public void FadeOut()
    {
        _fadeImage.DOFade(0f,0.8f).SetUpdate(true);
    }

    public void Init()
    {
        //UpgradeBoltAmount();
        //UpgradeShieldAmount();
        //UpgradeBoltTimer(0);
    }

    private void ItemCollected(Item item)
    {
        Color color = Color.yellow;
        char sign = '+';
        if (item.IsNegative)
        {
            color = Color.red;
            sign = '-';
        }
        if (item.ScoreBonus > 1 || (item.IsNegative && item.ScoreBonus > 0))
        {
            CreateScoreFlyingText(sign + item.ScoreBonus.ToString(),
                Vector3ToAnchoredPosition(item.transform.position),
                color);
            MarkScore(color);
        }
        if (item.TimeBonus > 0)
            MarkTimer(color);
            
    }

    public void UpdateScore()
    {
        _scoreText.SetText(levelManager.CurrentScore.ToString());
    }

    public void UpdateGameTimer()
    {
        _gameTimerImage.fillAmount = (float) levelManager.GameTimer / levelManager.GameTimeMax;
        _gameTimerText.SetText(levelManager.GameTimer.ToString());     
    }

    public void MarkTimer(Color color)
    {
        _gameTimerText.color = color;
        _gameTimerText.DOColor(Color.white,1f);
    }

    public void MarkScore(Color color)
    {
        _scoreText.color = color;
        _scoreText.DOColor(Color.white,1f);
    }

    public void UpdateBoltTimer()
    {
        if (levelManager.BoltTimer <= 0)
        {
            _boltTimerText.fontSize = 0;
            return;
        }
        if (_boltTimerText.fontSize == 0) 
            _boltTimerText.fontSize = 140;
        _boltTimerText.SetText(levelManager.BoltTimer.ToString());
    }

    public void UpdateDoubleScoreTimer()
    {
        doubleScoreTimerText.SetText(levelManager.DoubleScoreTimer.ToString());
    }

    public void UpdateBoltCooldown()
    {
        _boltCooldownImage.fillAmount = levelManager.BoltCoolDownTimer / levelManager.BoltCoolDown;

    }

    public void UpgradeBoltAmount()
    {
        _boltAmountText.SetText(GameManager.Instance.BoltAmount.ToString());
    }

    public void UpgradeShieldAmount()
    {
        _shieldAmountText.SetText(GameManager.Instance.ShieldAmount.ToString());
    }

    public void ScoreFlyingText(int number, Vector3 pos,Color color)
    {
        string text = number.ToString();
        if (number > 0) text = string.Concat("+", text);
        CreateScoreFlyingText(text, Vector3ToAnchoredPosition(pos), color);

    }

    public void ScoreFlyingText(string text, Vector3 pos,Color color)
    {
        CreateScoreFlyingText(text, Vector3ToAnchoredPosition(pos), color);
    }

    private Vector2 Vector3ToAnchoredPosition(Vector3 pos)
    {
        Vector2 worldToScreenPos = RectTransformUtility.WorldToScreenPoint(_mainCamera, pos);
        Vector2 anchoredPosition = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _fadeImage.rectTransform,worldToScreenPos,null, out anchoredPosition);
        return anchoredPosition;
    }

    private void CreateScoreFlyingText(string text, Vector2 anchoredPosition, Color color)
    {
        TextMeshProUGUI scoreFlyingText = Instantiate(_scoreFlyingTextPrefab, _scoreFlyingTextPrefab.transform.parent);
        scoreFlyingText.rectTransform.anchoredPosition = anchoredPosition;
        scoreFlyingText.transform.SetSiblingIndex(0);
        scoreFlyingText.SetText(text);
        scoreFlyingText.color = color;
        scoreFlyingText.rectTransform.DOAnchorPos(anchoredPosition + new Vector2(0, 300f),1f);
        scoreFlyingText.DOFade(0, 1f);
        Destroy(scoreFlyingText.gameObject, 1.2f);
    }

    public void PauseMenu(bool state)
    {
        _pauseMenuCanvas.enabled = state;
    }

    private void OnDestroy()
    {
        //Instance = null;
    }

    private void BoltActivatedDarkBackground()
    {
        backgroundImage.DOColor(boltActivatedColor, 0.5f);
    }

    private void BoltIsOverDarkBackground()
    {

        backgroundImage.DOColor(Color.white, 0.5f);
    }

}
