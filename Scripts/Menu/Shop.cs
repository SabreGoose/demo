using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    
    private GameManager _gameManager;
    [SerializeField] private GameObject _selectItemPanel;
    [SerializeField] private GameObject _mainPanel;
    private ItemToUpgradeSO _currentItemToUpgrade;
    private int _currentUpgradeIndex;
    [SerializeField] private GameObject _selectItemButtonPrefab;
    [SerializeField] private Transform _selectItemButtonLayout;
    [SerializeField] private NewButton _buyButton;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _expImage;
    [Header("Information")]
    [SerializeField] private TextMeshProUGUI _itemName; 
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI  _priceAmountText;
    [SerializeField] private GameObject _priceAmountBlock;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _purchaseImage;
    [SerializeField] private Sprite _purchaseSpriteBlocked;
    [SerializeField] private Sprite _purchaseSpriteBought;
    [Header("Upgrades")]
    [SerializeField] private Image[] _upgradeImages;
    [SerializeField] private Image[] _upgradeLevelImages;
    [SerializeField] private TextMeshProUGUI[] _upgradeReqLevelText;
    [SerializeField] private Sprite _upgradeSpriteBlocked, _upgradeSpriteBasic;
    [SerializeField] private Sprite _upgradeGainedSprite, _upgradeNotGainedSprite, _upgrainedNotReadyToGainSprite;
    [SerializeField] private Color _upgradeSelectionColor;


    
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _priceAmountBlock.SetActive(false);
        CreateSelectItemButtons();
        SelectItemToUpgrade(0);
        _goldText.text = _gameManager.Gold.ToString();
        int level = _gameManager.Level;
        _levelText.text = $"Уровень {level}";
        _expImage.fillAmount = _gameManager.Experience / _gameManager.ExperienceForLevel(level + 1);
    }

    private void CreateSelectItemButtons()
    {
        for (int i = 0; i <  _gameManager.ItemsToUpgrade.Length; i++ )
        {
           int n = i;
           Transform transform = Instantiate(_selectItemButtonPrefab,_selectItemButtonLayout).transform;
           Image image = transform.GetChild(0).GetComponent<Image>();
           image.sprite = _gameManager.ItemsToUpgrade[i].MainImage;
           Button button = transform.GetComponent<Button>();
           button.onClick.AddListener(() => {SelectItemToUpgrade(n);});
           TextMeshProUGUI name = transform.GetComponentInChildren<TextMeshProUGUI>();
           name.text = _gameManager.ItemsToUpgrade[i].Name;
        }
    }

    public void ShowSelectItemMenu(bool state)
    {
        _selectItemPanel.SetActive(state);
        _mainPanel.SetActive(!state);
    }

    public void SelectItemToUpgrade(int index)
    {
        Debug.Log(index);
        _currentItemToUpgrade = _gameManager.ItemsToUpgrade[index];
        _itemName.text = _currentItemToUpgrade.Name;
        _itemIcon.sprite = _currentItemToUpgrade.MainImage;
        ShowSelectItemMenu(false);
        UpdateUpgrades();
        SelectUpgrade(0);
    }

    private void UpdateUpgrades()
    {
        int level = _gameManager.Level;
        for (int i = 0; i < _upgradeImages.Length; i++)
        {
            
            if (level < _currentItemToUpgrade.GetUpdateRequireableLevel(i))
            {
                _upgradeImages[i].sprite = _upgradeSpriteBlocked;
                _upgradeLevelImages[i].sprite = _upgrainedNotReadyToGainSprite;
                _upgradeReqLevelText[i].enabled = true;
                _upgradeReqLevelText[i].text = _currentItemToUpgrade.GetUpdateRequireableLevel(i).ToString();
            }
            else
            {
                _upgradeReqLevelText[i].enabled = false;
                if (_gameManager.IsItemUpdateGained(_currentItemToUpgrade.ID,i))
                    _upgradeLevelImages[i].sprite = _upgradeGainedSprite;
                else
                    _upgradeLevelImages[i].sprite = _upgradeNotGainedSprite;
                
                if (_currentItemToUpgrade.GetUpdateSprite(i) != null)
                {
                    _upgradeImages[i].sprite = _currentItemToUpgrade.GetUpdateSprite(i);
                    continue;
                }
                _upgradeImages[i].sprite = _upgradeSpriteBasic;
            }
        }
    }


    public void SelectUpgrade(int index)
    {
        _buyButton.ShowButton(false);
        _buyButton.hideAfterClick = true;
        _upgradeImages[_currentUpgradeIndex].color = Color.white;
        _upgradeImages[index].color = _upgradeSelectionColor;
        _upgradeReqLevelText[_currentUpgradeIndex].color = Color.white;
        _upgradeReqLevelText[index].color = _upgradeSelectionColor;
        _currentUpgradeIndex = index;
        _upgradeDescriptionText.SetText(_currentItemToUpgrade.GetUpgradeDescription(index));
        int requireableLevel = _currentItemToUpgrade.GetUpdateRequireableLevel(index);
        if (_gameManager.Level < requireableLevel)
        {
            _priceText.fontSize = 90;
            _priceText.text = $"Необходим уровень {requireableLevel}";
            _priceAmountBlock.SetActive(false);
            _purchaseImage.sprite = _purchaseSpriteBlocked;
            return;
        }
        if(_gameManager.IsItemUpdateGained(_currentItemToUpgrade.ID, index))
        {
            _priceText.fontSize = 100;
            _priceAmountBlock.SetActive(false);
            _priceText.text = "Уже куплено";
            _purchaseImage.sprite = _purchaseSpriteBought;
            return;
        }
        _priceText.fontSize = 90;
        _priceText.text = "Стоимость:";
        _priceAmountBlock.SetActive(true);
        _purchaseImage.sprite = _purchaseSpriteBought;
        if (_gameManager.Gold >= _currentItemToUpgrade.GetUpdatePrice(index))
            _buyButton.ShowButton(true);

    }

    public void CloseBackButtonClick()
    {
        if (_selectItemPanel.activeSelf)
        {
            ShowSelectItemMenu(false);
            return;
        }
    }

    public void BuyButtonClick()
    {
        _gameManager.Gold -= _currentItemToUpgrade.GetUpdatePrice(_currentUpgradeIndex);
        _gameManager.GainItemUpdate(_currentItemToUpgrade.ID, _currentUpgradeIndex);
        _goldText.text = _gameManager.Gold.ToString();
        _priceText.fontSize = 100;
        _priceAmountBlock.SetActive(false);
        _priceText.text = "Уже куплено";
        _upgradeLevelImages[_currentUpgradeIndex].sprite = _upgradeGainedSprite;
    }
}
