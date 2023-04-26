using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform playButton;
    [SerializeField] private Image background, fade;
    [SerializeField] private TextMeshProUGUI[] menuTexts;

    //[SerializeField] private ParticleSystem[] startParticleSystems;
    [SerializeField] private ParticleSystem endParticleSystem;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] CanvasGroup lowCanvasGroup;
    
    
    private void Awake() 
    {
        Time.timeScale = 1;
    }
    
    private void Start()
    {
        GameManager.Instance.shopFromRestart = false;
        goldText.SetText(GameManager.Instance.Gold.ToString());
    }
    
    
    public void PlayButtonClick()
    {
        // foreach (ParticleSystem particleSystem in startParticleSystems)
        // {
        //     particleSystem.Stop();
        // }
        MainMenuParticles.Instance.Stop();
        foreach (TextMeshProUGUI text in menuTexts)
        {
            text.DOFade(0f, 0.5f);
        }
        lowCanvasGroup.DOFade(0f, 0.5f);
        StartCoroutine(StartGameCoroutine());
    }

    // public void ShopButtonClick()
    // {
    //     GameManager.LoadShop();
    // }

    private IEnumerator StartGameCoroutine()
    {
        //yield return new WaitForSecondsRealtime(0.5f);
        background.rectTransform.DOAnchorPos(new Vector2(0f, 1000f),1f);
        yield return new WaitForSecondsRealtime(2f);
        MainMenuParticles.Instance.Destroy();
        playButton.transform.DOScale(Vector3.zero,0.4f);
        fade.DOFade(1,endParticleSystem.main.duration * 0.25f).SetDelay(endParticleSystem.main.duration * 0.6f);
        endParticleSystem.Play();
        yield return new WaitForSecondsRealtime(endParticleSystem.main.duration);
        GameManager.LoadGame();

    }


}
