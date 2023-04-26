using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoMenu : MonoBehaviour
{
    [SerializeField] 
    [TextArea(2,4)] private string[] info;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private NewButton[] swipeButtons;
    [SerializeField] private GameObject[] slides;
    private int currentIndex = 0;


    private void Awake() 
    {
        
    }
    
    private void Start()
    {
        swipeButtons[0].ShowButton(false);
        swipeButtons[1].ShowButton(false);
        SetSwipeButtons();
        ShowSlide();
    }

    private void ShowSlide()
    {
        description.SetText(info[currentIndex]);
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].SetActive( i == currentIndex);
        }
    }

    public void IncreaseIndex(int value)
    {
        currentIndex += value;
        Mathf.Clamp(currentIndex, 0, slides.Length - 1);
        SetSwipeButtons();
        ShowSlide();
    }

    private void SetSwipeButtons()
    {
        if (currentIndex > 0) swipeButtons[0].ShowButton(true);
        if (currentIndex < slides.Length - 1) swipeButtons[1].ShowButton(true);
        swipeButtons[0].hideAfterClick = (currentIndex == 1);
        swipeButtons[1].hideAfterClick = (currentIndex == slides.Length - 2);
    }

}