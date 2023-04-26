using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class NewButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public bool interactable = true;
    Button button;
    [SerializeField] public bool oneClick;
    [SerializeField] public bool hideAfterClick;
    [SerializeField] public bool scaleBack = true;
    private Image image;
    [SerializeField] private UnityEvent clickedAction;
    

    private void Awake() 
    {
        image = GetComponent<Image>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;
        AudioPlayer.Instance.PlayClick();
        interactable = false;
        Vector3 scale = transform.localScale;
        transform.DOScale(transform.localScale * 0.9f, 0.1f).SetUpdate(true)
            .OnComplete(() =>
                {
                    if (hideAfterClick)
                    {
                        ShowButton(false);
                        hideAfterClick = false;
                    }
                    if (scaleBack)
                    {
                        transform.DOScale(scale, 0.1f).SetUpdate(true).OnComplete(() =>
                        {
                            if (!oneClick) interactable = true;
                        });
                    }
                    else if (!oneClick) interactable = true;
                    clickedAction.Invoke();
                    
                });
    }

    private IEnumerator EnableClick()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        interactable = true;
    }

    

    
    public void ShowButton(bool state)
    {
        if (image is null) return;
        if (!state)
        {
            image.enabled = false;
            return;
        }
        image.enabled = true;
        
    }

}
