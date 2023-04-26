using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] NewButton[] buttons;
    [SerializeField] private TextMeshProUGUI _scoreText, 
    _bestScoreText, 
    _goldText, 
    _goldBonusText,
    _goldGainForPlayText;
    [SerializeField] Image fade;
    [SerializeField] RectTransform player;
    [SerializeField] GameObject bestScoreMedal;
    [SerializeField] private ExpBar _expBar;


    
    private void Start()
    {
        GameManager.Instance.shopFromRestart = true;
        Time.timeScale = 1;
        //bestScoreMedal.SetActive(GameManager.Instance.CheckBestScore());
        SetScoreTexts();
        _goldText.SetText(GameManager.Instance.Gold.ToString());
        _expBar.SetStartExp();
        StartCoroutine(GameOverCoroutine());


    }

    private IEnumerator GameOverCoroutine()
    {
        string goldGainForPlayText = GameManager.Instance.GoldGainForPlay.ToString();
        _goldGainForPlayText.SetText(goldGainForPlayText);
        yield return new WaitForSecondsRealtime(2f);
        _expBar.UpdateBar();
        yield return StartCoroutine(_expBar.UpdateBar());
        GameManager.Instance.UpgradeGold();
        _goldText.SetText(GameManager.Instance.Gold.ToString());
        Vector3 scale = _goldText.transform.localScale;
        goldGainForPlayText = GameManager.Instance.GoldGainForPlay.ToString();
        _goldBonusText.SetText("+" + goldGainForPlayText);
        var sequence = DOTween.Sequence()
            .Append(_goldBonusText.rectTransform.DOAnchorPos(new Vector2(235, -120), 0.5f))
            .Append(_goldText.transform.DOScale(scale * 1.3f, 0.5f))
            .Append(_goldText.transform.DOScale(scale, 0.5f))
            .Append(player.DOAnchorPos(player.anchoredPosition - new Vector2(0, 1000), 0.5f))
            .OnComplete(() =>
            {
                foreach (NewButton button in buttons)
                {
                    button.transform.DOScale(Vector3.one, 0.5f);
                }
            });
        if (goldGainForPlayText != _goldGainForPlayText.text)
        {
            _goldGainForPlayText.SetText(goldGainForPlayText);
            var goldGainSequence = DOTween.Sequence()
                .Append(_goldGainForPlayText.transform.DOScale(Vector3.one * 1.3f, 0.5f))
                .Append(_goldGainForPlayText.transform.DOScale(Vector3.one, 0.5f));
        }
        yield return new WaitForSecondsRealtime(sequence.Duration() + 1f);
        foreach (NewButton button in buttons)
        {
            button.interactable = true;
        }
        
    }

    public void SetScoreTexts()
    {
        //_scoreText.SetText($"Очки : {GameManager.Instance.CurrentScore}");
        _bestScoreText.SetText($"Рекорд : {GameManager.Instance.BestScore}");
    }

    public void RestartGame()
    {
        fade.DOFade(1,1f);
        StartCoroutine(RestartGameCoroutine());
    }

    private IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.3f);
        GameManager.LoadGame();
    }

    


}
